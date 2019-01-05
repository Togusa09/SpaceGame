using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Turret TurretPrefab;
    public Ship Target;

    public bool IsHostile;
    public bool IsFixed;

    private List<Turret> _turrets = new List<Turret>();
    private Vector3 _destination;

    private Engine[] _engines;

    public bool Selected = false;

    private GameObject _destinationCircle;
    private GameObject _destinationLine;

    private Vector3 DestinationVectorLocal => transform.position - _destination;
    private float DestinationDistance => Vector3.Distance(transform.position, _destination);
    private Vector3 TargetVectorLocal
    {
        get
        {
            if (Target == null) return Vector3.zero;
            return transform.position - Target.transform.position;
        }
    }

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
        gameObject.DrawCircle(Size / 2, 1f, Color.green);
        var line = GetComponent<LineRenderer>();
        line.enabled = false;

        _destinationCircle = new GameObject("DestinationCircle");
        _destinationCircle.transform.SetParent(this.transform);
        _destinationCircle.DrawCircle(10, 1f, Color.green);
        _destinationCircle.SetActive(false);
        _destinationLine = new GameObject("DestinationLine");
        _destinationLine.transform.SetParent(this.transform);
        var lineRenderer = _destinationLine.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.enabled = true;
        lineRenderer.material.color = Color.green;
        _destinationLine.SetActive(false);

        _shield = transform.Find("Shield").gameObject;
    }

    private Turret AttachTurret(Transform attachmentNode)
    {
        var turret = (Turret)Instantiate(TurretPrefab, attachmentNode);
        return turret;
    }

    public float turnSpeed = 10.0f;
    public float moveSpeed = 0.01f;
    public float targetingRange = 80.0f;

    public int MaxHealth;
    public int CurrentHealth;
    public int MaxShield;
    public int CurrentShield;

    private GameObject _shield;

    // Update is called once per frame
    void Update()
    {
        ShowLineIfSelected();
        ProcessMovement();

        var targetsInRange = Physics.OverlapSphere(transform.position, targetingRange);
        var targets = targetsInRange.Select(x => x.GetComponent<Ship>()).Where(x => x != null).ToList();

        if (CurrentShield <= 0)
        {
            _shield?.SetActive(false);
        }
        else
        {
            _shield?.SetActive(true);
        }

        foreach (var turret in _turrets)
        {
            turret.CanFire = IsTargetInRange();
        }

        if (Mathf.Abs(DestinationDistance) > 1f)
        {
            _destinationCircle.transform.position = _destination;

            var moveDir = DestinationVectorLocal;
            var circleEdge = Vector3.Normalize(new Vector3(moveDir.x, 0, moveDir.z)) * 10f;

            var lineRenderer = _destinationLine.GetComponent<LineRenderer>();
            var points = new[] { _destination + circleEdge, transform.position};
            lineRenderer.SetPositions(points);
            _destinationCircle.transform.rotation = Quaternion.identity;;

            _destinationCircle.SetActive(true);
            _destinationLine.SetActive(true);
        }
        else
        {
            _destinationCircle.SetActive(false);
            _destinationLine.SetActive(false);
        }
    }

    public bool IsTargetInRange(Ship target = null)
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
            return 200.0f;

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

    public void SetTarget(Ship target)
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

        var dirVector = DestinationVectorLocal;

        if (dirVector == Vector3.zero)
        {
            foreach (var engine in _engines)
            {
                engine.StopEngine();
            }
            return;
        }

        var direction = Quaternion.LookRotation(dirVector, Vector3.up);

        if (DestinationDistance > 0.1f)
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
        Gizmos.color = Color.blue;
        
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }

    public void Attack(Ship target)
    {
        SetTarget(target);
        if (!IsTargetInRange(target))
        {
            ApproachToWeaponsRange(target.transform.position);
        }
    }

    public void MoveTo(Vector3 destination)
    {
        Debug.Log("Moving to " + destination);
        _destination = destination;
    }

    //public void ApproachTarget(Ship target)
    //{
    //    ApproachToDistance(target.transform.position, 40.0f + Size/2);
    //}

    public void ApproachTarget(Ship ship)
    {
        ApproachToDistance(ship.transform.position, ship.Size);
    }

    public void ApproachToWeaponsRange(Vector3 position)
    {
        ApproachToDistance(position, targetingRange - 20);
    }

    public void ApproachToDistance(Vector3 position, float distance)
    {
        var dir = transform.position - position;
        var target = new Ray(position, dir);
        Debug.DrawRay(position, dir, Color.blue, 6);
        var destination = target.GetPoint(distance);
        MoveTo(destination);
    }

    public void StopAll()
    {
        StopMovement();
        SetTarget(null);
    }

    public void StopMovement()
    {
        _destination = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        var shell = collision.gameObject.GetComponent<Shell>();

        if (shell)
        {
            CurrentHealth -= shell.Damage;

            Debug.Log("Projectile hit");
            Destroy(collision.gameObject);
        }
    }
}
