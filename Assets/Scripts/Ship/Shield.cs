using UnityEngine;

namespace Scripts.Ship
{
    [RequireComponent(typeof(ShipAppearance))]
    public class Shield : MonoBehaviour
    {
        public int CurrentShield;
        public int MaxShield;

        //private GameObject _shield;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentShield <= 0)
            {
                //_shield?.SetActive(false);
            }
            else
            {
                //_shield?.SetActive(true);
            }
        }
    }
}
