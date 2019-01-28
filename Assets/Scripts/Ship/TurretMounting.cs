using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Ship
{
    [SelectionBase]
    [RequireComponent(typeof(ShipAppearance))]
    public class TurretMounting : MonoBehaviour
    {
        public float TargetingRange;

        private ShipAppearance _shipAppearance;
        private UpdatedHardpoint[] _hardpoints;
        private List<UpdatedTurret> _turrets;

        private ShipAppearance _target;


        // Start is called before the first frame update
        void Start()
        {
            _shipAppearance = GetComponent<ShipAppearance>();
            _hardpoints = GetComponentsInChildren<UpdatedHardpoint>();
            _turrets = new List<UpdatedTurret>();

            //var turretNodes = FindTransforms(transform, "Hardpoint");
            foreach (var hardpoint in _hardpoints)
            {
                var turret = AttachTurret(hardpoint);
                //turret.SetTarget(Target);
                _turrets.Add(turret);
            }
        }

        private UpdatedTurret AttachTurret(UpdatedHardpoint hardpoint)
        {
            //var turret = (UpdatedTurret)Instantiate(hardpoint.Turret.TurretModel, hardpoint.transform);
            var turret = Instantiate(hardpoint.TurretPrefab, hardpoint.transform);
            return turret;
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var turret in _turrets)
            {
                turret.CanFire = IsTargetInRange();
            }
        }

        void OnDrawGizmos()
        {
        
        }

        public void StopAll()
        {
            SetTarget(null);
        }

        public void ApproachToWeaponsRange(Vector3 position)
        {
            _shipAppearance.ApproachToDistance(position, TargetingRange - 20);
        }

        public void Attack(ShipAppearance target)
        {
            SetTarget(target);
            if (!IsTargetInRange(target))
            {
                ApproachToWeaponsRange(target.transform.position);
            }
        }

        public void SetTarget(ShipAppearance target)
        {
            _target = target;
            foreach (var turret in _turrets)
            {
                //turret.SetTarget(target);
            }
        }

        public bool IsTargetInRange(ShipAppearance target = null)
        {
            if (target == null) target = _target;
            if (target == null) return false;

            var targetRange = Vector3.Distance(transform.position, target.transform.position);
            return targetRange < TargetingRange;
        }

        private Vector3 TargetVectorLocal
        {
            get
            {
                if (_target == null) return Vector3.zero;
                return transform.position - _target.transform.position;
            }
        }
    }
}
