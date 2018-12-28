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


    private bool _moveDiskActive;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDebounce();
        }

        var selectedShip = SelectionManager.Instance.GetSelectedShip();
        if (selectedShip == null)
        {
            return;
        }

        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        var movePlane = new Plane(Vector3.up, selectedShip.transform.position);


        if (_moveDiskActive && Input.GetMouseButtonUp(0))
        {
            selectedShip.MoveTo(_moveDisk.HitPoint);
            _moveDiskActive = false;
            _moveDisk.Deactivate();
        }

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(0)) && Camera.main != null
                                                                         && HasDebounceCompleted())
        {
            _moveDisk.SetDiskMode(MoveDisk.DiskMode.Vertical);
        }
        else
        {
            _moveDisk.SetDiskMode(MoveDisk.DiskMode.Horizontal);
        }
        

        if (Input.GetMouseButtonUp(1))
        {
            // Checks that the movement plane for the ship is in view
            if (movePlane.Raycast(mouseRay, out _))
            {
                if (!_moveDiskActive)
                {
                    _moveDiskActive = true;
                    _moveDisk.Activate(selectedShip.gameObject);
                }
                else
                {
                    _moveDiskActive = false;
                    _moveDisk.Deactivate();
                }
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
  
    }
}
