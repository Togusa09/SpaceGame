using Scripts.Ship;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ShipAppearance))]
public class ShipAppearanceInspector : Editor
{
    private ShipAppearance _shipAppearance;

    void OnEnable()
    {
        _shipAppearance = (ShipAppearance)target;

        UpdateShipModel();
    }

    void UpdateShipModel()
    {
        if (_shipAppearance.ShipModel)
        {
            var shipModelTransform = _shipAppearance.gameObject.transform.Find("ShipModel");

            var shipModel = shipModelTransform ? shipModelTransform.gameObject : Instantiate(_shipAppearance.ShipModel, _shipAppearance.transform);

            shipModel.name = "ShipModel";
            shipModel.hideFlags = HideFlags.NotEditable | HideFlags.DontSave;
            shipModel.transform.localEulerAngles = new Vector3(0, 90, 0);
            foreach (Transform model in shipModel.transform)
            {
                model.gameObject.hideFlags = HideFlags.NotEditable;
            }

            var meshFilters = shipModel.GetComponentsInChildren<MeshFilter>();
            var bounds = new Bounds();

            foreach (var mesh in meshFilters)
            {
                bounds.Encapsulate(mesh.sharedMesh.bounds);
            }

            var boxCollider = _shipAppearance.GetComponent<BoxCollider>();
            // Unity models are rotated 90 degress...
            boxCollider.size = new Vector3(bounds.size.y, bounds.size.z, bounds.size.x);
            boxCollider.center = new Vector3(bounds.center.y, bounds.center.z, -bounds.center.x);
        }
        else if (!_shipAppearance.ShipModel)
        {
            var shipModelTransform = _shipAppearance.gameObject.transform.Find("ShipModel");
            if (shipModelTransform)
            {
                DestroyImmediate(shipModelTransform.gameObject);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ShipModel"));
        if (EditorGUI.EndChangeCheck())
        {
            UpdateShipModel();
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("MoveSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TurnSpeed"));

        serializedObject.ApplyModifiedProperties();
    }



    protected virtual void OnSceneGUI()
    {

    }
}
