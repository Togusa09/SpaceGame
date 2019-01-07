using UnityEngine;

[System.Serializable]
public class UpdatedHardpoint //: MonoBehaviour
{
    public Vector3 Position;
    public Quaternion Rotation;

    public UpdatedHardpoint()
    {
        Rotation = Quaternion.identity;
    }
}
