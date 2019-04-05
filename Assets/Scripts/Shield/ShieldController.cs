using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ShieldController : MonoBehaviour
{
    private static readonly Vector4[] defaultEmptyVector = new Vector4[] { new Vector4(0, 0, 0, 0) };
    public Vector4[] points;

    private Material shieldMaterial;

    public float projectileLife = 0.1f;

    // Use this for initialization
    void Start()
    {
        points = new Vector4[50];
        shieldMaterial = GetComponent<Renderer>().material;
        projectiles = new List<Vector4>();
    }

    // Update is called once per frame
    void Update()
    {
        projectiles = projectiles
            .Select(projectile => new Vector4(projectile.x, projectile.y, projectile.z, projectile.w + (Time.deltaTime / projectileLife)))
            .Where(projectile => projectile.w <= 1)
            .OrderBy(projectile => projectile.w)
            .Take(50)
            .ToList();
        projectiles.ToArray().CopyTo(points, 0);

        shieldMaterial.SetInt("_PointsSize", points.Length);
        if (points.Length <= 0)
        {
            shieldMaterial.SetVectorArray("_Points", defaultEmptyVector);
        }
        else
        {
            shieldMaterial.SetVectorArray("_Points", points);
        }
    }

    private List<Vector4> projectiles;

    public void AddImpact(Vector3 position)
    {
        projectiles.Add(new Vector4(position.x, position.y, position.z));
    }
}