using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Control : MonoBehaviour
{
    public Color UiColor = Color.green;

    // Start is called before the first frame update
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

    void Update()
    {
        var selectedShip = SelectionManager.Instance.GetSelectedShip();
        if (selectedShip == null)
        {
            return;
        }

        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
        {
            var movePlane = new Plane(Vector3.up, selectedShip.transform.position);

            if (movePlane.Raycast(mouseRay, out _))
            {
                _active = !_active;
            }
        }

        if (Input.GetMouseButtonUp(0) && _active)
        {
            selectedShip.MoveTo(HitPoint);
            _active = false;
            _verticalOffset = 0;
            _moveDisk.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            _mouseButtonDownTime = Time.time;
        }

        if (_active)
        {
            _moveDisk.SetActive(true);
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(0)) && Camera.main != null && Time.time - _mouseButtonDownTime > _mouseButtonDebounceInterval)
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

            _planeCursorTop.transform.position = HitPoint;
            _planeCursorBottom.transform.position = _planeHitPoint;
            var line = _vertConnectingLine.GetComponent<LineRenderer>();
            line.positionCount = 4;
            line.SetPositions(new [] { _planeHitPoint, HitPoint, selectedShip.transform.position, _planeHitPoint });
        }
        else
        {
            _verticalOffset = 0;
            _moveDisk.SetActive(false);
        }

      
    }

    public void OnDrawGizmos()
    {
        //if (_active)
        //{
        //    Gizmos.DrawWireSphere(HitPoint, 0.5f);
        //}
    }
}
