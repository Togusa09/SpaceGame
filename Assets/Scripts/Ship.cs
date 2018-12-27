﻿using System.Collections;
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

        gameObject.DrawCircle(2, 0.1f, Color.green);
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

        var closestTarget = targets.OrderByDescending(x => Vector3.Distance(x.transform.position, transform.position))
            .FirstOrDefault();

        foreach (var turret in _turrets)
        {
            turret.SetTarget(closestTarget);
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

    void OnMouseDown()
    {
        SelectionManager.Instance.SelectShip(this);
    }
}
