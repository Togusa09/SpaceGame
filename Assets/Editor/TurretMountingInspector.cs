using System.Linq;
using Scripts.Ship;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TurretMounting))]
public class TurretMountingInspector : Editor
{
    private TurretMounting _turretMounting;

    private static GUIContent
        moveButtonContent = new GUIContent("\u21b4", "move down"),
        newButtonContent = new GUIContent("+", "duplicate"),
        deleteButtonContent = new GUIContent("-", "delete");

    void OnEnable()
    {
        _turretMounting = (TurretMounting)target;

        var hardpoints = _turretMounting.GetComponentsInChildren<UpdatedHardpoint>();
        foreach (var hardpoint in hardpoints)
        {
            UpdateTurretModel(hardpoint);
        }
    }

    void UpdateTurretModel(UpdatedHardpoint hardpoint)
    {
        if (hardpoint.Turret && hardpoint.Turret.TurretModel)
        {
            hardpoint.gameObject.hideFlags = HideFlags.None;

            var turretModelTransform = hardpoint.transform.Find("TurretModel");
            var turretModel = turretModelTransform ? turretModelTransform.gameObject : Instantiate(hardpoint.Turret.TurretModel, hardpoint.transform);

            turretModel.name = "TurrentModel";
            turretModel.hideFlags = HideFlags.NotEditable | HideFlags.DontSave; ;
            foreach (Transform model in turretModel.transform)
            {
                model.gameObject.hideFlags = HideFlags.NotEditable;
            }
        }
        else
        {
            var shipModelTransform = hardpoint.transform.Find("TurretModel");
            if (shipModelTransform)
            {
                DestroyImmediate(shipModelTransform.gameObject);
            }
        }
    }

    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();

    //    var list = _turretMounting.GetComponentsInChildren<UpdatedHardpoint>();
    //    ShowTurret(list);
    //    if (GUILayout.Button(newButtonContent))
    //    {
    //        var turret = new GameObject("Hardpoint");
    //        turret.AddComponent<UpdatedHardpoint>();
    //        turret.transform.SetParent(_turretMounting.transform);
    //        turret.transform.localPosition = Vector3.zero;
    //        turret.hideFlags = HideFlags.HideAndDontSave;
    //    }

    //    serializedObject.ApplyModifiedProperties();
    //}

    public static void ShowTurret(UpdatedHardpoint[] list)
    {
        for (var i = 0; i < list.Count(); i++)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Hardpoint " + i);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.BeginHorizontal();
            list[i].transform.name = EditorGUILayout.TextField(list[i].transform.name);
            if (GUILayout.Button(deleteButtonContent, GUILayout.Width(30)))
            {
                GameObject.DestroyImmediate(list[i]);
            }
            EditorGUILayout.EndHorizontal();

            var eulerUpdates = EditorGUILayout.Vector3Field("Euler Rotation", list[i].transform.eulerAngles);
            list[i].transform.rotation = Quaternion.Euler(eulerUpdates);

            list[i].transform.localPosition = EditorGUILayout.Vector3Field("Position", list[i].transform.localPosition);
            list[i].Turret = (UpdatedTurret)EditorGUILayout.ObjectField("Turret Prefab", list[i].Turret, typeof(UpdatedTurret), false);
            list[i].hideFlags = HideFlags.None;

            EditorGUI.indentLevel -= 1;
        }
    }


    //protected virtual void OnSceneGUI()
    //{
    //    TurretMounting example = (TurretMounting)target;

    //    var hardpoints = example.GetComponentsInChildren<UpdatedHardpoint>();

    //    EditorGUI.BeginChangeCheck();

    //    Vector3[] positions = new Vector3[hardpoints.Count()];
    //    Quaternion[] rotations = new Quaternion[hardpoints.Count()];


    //    GUIStyle style = new GUIStyle();
    //    style.normal.textColor = Color.green;

    //    for (var index = 0; index < hardpoints.Count(); index++)
    //    {
    //        var hardpoint = hardpoints[index];

    //        if (hardpoint.transform.rotation == new Quaternion())
    //        {
    //            hardpoint.transform.rotation = Quaternion.identity;
    //        }

    //        positions[index] = hardpoint.transform.position;
    //        rotations[index] = hardpoint.transform.rotation;

    //        if (Tools.current == Tool.Move)
    //        {
    //            positions[index] = Handles.PositionHandle(hardpoint.transform.position, hardpoint.transform.rotation);
    //        }
    //        else if (Tools.current == Tool.Rotate)
    //        {
    //            rotations[index] = Handles.RotationHandle(hardpoint.transform.rotation, hardpoint.transform.position);
    //        }

    //        Vector3 position = hardpoint.transform.position + Vector3.up * 2f;
    //        string posString = position.ToString();

    //        Handles.Label(position,
    //            posString + "\nName: " +
    //            hardpoint.Name,
    //            style
    //        );
    //    }

    //    if (EditorGUI.EndChangeCheck())
    //    {
    //        for (var index = 0; index < hardpoints.Count(); index++)
    //        {
    //            var hardPoint = hardpoints[index];
    //            if (hardPoint.transform.position != positions[index] || hardPoint.transform.rotation != rotations[index])
    //            {
    //                Undo.RecordObject(example, "Updated hard point");
    //                hardPoint.transform.position = positions[index];
    //                hardPoint.transform.rotation = rotations[index];
    //            }
    //        }
    //    }

    //    EditorUtility.SetDirty(target);
    //}
}
