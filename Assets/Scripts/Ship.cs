using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Turret TurretPrefab;
    public Target Target;

    private List<Turret> _turrets = new List<Turret>();
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
            _turrets.Add(turret);
        }

        _destination = transform.position;

        _engines = GetComponentsInChildren<Engine>();

        Debug.Log(Size);
        gameObject.DrawCircle(Size / 2, 0.1f, Color.green);
        var line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    private Turret AttachTurret(Transform attachmentNode)
    {
        var turret = (Turret)Instantiate(TurretPrefab, attachmentNode);
        return turret;
    }

    public float turnSpeed = 10.0f;
    public float moveSpeed = 0.01f;
    public float targetingRange = 8.0f;


    // Update is called once per frame
    void Update()
    {
        ShowLineIfSelected();
        ProcessMovement();

        var targetsInRange = Physics.OverlapSphere(transform.position, targetingRange);
        var targets = targetsInRange.Select(x => x.GetComponent<Target>()).Where(x => x != null).ToList();


        foreach (var turret in _turrets)
        {
            turret.CanFire = IsTargetInRange();
        }

        //var closestTarget = targets.OrderByDescending(x => Vector3.Distance(x.transform.position, transform.position))
        //    .FirstOrDefault();

        //foreach (var turret in _turrets)
        //{
        //    turret.SetTarget(closestTarget);
        //}
    }

    public bool IsTargetInRange(Target target = null)
    {
        if (target == null) target = Target;
        if (target == null) return false;

        var targetRange = Vector3.Distance(transform.position, target.transform.position);
        return targetRange < targetingRange;
    }

    public float Size
    {
        get
        {
            return 6.0f;

            // Sizes are coming out inconsistently, unsure why
            //var meshColliders = GetComponentsInChildren<MeshCollider>();

            //if (!meshColliders.Any()) return 0;

            //var bounds = new Bounds();
            //foreach (var meshCollider in meshColliders)
            //{
            //    bounds.Encapsulate(meshCollider.bounds);
            //}

            //var size = bounds.size;
            //if (size.x > size.y)
            //{
            //    return size.x;
            //}

            //return size.y;
        }
    }

    public void SetTarget(Target target)
    {
        Target = target;
        foreach (var turret in _turrets)
        {
            turret.SetTarget(target);
        }
    }

    private void ProcessMovement()
    {
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

    private void ShowLineIfSelected()
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_destination, 1.0f);
        }

        Gizmos.color = Color.blue;
        
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }

    public void MoveTo(Vector3 destination)
    {
        Debug.Log("Moving to " + destination);
        _destination = destination;
    }
}
