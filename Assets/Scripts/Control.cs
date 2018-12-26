using Assets.Scripts;
using UnityEngine;

public class Control : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        moveDisk = new GameObject();
    }

    

    private GameObject moveDisk;


    private bool _active;
    private Vector3 _hitPoint;

    //private Vector3 _startingShiftMousePosition;
    private float _verticalOffset;

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
            float enter;

            if (movePlane.Raycast(mouseRay, out enter))
            {
                _active = !_active;
            }
        }

        if (_active)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Camera.current != null)
            {
                var verticalPlane = new Plane(Camera.current.transform.forward, _hitPoint);

                if (verticalPlane.Raycast(mouseRay, out var enter))
                {
                    var raisedPoint = mouseRay.GetPoint(enter);
                    _verticalOffset = raisedPoint.y - _hitPoint.y;
                }
            }
            else
            {
                var movePlane = new Plane(Vector3.up, selectedShip.transform.position);

                if (movePlane.Raycast(mouseRay, out var enter))
                {
                    _hitPoint = mouseRay.GetPoint(enter);

                    var distance = Vector3.Distance(selectedShip.transform.position, _hitPoint);
                    
                    moveDisk.transform.parent = selectedShip.transform;
                    moveDisk.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                    moveDisk.transform.localPosition = Vector3.zero;
                    moveDisk.DrawCircle(distance, 0.1f);
                }
            }
            
        }
        else
        {
            _verticalOffset = 0;
            HideCircle(moveDisk.gameObject);
        }

        if (Input.GetMouseButtonDown(0) && _active)
        {
            
            selectedShip.MoveTo(_hitPoint + new Vector3(0, _verticalOffset, 0));
            _active = false;
            _verticalOffset = 0;
            HideCircle(moveDisk.gameObject);
        }
    }

    public void OnDrawGizmos()
    {
        if (_active)
        {
            Gizmos.DrawWireSphere(_hitPoint + new Vector3(0, _verticalOffset, 0), 0.5f);
        }
    }

    private void HideCircle(GameObject gameObject)
    {
        var line = gameObject.GetComponent<LineRenderer>();
        if (line != null)
        {
            line.enabled = false;
        }
    }

    
}
