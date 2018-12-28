using Assets.Scripts;
using UnityEngine;

public class MoveDisk : MonoBehaviour
{
    public Color UiColor = Color.green;

    void Start()
    {
        //_moveDisk = new GameObject("MoveDisk");
        _planeCursorTop = new GameObject("PlaneCursor");
        _planeCursorTop.DrawCircle(1, 0.1f, UiColor);
        _planeCursorTop.transform.SetParent(transform);

        _planeCursorBottom = new GameObject("PlaneCursor");
        _planeCursorBottom.DrawCircle(1, 0.1f, UiColor);
        _planeCursorBottom.transform.SetParent(transform);

        _vertConnectingLine = new GameObject("ConnectingLine");
        _vertConnectingLine.transform.SetParent(transform);
        var lineRenderer = _vertConnectingLine.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.enabled = true;
        lineRenderer.material.color = UiColor;

        //this.gameObject.SetActive(false);
    }

    //private GameObject _moveDisk;
    private GameObject _planeCursorTop;
    private GameObject _planeCursorBottom;
    private GameObject _vertConnectingLine;

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

    private GameObject _anchorObject;
    public void Activate(GameObject targetObject)
    {
        _anchorObject = targetObject;
        LocationSet = false;
        _verticalOffset = 0;
        gameObject.SetActive(true);
    }

    public bool LocationSet { get; private set; }
    public Vector3 Location { get; private set; }

    public void Deactivate()
    {
        LocationSet = false;
        _verticalOffset = 0;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (_anchorObject == null) return;
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        var movePlane = new Plane(Vector3.up, _anchorObject.transform.position);

        if (Input.GetMouseButtonUp(0))
        {
            Location = HitPoint;
            LocationSet = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartDebounce();
        }

        transform.SetParent(_anchorObject.transform);
        transform.localPosition = Vector3.zero;

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
            if (movePlane.Raycast(mouseRay, out var enter))
            {
                _planeHitPoint = mouseRay.GetPoint(enter);

                var distance = Vector3.Distance(_anchorObject.transform.position, _planeHitPoint);

                transform.parent = _anchorObject.transform;
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                transform.localPosition = Vector3.zero;
                gameObject.DrawCircle(distance, 0.1f, UiColor);
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
        line.SetPositions(new[] { _planeHitPoint, HitPoint, transform.position, _planeHitPoint });
    }
}
