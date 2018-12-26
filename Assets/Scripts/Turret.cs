using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Target Target;
    public float Speed = 2.0f;

    private Transform _barrelTransform;
    public float _angle;
    private float _dir = 2;
    
    void Start()
    {
        _barrelTransform = GetComponent<Transform>().Find("Turret/Barrel");
    }

    // Update is called once per frame
    void Update()
    {
        TrackTarget();
        //UpDown();
    }

    private Vector3 _turretCurrent;
    private Vector3 _turretTarget;

    private Vector3 _barrelCurrent;
    private Vector3 _barrelTarget;

    private float _targetDistance;

    private void TrackTarget()
    {
        _turretCurrent = transform.forward;
        _barrelCurrent = _barrelTransform.forward;

        Vector3 relativePos = Target.transform.position - transform.position;
        _targetDistance = relativePos.magnitude;

        var turrentPlane = new Plane(transform.up, transform.position);

        // Rotate turret
        var turretDirection = Vector3.ProjectOnPlane(relativePos, transform.up);
        Quaternion rotation = Quaternion.LookRotation(turretDirection, transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * Speed);

       
        // Set barrel angle

        var barrelDirection = Vector3.ProjectOnPlane(relativePos, transform.right);

        var aimAngle = Vector3.SignedAngle(transform.up, barrelDirection, transform.right);
        Debug.Log(aimAngle);
        if (aimAngle >= 0 && aimAngle <= 90)
        {
            Quaternion barrelRotation = Quaternion.LookRotation(barrelDirection, _barrelTransform.up);
            _barrelTransform.rotation = Quaternion.Lerp(_barrelTransform.rotation, barrelRotation, Time.deltaTime * Speed);
        }
        else
        {
            var barrelRotation = Quaternion.Euler(0, -90, 0);
            _barrelTransform.localRotation = Quaternion.Lerp(_barrelTransform.localRotation, barrelRotation, Time.deltaTime * Speed);
            
        }
    }

    private void OnDrawGizmos()
    {
        var originalColour = Gizmos.color;

        Gizmos.color = Color.green;
        
        Gizmos.DrawRay(_barrelTransform.position, _barrelCurrent.normalized * _targetDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _turretCurrent.normalized * 2);

        Gizmos.color = originalColour;
    }

    private void UpDown()
    {
        var shift = Time.deltaTime * _dir;
        _angle += shift;

        if (_angle > 90)
        {
            _angle = 90;
            _dir = -1.0f;
        };

        if (_angle < 0)
        {
            _angle = 0;
            _dir = 1.0f;
        };

        _barrelTransform.localEulerAngles = new Vector3(0, -_angle, 0);
    }

    public void SetTarget(Target target)
    {
        Target = target;
    }
}
