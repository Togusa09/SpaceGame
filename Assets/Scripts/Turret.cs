using UnityEngine;

public class Turret : MonoBehaviour
{
    public Target Target;
    public float Speed = 2.0f;
    public float Range = 5f;

    private Transform _barrelTransform;

    public Shell ShellPrefab;
    
    void Start()
    {
        _barrelTransform = GetComponent<Transform>().Find("Turret/Barrel");
    }

    public bool CanFire = false;

    // Update is called once per frame
    void Update()
    {
        var targetLocked = TrackTarget();

        if (targetLocked && CanFire)
        {
            var time = Time.time;
            if ((time - _lastFireTime) > _fireRate)
            {
                _lastFireTime = time;
                FireCannon();
            }
        }
    }

    private float _lastFireTime;
    private float _fireRate = 1f;
    private float _shellVelocity = 1f;

    private void FireCannon()
    {
        var shell = Instantiate(ShellPrefab, _barrelTransform.position, _barrelTransform.rotation);
        var rigidBody = shell.GetComponent<Rigidbody>();
        rigidBody.velocity = _barrelTransform.transform.forward * _shellVelocity;
    }

    private Vector3 _turretCurrent;
    private Vector3 _turretTarget;

    private Vector3 _barrelCurrent;
    private Vector3 _barrelTarget;

    private float _targetDistance;



    private bool TrackTarget()
    {
        _turretCurrent = transform.forward;
        _barrelCurrent = _barrelTransform.forward;

        if (Target == null) return false;

        Vector3 relativePos = Target.transform.position - transform.position;
        _targetDistance = relativePos.magnitude;

        //var turretPlane = new Plane(transform.up, transform.position);

        // Rotate turret
        var turretDirection = Vector3.ProjectOnPlane(relativePos, transform.up);
        Quaternion rotation = Quaternion.LookRotation(turretDirection, transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * Speed);
       
        // Set barrel angle
        var barrelDirection = Vector3.ProjectOnPlane(relativePos, transform.right);

        var aimAngle = Vector3.SignedAngle(transform.up, barrelDirection, transform.right);
        if (aimAngle >= 0 && aimAngle <= 90)
        {
            var barrelRotation = Quaternion.LookRotation(barrelDirection, _barrelTransform.up);
            _barrelTransform.rotation = Quaternion.Lerp(_barrelTransform.rotation, barrelRotation, Time.deltaTime * Speed);
        }
        else
        {
            var barrelRotation = Quaternion.Euler(0, -90, 0);
            _barrelTransform.localRotation = Quaternion.Lerp(_barrelTransform.localRotation, barrelRotation, Time.deltaTime * Speed);
        }

        return Vector3.Angle(_barrelCurrent, relativePos) < 10.0f;
    }

    private void OnDrawGizmos()
    {
        var originalColour = Gizmos.color;

        Gizmos.color = Color.green;

        if (_barrelTransform != null)
        {
            Gizmos.DrawRay(_barrelTransform.position, _barrelCurrent.normalized * _targetDistance);
        }
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, _turretCurrent.normalized * 2);

        Gizmos.color = originalColour;
    }

    public void SetTarget(Target target)
    {
        if (Target != target)
        {
            Debug.Log("Updating target to " + target);
            Target = target;
        }
    }
}
