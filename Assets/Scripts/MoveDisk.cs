using Assets.Scripts;
using UnityEngine;

public class MoveDisk : MonoBehaviour
{
    public Color UiColor = Color.green;

    void Start()
    {
        _planeCursorTop = new GameObject("PlaneCursor");
        _planeCursorTop.DrawCircle(10, 1f, UiColor);
        _planeCursorTop.transform.SetParent(transform);

        _planeCursorBottom = new GameObject("PlaneCursor");
        _planeCursorBottom.DrawCircle(10, 1f, UiColor);
        _planeCursorBottom.transform.SetParent(transform);

        _verticalConnectingLine = new GameObject("ConnectingLine");
        _verticalConnectingLine.transform.SetParent(transform);
        var lineRenderer = _verticalConnectingLine.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.enabled = true;
        lineRenderer.material.color = UiColor;

        gameObject.SetActive(false);
    }

    public bool IsActive => gameObject.activeSelf;

    private GameObject _planeCursorTop;
    private GameObject _planeCursorBottom;
    private GameObject _verticalConnectingLine;

    private Vector3 _planeHitPoint;
    public Vector3 HitPoint => _planeHitPoint + new Vector3(0, _verticalOffset, 0);
    private float _verticalOffset;

    private GameObject _anchorObject;
    public void Activate(GameObject targetObject)
    {
        _anchorObject = targetObject;
        _verticalOffset = 0;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        //_verticalOffset = 0;
        gameObject.SetActive(false);
    }

    public enum DiskMode
    {
        Horizontal,
        Vertical
    }

    private DiskMode _diskMode;

    public void SetDiskMode(DiskMode diskMode)
    {
        _diskMode = diskMode;
    }

    void Update()
    {
        if (_anchorObject == null) return;

        // Undecided if this should be passed in? Maybe if dealing with multiple cameras, just wanted
        // to extra the click logic for the moment
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        var movePlane = new Plane(Vector3.up, _anchorObject.transform.position);

        transform.SetParent(_anchorObject.transform);
        transform.localPosition = Vector3.zero;

        if (_diskMode == DiskMode.Vertical)
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
                gameObject.DrawCircle(distance, 1f, UiColor);
            }
        }

        UpdateMoveDiskDisplay();
    }

    private void UpdateMoveDiskDisplay()
    {
        _planeCursorTop.transform.position = HitPoint;
        _planeCursorBottom.transform.position = _planeHitPoint;
        var line = _verticalConnectingLine.GetComponent<LineRenderer>();
        line.positionCount = 4;
        line.SetPositions(new[] { _planeHitPoint, HitPoint, transform.position, _planeHitPoint });
    }
}
