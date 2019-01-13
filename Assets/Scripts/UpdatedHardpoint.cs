using UnityEngine;

[System.Serializable]
public class UpdatedHardpoint : MonoBehaviour
{
    public string Name;
    //public Vector3 Position;
    //public Quaternion Rotation;
    public UpdatedTurret Turret;

    public UpdatedHardpoint()
    {
        //Rotation = Quaternion.identity;
    }
}
