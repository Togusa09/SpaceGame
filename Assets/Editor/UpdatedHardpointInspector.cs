using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpdatedHardpoint))]
public class UpdatedHardpointInspector : Editor
{
    SerializedProperty position;
    SerializedProperty rotation;

    private UpdatedHardpoint _hardpoint;

    void OnEnable()
    {
        position = serializedObject.FindProperty("Position");
        rotation = serializedObject.FindProperty("Rotation");

        //_hardpoint = (UpdatedHardpoint)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(position);
        //EditorGUILayout.Vector4Field("Rotation2", rotation.vector4Value);
        EditorGUI.BeginChangeCheck();

        var updatedRotation = EditorGUILayout.Vector3Field("Euler Rotation", rotation.quaternionValue.eulerAngles);
        if (EditorGUI.EndChangeCheck())
        {
            //rotation;
        }

        serializedObject.ApplyModifiedProperties();
    }

    //public void OnSceneGUI()
    //{
    //    var t = target as UpdatedHardpoint;

    //    EditorGUI.BeginChangeCheck();
    //    Vector3 pos = Handles.PositionHandle(t.Position, Quaternion.identity);
    //    if (EditorGUI.EndChangeCheck())
    //    {
    //        Undo.RecordObject(target, "Move point");
    //        t.Position = pos;
    //        //t.Update();
    //    }
    //}
}
