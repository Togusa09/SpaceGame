using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ShipAppearance : MonoBehaviour
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
}
