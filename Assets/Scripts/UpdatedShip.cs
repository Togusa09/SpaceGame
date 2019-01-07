using System.Collections.Generic;
using UnityEngine;

public class UpdatedShip : MonoBehaviour
{
    public GameObject ShipModel;
    public List<UpdatedHardpoint> Hardpoints = new List<UpdatedHardpoint> { new UpdatedHardpoint() };

    //public Vector3[] Hardpoint;

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
        if (ShipModel)
        {
            var mesh = ShipModel.GetComponentsInChildren<MeshFilter>();
            foreach (var meshFilter in mesh)
            {
                Gizmos.DrawWireMesh(meshFilter.sharedMesh, transform.position, transform.rotation * Quaternion.Euler(-90, 0, 0));
                //Gizmos.DrawMesh(meshFilter.sharedMesh, transform.position, transform.rotation * Quaternion.Euler(-90, 0, 0));
            }
        }

        if (Hardpoints != null)
        {
            foreach (var hardpoint in Hardpoints)
            {
                Gizmos.DrawSphere(transform.position + hardpoint.Position, 5f);
            }
        }
    }
}
