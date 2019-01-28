using System.Collections.Generic;
using System.Linq;
using Scripts.Ship;
using UnityEditor;
using UnityEngine;
using SerializedObject = UnityEditor.SerializedObject;

[CustomEditor(typeof(TurretMounting))]
public class TurretMountingInspector : Editor
{
    private class SerializedTurretObjects
    {
        public SerializedObject Hardpoint;
        public SerializedObject Transform;
        public SerializedObject GameObject;
    }


    private TurretMounting _turretMounting;

    private static GUIContent
        moveButtonContent = new GUIContent("\u21b4", "move down"),
        newButtonContent = new GUIContent("+", "duplicate"),
        deleteButtonContent = new GUIContent("-", "delete");

    void OnEnable()
    {
        _turretMounting = (TurretMounting)target;
        _childObjects = new List<SerializedTurretObjects>();

        var hardpoints = _turretMounting.GetComponentsInChildren<UpdatedHardpoint>();
        foreach (var hardpoint in hardpoints)
        {
            UpdateTurretModel(hardpoint);
            //_childObjects.Add(new SerializedObject(hardpoint.gameObject));
            hardpoint.hideFlags = HideFlags.HideInHierarchy;
            _childObjects.Add(new SerializedTurretObjects
            {
                Hardpoint = new SerializedObject(hardpoint),
                GameObject = new SerializedObject(hardpoint.gameObject),
                Transform = new SerializedObject(hardpoint.transform)
            });
        }


    }

    void UpdateTurretModel(UpdatedHardpoint hardpoint)
    {
        if (hardpoint.TurretPrefab && hardpoint.TurretPrefab.TurretModel)
        {
            hardpoint.gameObject.hideFlags = HideFlags.None;

            var turretModelTransform = hardpoint.transform.Find("TurretModel");
            var turretModel = turretModelTransform
                ? turretModelTransform.gameObject
                : Instantiate(hardpoint.TurretPrefab.TurretModel, hardpoint.transform);

            turretModel.name = "TurrentModel";
            turretModel.hideFlags = HideFlags.None;
            ;
            foreach (Transform model in turretModel.transform)
            {
                model.gameObject.hideFlags = HideFlags.None;
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

    private List<SerializedTurretObjects> _childObjects;

    public override void OnInspectorGUI()
    {
        int? indexToRemove = null;
        for (var i = 0; i < _childObjects.Count(); i++)
        {
            var hardpoint = _childObjects[i].Hardpoint;
            var gameObject = _childObjects[i].GameObject;
            var transform = _childObjects[i].Transform;

            EditorGUILayout.LabelField("Hardpoint " + i);
            EditorGUI.indentLevel += 1;

            EditorGUI.BeginChangeCheck();

            var gameObjectName = gameObject.FindProperty("m_Name");
            //var transformName = transform.FindProperty("Name");
            EditorGUILayout.PropertyField(gameObjectName);

            //var name = EditorGUILayout.TextField("Name", gameObject.targetObject.name);
            //if (EditorGUI.EndChangeCheck())
            //{
            //    gameObject.targetObject.name = name;
            //}

            var positionProperty = transform.FindProperty("m_LocalPosition");
            EditorGUILayout.PropertyField(positionProperty);
            //var test = hardpoint.FindProperty("TurretPrefab");
            

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(hardpoint.FindProperty("TurretPrefab"));
            if (EditorGUI.EndChangeCheck())
            {
                var t = 3;

                
            }

            // var turretProperty = hardpoint.FindProperty("Turret");
            //EditorGUILayout.PropertyField(turretProperty, true);
            
            //list[i].transform.localPosition = EditorGUILayout.Vector3Field("Position", list[i].transform.localPosition);
            //list[i].Turret = (UpdatedTurret)EditorGUILayout.ObjectField("Turret Prefab", list[i].Turret, typeof(UpdatedTurret), false);

            //var test = new List<string>();
            //var prop = hardpoint.GetIterator();
            //do
            //{
            //    test.Add(prop.name);

            //} while (prop.Next(true));


            if (GUILayout.Button(deleteButtonContent))
            {
                indexToRemove = i;
            }

            EditorGUI.indentLevel -= 1;

            transform.ApplyModifiedProperties();
            hardpoint.ApplyModifiedProperties();
        }

        if (indexToRemove.HasValue)
        {
            var obj = _childObjects[indexToRemove.Value];
            _childObjects.RemoveAt(indexToRemove.Value);
            DestroyImmediate(obj.GameObject.targetObject);
            ;
        }

        if (GUILayout.Button(newButtonContent))
        {
            var gameObject = new GameObject("Hardpoint");
            var hardpoint = gameObject.AddComponent<UpdatedHardpoint>();
            gameObject.transform.SetParent(_turretMounting.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.hideFlags = HideFlags.HideInHierarchy;

            _childObjects.Add(new SerializedTurretObjects
            {
                Hardpoint = new SerializedObject(hardpoint),
                GameObject = new SerializedObject(gameObject),
                Transform = new SerializedObject(hardpoint.transform)
            });
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
            list[i].TurretPrefab = (UpdatedTurret)EditorGUILayout.ObjectField("Turret Prefab", list[i].TurretPrefab, typeof(UpdatedTurret),
                    false);
            list[i].hideFlags = HideFlags.None;

            EditorGUI.indentLevel -= 1;
        }
    }


    protected virtual void OnSceneGUI()
    {
        for (var i = 0; i < _childObjects.Count(); i++)
        {
            var hardpoint = _childObjects[i].Hardpoint;
            var gameObject = _childObjects[i].GameObject;
            var transform = _childObjects[i].Transform;

            var obj = transform.targetObject as Transform;

            //var positionProperty = transform.FindProperty("m_LocalPosition");
            //var rotationProperty = transform.FindProperty("m_LocalRotation");

            if (Tools.current == Tool.Move)
            {
                EditorGUI.BeginChangeCheck();
                //positions[index] = Handles.PositionHandle(hardpoint.transform.position, hardpoint.transform.rotation);
                var position = Handles.PositionHandle(
                    obj.position, obj.rotation);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(obj, "Change Look At Target Position");
                    obj.position = position;

                }
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
}
