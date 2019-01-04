
using UnityEngine;

namespace Assets.Scripts.UI
{
    public enum MoveTargetType
    {
        Position,
        Ship,
        Target
    }

    public class MovementInformation
    {
        public MoveTargetType MoveTargetType { get; private set; }

        public Vector3 Location { get; private set; }
        public Target Target { get; private set; }
        public Ship Ship { get; private set; }

        private void SetAllNull()
        {
            Location = Vector3.zero;
            Target = null;
            Ship = null;
        }

        public void SetDestination(Vector3 location)
        {
            SetAllNull();
            Location = location;
            MoveTargetType = MoveTargetType.Position;
        }

        public void SetDestination(Target target)
        {
            SetAllNull();
            Target = target;
            MoveTargetType = MoveTargetType.Target;
        }
        public void SetDestination(Ship ship)
        {
            SetAllNull();
            Ship = ship;
            MoveTargetType = MoveTargetType.Ship;
        }
    }
}
