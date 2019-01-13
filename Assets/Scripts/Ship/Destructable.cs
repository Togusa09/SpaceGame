using UnityEngine;

namespace Scripts.Ship
{
    [RequireComponent(typeof(ShipAppearance))]
    public class Destructable : MonoBehaviour
    {
        public int CurrentHealth;
        public int MaxHealth;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentHealth <= 0)
            {
                Explode();
            }
        }

        public void Explode()
        {
            var explosion = transform.Find("Explosion");
            if (!explosion) return;

            var exp = explosion.GetComponent<ParticleSystem>();

            if (exp.isPlaying)
                return;

            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in renderers)
            {
                meshRenderer.enabled = false;
            }

            var skinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var meshRenderer in skinnedRenderers)
            {
                meshRenderer.enabled = false;
            }

            exp.Play();
            Destroy(gameObject, exp.main.duration);

        }
    }
}
