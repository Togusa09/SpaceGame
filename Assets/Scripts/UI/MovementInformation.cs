
using UnityEngine;

namespace Assets.Scripts.UI
{
    public enum MoveTargetType
    {
        Position,
        Ship
    }

    public class MovementInformation
    {
        public MoveTargetType MoveTargetType { get; private set; }

        public Vector3 Location { get; private set; }
        public OldShip OldShip { get; private set; }

        private void SetAllNull()
        {
            Location = Vector3.zero;
            OldShip = null;
        }

        public void SetDestination(Vector3 location)
        {
            SetAllNull();
            Location = location;
            MoveTargetType = MoveTargetType.Position;
        }

        public void SetDestination(OldShip oldShip)
        {
            SetAllNull();
            OldShip = oldShip;
            MoveTargetType = MoveTargetType.Ship;
        }
    }
}
