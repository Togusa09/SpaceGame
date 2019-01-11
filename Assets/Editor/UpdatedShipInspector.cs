using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

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

    public static void Show(UpdatedHardpoint[] list)
    {
        

       //EditorGUILayout.PropertyField(list);
        for (int i = 0; i < list.Count(); i++)
        {
            //var t = list.GetArrayElementAtIndex(i);
            //var obj = t.serializedObject;
            //var prop = t.FindPropertyRelative("transform.position");
            //EditorGUILayout.PropertyField(prop);
            EditorGUI.BeginChangeCheck();

            //var item = list.GetArrayElementAtIndex(i);
            var item = new SerializedObject(list[i]);
            EditorGUILayout.LabelField("Hardpoint " + i);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(item.FindProperty("Name"));
            //EditorGUILayout.PropertyField(item.FindProperty("transform.position"), true);
            var rotation = item.FindProperty("transform.rotation");
            //EditorGUILayout.PropertyField(rotation, true);
            var eulerUpdates = EditorGUILayout.Vector3Field("Euler Rotation", rotation.quaternionValue.eulerAngles);
            EditorGUILayout.PropertyField(item.FindProperty("Turret"));

            if (GUILayout.Button(deleteButtonContent))
            {
                //list.(i);
                GameObject.Destroy(list[i]);
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
        //var list = serializedObject.FindProperty("Hardpoints");

        var list = _ship.GetComponentsInChildren<UpdatedHardpoint>();
        //serializedObject
        Show(list);
        if (GUILayout.Button(newButtonContent))
        {
            //list.InsertArrayElementAtIndex(list.arraySize);
            //var turret = new GameObject("Turret");
            var turret = GameObject.CreatePrimitive(PrimitiveType.Cube);
            turret.AddComponent<UpdatedHardpoint>();
            turret.transform.SetParent(_ship.transform);
            turret.transform.localPosition = Vector3.zero;
            //turret.hideFlags = HideFlags.HideInHierarchy;
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void OnSceneGUI()
    {
        UpdatedShip example = (UpdatedShip)target;


        var hardpoints = example.GetComponentsInChildren<UpdatedHardpoint>();
    
        foreach (var hardpoint in hardpoints)
        {
        //    if (hardpoint.Turret == null)
        //    {
        //        //Gizmos.DrawSphere(_ship.transform.position + hardpoint.Position, 5f);
        //    }
        //    else
        //    {
        //        //var turretRender = hardpoint.Turret.GetComponentsInChildren<SkinnedMeshRenderer>();
        //       //var turretMesh = hardpoint.Turret.GetComponentsInChildren<MeshFilter>();
        //        var turretRenderer = hardpoint.Turret.GetComponentsInChildren<SkinnedMeshRenderer>();

        //        //Graphics.Dr
               

        //        foreach (var turretRender in turretRenderer)
        //        {
        //            var material = turretRender.sharedMaterial;
        //            material.SetPass(0);
        //            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;

        //            //Debug
        //            //Gizmos.DrawMesh(meshFilter.sharedMesh, _ship.transform.position + hardpoint.Position, hardpoint.Rotation * _ship.transform.rotation * Quaternion.Euler(-90, 0, 0), Vector3.one);
        //            //Graphics.DrawMesh(turretRender.sharedMesh, _ship.transform.position + hardpoint.Position, hardpoint.Rotation * _ship.transform.rotation * Quaternion.Euler(-90, 0, 0),
        //            //    material, 0, null, 0, null, false, false, false);
        //            Graphics.DrawMeshNow(turretRender.sharedMesh, _ship.transform.position + hardpoint.Position,
        //                hardpoint.Rotation * _ship.transform.rotation * Quaternion.Euler(-90, 0, 0));
        //        }
        //    }
        }
        

        //Graphics.DrawMesh();
        var mesh = _ship.ShipModel.GetComponentsInChildren<MeshFilter>();
        var renderer = _ship.ShipModel.GetComponentInChildren<MeshRenderer>();

        foreach (var meshFilter in mesh)
        {
            var material = renderer.sharedMaterial;
            material.SetPass(0);
            //material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
            
            Graphics.DrawMeshNow(meshFilter.sharedMesh, _ship.transform.position, _ship.transform.rotation * Quaternion.Euler(-90, 0, 0));
            //Graphics.DrawMesh(meshFilter.sharedMesh, _ship.transform.localToWorldMatrix, material, 0);
            //Graphics.DrawMesh(meshFilter.sharedMesh, _ship.transform.position, _ship.transform.rotation * Quaternion.Euler(-90, 0, 0), 
            //    material, 0, null, 0, null, false, false, false);
        }

        EditorGUI.BeginChangeCheck();


        Vector3[] positions = new Vector3[hardpoints.Count()];
        Quaternion[] rotations = new Quaternion[hardpoints.Count()];

        for (var index = 0; index < hardpoints.Count(); index++)
        {
            var hardpoint = hardpoints[index];

            if (hardpoint.transform.rotation == new Quaternion())
            {
                hardpoint.transform.rotation = Quaternion.identity;
            }

            positions[index] = hardpoint.transform.position;
            rotations[index] = hardpoint.transform.rotation;



            if (Tools.current == Tool.Move)
            {
                positions[index] = Handles.PositionHandle(hardpoint.transform.position, hardpoint.transform.rotation)
                                   - example.transform.position;
            }
            else if (Tools.current == Tool.Rotate)
            {
                rotations[index] = Handles.RotationHandle(hardpoint.transform.rotation,
                    example.transform.position + hardpoint.transform.position);
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            for (var index = 0; index < hardpoints.Count(); index++)
            {
                var hardPoint = hardpoints[index];
                if (hardPoint.transform.position != positions[index] || hardPoint.transform.rotation != rotations[index])
                {
                    Undo.RecordObject(example, "Updated hard point");
                    hardPoint.transform.position = positions[index];
                    hardPoint.transform.rotation = rotations[index];
                }

                //hardpoint.Update();
            }
        }
    }
}
