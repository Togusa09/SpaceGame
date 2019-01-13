using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpdatedHardpoint))]
public class UpdatedHardpointInspector : Editor
{
    //SerializedProperty position;
    SerializedProperty rotation;

    private UpdatedHardpoint _hardpoint;

    void OnEnable()
    {
        //position = serializedObject.FindProperty("Position");
        rotation = serializedObject.FindProperty("Transform.rotation");

        //_hardpoint = (UpdatedHardpoint)target;
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("Transform.position"));
        //EditorGUILayout.Vector4Field("Rotation2", rotation.vector4Value);
        //EditorGUI.BeginChangeCheck();



        //serializedObject.ApplyModifiedProperties();
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
