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

    private void TrackTarget()
    {
        Vector3 relativePos = transform.position - Target.transform.position;

        // Rotate turret
        var turretDirection = Vector3.ProjectOnPlane(relativePos, transform.up);
        Quaternion rotation = Quaternion.LookRotation(turretDirection, transform.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * Speed);

        

        // Set barrel angle
        var barrelDirection = Vector3.ProjectOnPlane(relativePos, transform.right);
        Quaternion barrelRotation = Quaternion.LookRotation(barrelDirection, _barrelTransform.up) * Quaternion.Euler(0, 90, 0);
        _barrelTransform.rotation = Quaternion.Lerp(_barrelTransform.rotation, barrelRotation, Time.deltaTime * Speed);

        var angle = Quaternion.Angle(barrelRotation, Quaternion.AngleAxis(0, transform.up));
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
