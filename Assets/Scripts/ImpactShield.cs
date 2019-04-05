using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ImpactShield : MonoBehaviour
{
    public Ship Ship;

    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        //var colliders = GetComponentsInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision collision)
    {
        Collider myCollider = collision.contacts[0].thisCollider;

        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal * 10, Color.white);

        // Now do whatever you need with myCollider.
        // (If multiple colliders were involved in the collision, 
        // you can find them all by iterating through the contacts)

        var shell = collision.gameObject.GetComponent<Shell>();

        var shieldController = myCollider.GetComponent<ShieldController>();
        var collisionPoint = collision.contacts[0].point;
        
        var localPosition = myCollider.transform.InverseTransformPoint(collisionPoint);
        Debug.Log(localPosition);

        shieldController.AddImpact(localPosition/ 2);

        Ship.CurrentShield -= shell.Damage;
    }
}
