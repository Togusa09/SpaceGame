using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Turret TurretPrefab;
    public Target Target;

    private List<Turret> _turrets;
    private Vector3 _destination;

    private Engine[] _engines;

    public bool Selected = false;

    // Start is called before the first frame update
    void Start()
    {
        var turretNodes = FindTransforms(transform, "Hardpoint");
        foreach (var node in turretNodes)
        {
            var turret = AttachTurret(node);
            turret.SetTarget(Target);
        }

        _destination = transform.position;

        _engines = GetComponentsInChildren<Engine>();

        gameObject.DrawCircle(4, 0.1f);
        var line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    private Turret AttachTurret(Transform attachmentNode)
    {
        var turret = (Turret)Instantiate(TurretPrefab, attachmentNode);
        return turret;
    }

    private float turnSpeed = 10.0f;
    private float moveSpeed = 0.01f;

    // Update is called once per frame
    void Update()
    {
        var line = GetComponent<LineRenderer>();
        if (Selected)
        {
            if (line != null)
            {
                line.enabled = true;
            }
        }
        else
        {
            if (line != null)
            {
                line.enabled = false;
            }
        }

        // https://answers.unity.com/questions/29751/gradually-moving-an-object-up-to-speed-rather-then.html

        var distance = Vector3.Distance(_destination, transform.position);

        var dirVector = transform.position - _destination;

        if (dirVector == Vector3.zero)
        {
            foreach (var engine in _engines)
            {
                engine.StopEngine();
            }
            return;
        }


        var direction = Quaternion.LookRotation(dirVector, Vector3.up);

        if (Mathf.Abs(distance) > 0.1f)
        { 
            // Lerp rotation
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, direction, Time.deltaTime * turnSpeed);
        }

        var remainingAngle = Quaternion.Angle(transform.rotation, direction);
        if (Mathf.Abs(remainingAngle) < 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, moveSpeed);

            foreach (var engine in _engines)
            {
                engine.StartEngine();
            }
        }
    }

    public List<Transform> FindTransforms(Transform parent, string name, List<Transform> itemList = null)
    {
        var foundItems = itemList ?? new List<Transform>();
        if (parent.name.StartsWith(name))
        {
            foundItems.Add(parent);
        }

        foreach (Transform child in parent)
        {
            FindTransforms(child, name, foundItems);
        }

        return foundItems;
    }

    public void OnDrawGizmos()
    {
        var distance = Vector3.Distance(_destination, transform.position);
        if (Mathf.Abs(distance) > 0.1f)
        {
            Gizmos.DrawWireSphere(_destination, 2.0f);
        }
    }

    //public Transform FindTransform(Transform parent, string name)
    //{
    //    if (parent.name.Equals(name)) return parent;
    //    foreach (Transform child in parent)
    //    {
    //        Transform result = FindTransform(child, name);
    //        if (result != null) return result;
    //    }
    //    return null;
    //}

    public void MoveTo(Vector3 destination)
    {
        _destination = destination;
    }

    void OnMouseDown()
    {
        SelectionManager.Instance.SelectShip(this);
    }
}
