using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class OldShip : MonoBehaviour
{
    public Turret TurretPrefab;
    public OldShip Target;

    public bool IsHostile;
    public bool IsFixed;

    private List<Turret> _turrets = new List<Turret>();
    private Vector3 _destination;

    private Engine[] _engines;

    public bool Selected = false;



  

    // Start is called before the first frame update
    void Start()
    {
       
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
