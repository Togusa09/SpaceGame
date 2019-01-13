using UnityEngine;

[System.Serializable]
public class UpdatedHardpoint : MonoBehaviour
{
    public string Name;
    public bool CanFire;

    
    public UpdatedTurret Turret;

    public UpdatedHardpoint()
    {
        //Rotation = Quaternion.identity;
    }
}
