using Assets.Scripts;
using UnityEngine;

public class Control : MonoBehaviour
{
    public Color UiColor = Color.green;

    void Start()
    {
        var moveDiskGameObject = new GameObject("MoveDisk");
        _moveDisk = moveDiskGameObject.AddComponent<MoveDisk>();
        _moveDisk.enabled = true;
    }

    private MoveDisk _moveDisk;
    private Ray? TargetRay;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDebounce();
        }

        //TargetRay = null;

        var selectionManager = SelectionManager.Instance;
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // Find out if mouse is current over a ship
        Ship targetShip = null;
        Target targetTarget = null;

        if (Physics.Raycast(mouseRay, out var hitInfo))
        {
            targetShip = hitInfo.transform.GetComponent<Ship>();
            targetTarget = hitInfo.transform.GetComponent<Target>();
        }

        // Simple action
        if (Input.GetMouseButtonUp(0))
        {
            

            if (targetShip != null)
            {
                SelectionManager.Instance.SelectShip(targetShip);
                _moveDisk.Deactivate();
            }
            else if (_moveDisk.IsActive)
            {
                var selectedShip = selectionManager.GetSelectedShip();
                selectedShip.MoveTo(_moveDisk.HitPoint);

                _moveDisk.Deactivate();
            }
        }

        // Compound action
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(0)) && Camera.main != null
                                                                         && HasDebounceCompleted())
        {
            _moveDisk.SetDiskMode(MoveDisk.DiskMode.Vertical);
        }
        else
        {
            _moveDisk.SetDiskMode(MoveDisk.DiskMode.Horizontal);
        }

        // Simple action
        if (Input.GetMouseButtonUp(1))
        {
            var selectedShip = selectionManager.GetSelectedShip();
            

            if (targetTarget != null && selectedShip != null)
            {
                if (targetTarget.IsHostile)
                {
                    selectedShip.SetTarget(targetTarget);
                    var targetInRange = selectedShip.IsTargetInRange(targetTarget);
                    if (!targetInRange)
                    {
                        var dir = selectedShip.transform.position - targetTarget.transform.position;
                        TargetRay = new Ray(targetTarget.transform.position, dir);
                        var destination = TargetRay.Value.GetPoint(selectedShip.targetingRange - 20);
                        selectedShip.MoveTo(destination);
                    }
                }
                else
                {
                    var dir = selectedShip.transform.position - targetTarget.transform.position;
                    TargetRay = new Ray(targetTarget.transform.position, dir);
                    var destination = TargetRay.Value.GetPoint((selectedShip.Size / 2) );
                    selectedShip.MoveTo(destination);
                }
                
            }
            else if (!_moveDisk.IsActive && selectedShip != null)
            {
                var movePlane = new Plane(Vector3.up, selectedShip.transform.position);

                // Checks that the movement plane for the ship is in view. If the camera is below the plane of movement,
                // the disk can't be show. Could be dealt with using advanced behaviours, but just keeping simple for moment
                if (movePlane.Raycast(mouseRay, out _))
                {
                    _moveDisk.Activate(selectedShip.gameObject);
                }
                else
                {
                    Debug.LogWarning("Move plane not visible to camera. ");
                }
            }
            else
            {
                _moveDisk.Deactivate();
            }
            
        }
    }

    private float _mouseButtonDownTime;
    private float _mouseButtonDebounceInterval = 0.1f;

    private void StartDebounce()
    {
        _mouseButtonDownTime = Time.time;
    }
    private bool HasDebounceCompleted()
    {
        return Time.time - _mouseButtonDownTime > _mouseButtonDebounceInterval;
    }

    public void OnDrawGizmos()
    {
        if (TargetRay.HasValue)
        {
            Gizmos.DrawRay(TargetRay.Value);
        }
    }
}
