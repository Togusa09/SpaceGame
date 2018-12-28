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


    private bool _active;

    void Update()
    {
        var selectedShip = SelectionManager.Instance.GetSelectedShip();
        if (selectedShip == null)
        {
            return;
        }

        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        var movePlane = new Plane(Vector3.up, selectedShip.transform.position);

        if (_active && _moveDisk.LocationSet)
        {
            selectedShip.MoveTo(_moveDisk.Location);
            _active = false;
            _moveDisk.Deactivate();
        }


        if (Input.GetMouseButtonUp(1))
        {
            // Checks that the movement plane for the ship is in view
            if (movePlane.Raycast(mouseRay, out _))
            {
                if (!_active)
                {
                    _active = true;
                    _moveDisk.Activate(selectedShip.gameObject);
                }
                else
                {
                    _active = false;
                    _moveDisk.Deactivate();
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
  
    }
}
