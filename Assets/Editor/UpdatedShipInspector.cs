using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpdatedShip))]
public class UpdatedShipInspector : Editor
{
    SerializedProperty hardpoints;
    private UpdatedShip _ship;
    


    private static GUIContent
        moveButtonContent = new GUIContent("\u21b4", "move down"),
        newButtonContent = new GUIContent("+", "duplicate"),
        deleteButtonContent = new GUIContent("-", "delete");



    void OnEnable()
    {
        hardpoints = serializedObject.FindProperty("Hardpoints");
        _ship = (UpdatedShip) target;
    }

    public static void Show(SerializedProperty list)
    {
        

       //EditorGUILayout.PropertyField(list);
        for (int i = 0; i < list.arraySize; i++)
        {
            //var t = list.GetArrayElementAtIndex(i);
            //var obj = t.serializedObject;
            //var prop = t.FindPropertyRelative("transform.position");
            //EditorGUILayout.PropertyField(prop);
            EditorGUI.BeginChangeCheck();

            var item = list.GetArrayElementAtIndex(i);
            EditorGUILayout.LabelField("Hardpoint " + i);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(item.FindPropertyRelative("Position"), true);
            var rotation = item.FindPropertyRelative("Rotation");
            //EditorGUILayout.PropertyField(rotation, true);
            var eulerUpdates = EditorGUILayout.Vector3Field("Euler Rotation", rotation.quaternionValue.eulerAngles);
            if (GUILayout.Button(deleteButtonContent))
            {
                list.DeleteArrayElementAtIndex(i);
            }

            EditorGUI.indentLevel -= 1;
            //EditorGUILayout.LabelField("Test");

            if (EditorGUI.EndChangeCheck())
            {
                rotation.quaternionValue = Quaternion.Euler(eulerUpdates);
            }
        }

        //EditorGUILayout.PropertyField(list);

        //ShowButtons(list);
    }

    private static void ShowButtons(SerializedProperty list)
    {
        GUILayout.Button(moveButtonContent);
        if (GUILayout.Button(newButtonContent))
        {
            list.InsertArrayElementAtIndex(list.arraySize);
        }

        GUILayout.Button(deleteButtonContent);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ShipModel"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("Hardpoints"), true);
        var list = serializedObject.FindProperty("Hardpoints");
        Show(list);
        if (GUILayout.Button(newButtonContent))
        {
            list.InsertArrayElementAtIndex(list.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void OnSceneGUI()
    {
        UpdatedShip example = (UpdatedShip)target;

        EditorGUI.BeginChangeCheck();

        Vector3[] positions = new Vector3[example.Hardpoints.Count];
        Quaternion[] rotations = new Quaternion[example.Hardpoints.Count];

        for (var index = 0; index < example.Hardpoints.Count; index++)
        {
            var hardpoint = example.Hardpoints[index];

            if (hardpoint.Rotation == new Quaternion())
            {
                hardpoint.Rotation = Quaternion.identity;
            }

            positions[index] = hardpoint.Position;
            rotations[index] = hardpoint.Rotation;

            

            if (Tools.current == Tool.Move)
            {
                positions[index] = Handles.PositionHandle(example.transform.position + hardpoint.Position, example.transform.rotation * hardpoint.Rotation)
                    - example.transform.position;
            }
            else if (Tools.current == Tool.Rotate)
            {
                rotations[index] = Handles.RotationHandle(example.transform.rotation * hardpoint.Rotation,
                    example.transform.position + hardpoint.Position);
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            for (var index = 0; index < example.Hardpoints.Count; index++)
            {
                var hardPoint = example.Hardpoints[index];
                if (hardPoint.Position != positions[index] || hardPoint.Rotation != rotations[index])
                {
                    Undo.RecordObject(example, "Updated hard point");
                    hardPoint.Position = positions[index];
                    hardPoint.Rotation = rotations[index];
                }
                
                //hardpoint.Update();
            }
        }
    }
}
