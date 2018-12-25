using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Ship CurrentShip;

    private bool active;
    private Vector3? hitPoint = null;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var movePlane = new Plane(Vector3.up, transform.position);
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
            var movePlane = new Plane(Vector3.up, transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (movePlane.Raycast(ray, out var enter))
            {
                hitPoint = ray.GetPoint(enter);

                var distance = Vector3.Distance(CurrentShip.transform.position, hitPoint.Value);
                DrawCircle(gameObject, distance, 0.1f);
            }
        }
        else
        {
            hitPoint = null;
            HideCircle();
        }

        if (Input.GetMouseButtonDown(0) && hitPoint.HasValue && CurrentShip != null)
        {
            CurrentShip.MoveTo(hitPoint.Value);
            hitPoint = null;
            active = false;
            HideCircle();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(10, 0.1f, 10));
        if (hitPoint.HasValue)
        {
            Gizmos.DrawWireSphere(hitPoint.Value, 0.5f);
        }
    }

    private void HideCircle()
    {
        var line = GetComponent<LineRenderer>();
        if (line != null)
        {
            line.enabled = false;
        }
    }

    public static void DrawCircle(GameObject container, float radius, float lineWidth)
    {
        var segments = 360;
        var line = container.GetComponent<LineRenderer>();
        if (line == null)
        {
            line = container.AddComponent<LineRenderer>();
        }

        line.enabled = true;

        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.SetPositions(points);
    }
}
