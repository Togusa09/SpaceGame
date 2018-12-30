using UnityEngine;

public class Target : MonoBehaviour
{
    public bool IsHostile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile hit");
        Destroy(collision.gameObject);
    }
}
