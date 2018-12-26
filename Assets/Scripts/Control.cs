using Assets.Scripts;
using UnityEngine;

public class Control : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        moveDisk = new GameObject();
    }

    private bool active;
    private Vector3? hitPoint = null;

    private GameObject moveDisk;

    void Update()
    {
        var selectedShip = SelectionManager.Instance.GetSelectedShip();
        if (selectedShip == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            var movePlane = new Plane(Vector3.up, selectedShip.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;

            if (movePlane.Raycast(ray, out enter))
            {
                active = !active;
            }
        }

        hitPoint = null;

        if (active)
        {
            var movePlane = new Plane(Vector3.up, selectedShip.transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (movePlane.Raycast(ray, out var enter))
            {
                hitPoint = ray.GetPoint(enter);

                var distance = Vector3.Distance(selectedShip.transform.position, hitPoint.Value);

                moveDisk.transform.parent = selectedShip.transform;
                moveDisk.transform.localPosition = Vector3.zero;
                moveDisk.DrawCircle(distance, 0.1f);
            }
        }
        else
        {
            hitPoint = null;
            HideCircle(moveDisk.gameObject);
        }

        if (Input.GetMouseButtonDown(0) && hitPoint.HasValue)
        {
            selectedShip.MoveTo(hitPoint.Value);
            hitPoint = null;
            active = false;
            HideCircle(moveDisk.gameObject);
        }
    }

    public void OnDrawGizmos()
    {
        if (hitPoint.HasValue)
        {
            Gizmos.DrawWireSphere(hitPoint.Value, 0.5f);
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
