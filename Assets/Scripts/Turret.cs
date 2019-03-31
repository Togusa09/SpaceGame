using UnityEngine;

public class Turret : MonoBehaviour
{
    public Ship Target;
    public float Speed = 2.0f;
    public float Range = 5f;
    public int DamagePerShot = 2;

    private Transform _barrelTransform;

    public Shell ShellPrefab;
    
    void Start()
    {
        _barrelTransform = GetComponent<Transform>().Find("Turret/Armature/Turret/Barrel");

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
    private float _shellVelocity = 20f;

    private void FireCannon()
    {
        var shell = Instantiate(ShellPrefab, _barrelTransform.position, _barrelTransform.rotation * Quaternion.LookRotation(Vector3.left));
        shell.Damage = DamagePerShot;
        var rigidBody = shell.GetComponent<Rigidbody>();
        rigidBody.velocity = -_barrelTransform.transform.right * _shellVelocity;
    }

    private Vector3 _turretTarget;

    private Vector3 _barrelTarget;



    private bool TrackTarget()
    {
        if (Target == null) return false;

        Vector3 relativePos = Target.transform.position - transform.position;

        // Rotate turret
        var turretDirection = Vector3.ProjectOnPlane(relativePos, transform.up);
        Quaternion rotation = Quaternion.LookRotation(turretDirection, transform.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * Speed);

        Debug.DrawRay(transform.position, transform.forward * 20, Color.blue);

        // Set barrel angle
        var barrelDirection = Vector3.ProjectOnPlane(relativePos, transform.right);
        Debug.DrawRay(_barrelTransform.position, barrelDirection * 20, Color.yellow);

        var aimAngle = Vector3.SignedAngle(transform.up, barrelDirection, transform.right);
        
        if (aimAngle >= 0 && aimAngle <= 90)
        {
            _barrelTransform.localRotation = Quaternion.Euler(0, 0, aimAngle);
        }
        else
        {
            _barrelTransform.localRotation = Quaternion.Euler(0, 0, 90);
        }

        Debug.DrawRay(_barrelTransform.position, _barrelTransform.rotation * -Vector3.right * 30, Color.cyan);
        Debug.DrawRay(_barrelTransform.position, relativePos.normalized * 30, Color.magenta);

        var targetAngle = Vector3.Angle(_barrelTransform.rotation * -Vector3.right, relativePos);
        return targetAngle < 10.0f;
    }

    public void SetTarget(Ship target)
    {
        if (Target != target)
        {
            Debug.Log("Updating target to " + target);
            Target = target;
        }
    }
}
