using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(ShipAppearance))]
public class TurretMounting : MonoBehaviour
{
    public GameObject ShipModel;

    // Start is called before the first frame update
    void Start()
    {
        var model = Instantiate(ShipModel, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        
    }
}
