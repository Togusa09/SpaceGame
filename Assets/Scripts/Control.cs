using Assets.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class Control : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IMoveHandler
{
    public Color UiColor = Color.green;

    void Start()
    {

        _moveDisk = new GameObject("MoveDisk");
        _planeCursorTop = new GameObject("PlaneCursor");
        _planeCursorTop.DrawCircle(1, 0.1f, UiColor);
        _planeCursorTop.transform.SetParent(_moveDisk.transform);

        _planeCursorBottom = new GameObject("PlaneCursor");
        _planeCursorBottom.DrawCircle(1, 0.1f, UiColor);
        _planeCursorBottom.transform.SetParent(_moveDisk.transform);

        _vertConnectingLine = new GameObject("ConnectingLine");
        _vertConnectingLine.transform.SetParent(_moveDisk.transform);
        var lineRenderer = _vertConnectingLine.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.enabled = true;
        lineRenderer.material.color = UiColor;

        _moveDisk.SetActive(false);

    }


    private GameObject _moveDisk;
    private GameObject _planeCursorTop;
    private GameObject _planeCursorBottom;
    private GameObject _vertConnectingLine;

    private bool _active;
    private Vector3 _planeHitPoint;
    private Vector3 HitPoint => _planeHitPoint + new Vector3(0, _verticalOffset, 0);
    private float _verticalOffset;

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

    void Update()
    {
        var selectedShip = SelectionManager.Instance.GetSelectedShip();
        if (selectedShip == null)
        {
            return;
        }

        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Input.GetMouseButtonUp(1))
        //{
        //    // Checks that the movement plane for the ship is in view
        //    var movePlane = new Plane(Vector3.up, selectedShip.transform.position);
        //    if (movePlane.Raycast(mouseRay, out _))
        //    {
        //        if (_active)
        //        {
        //            DeactivateMoveDisk();
        //        }
        //        else
        //        {
        //            ActivateMoveDisc();
        //        }
        //    }
        //}

        if (!_active) return;
        
        //if (Input.GetMouseButtonUp(0))
        //{
        //    selectedShip.MoveTo(HitPoint);
        //    DeactivateMoveDisk();
        //    return;
        //}

        if (Input.GetMouseButtonDown(0))
        {
            StartDebounce();
        }

        _moveDisk.transform.SetParent(selectedShip.transform);
        _moveDisk.transform.localPosition = Vector3.zero;
        

        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(0)) && Camera.main != null 
                                                                         && HasDebounceCompleted())
        {
            var verticalPlane = new Plane(Camera.main.transform.forward, _planeHitPoint);

            if (verticalPlane.Raycast(mouseRay, out var enter))
            {
                var raisedPoint = mouseRay.GetPoint(enter);
                _verticalOffset = raisedPoint.y - _planeHitPoint.y;
            }
        }
        else
        {
            var movePlane = new Plane(Vector3.up, selectedShip.transform.position);

            if (movePlane.Raycast(mouseRay, out var enter))
            {
                _planeHitPoint = mouseRay.GetPoint(enter);

                var distance = Vector3.Distance(selectedShip.transform.position, _planeHitPoint);
                
                _moveDisk.transform.parent = selectedShip.transform;
                _moveDisk.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                _moveDisk.transform.localPosition = Vector3.zero;
                _moveDisk.DrawCircle(distance, 0.1f, UiColor);
            }
        }

        UpdateMoveDiscDisplay();
    }

    private void UpdateMoveDiscDisplay()
    {
        _planeCursorTop.transform.position = HitPoint;
        _planeCursorBottom.transform.position = _planeHitPoint;
        var line = _vertConnectingLine.GetComponent<LineRenderer>();
        line.positionCount = 4;
        line.SetPositions(new[] { _planeHitPoint, HitPoint, _moveDisk.transform.position, _planeHitPoint });
    }

    private void DeactivateMoveDisk()
    {
        _verticalOffset = 0;
        _moveDisk.SetActive(false);
        _active = false;
    }

    private void ActivateMoveDisc()
    {
        _verticalOffset = 0;
        _moveDisk.SetActive(true);
        _active = true;
    }

    public void OnDrawGizmos()
    {
  
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var selectedShip = SelectionManager.Instance.GetSelectedShip();
        if (selectedShip == null)
        {
            return;
        }

        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            var movePlane = new Plane(Vector3.up, selectedShip.transform.position);
            if (movePlane.Raycast(mouseRay, out _))
            {
                if (_active)
                {
                    DeactivateMoveDisk();
                }
                else
                {
                    ActivateMoveDisc();
                }
            }
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_active)
            {
                selectedShip.MoveTo(HitPoint);
                DeactivateMoveDisk();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down");
    }

    public void OnMove(AxisEventData eventData)
    {
        Debug.Log("Moving");
    }
}
