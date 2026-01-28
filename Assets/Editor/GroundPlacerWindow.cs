using UnityEngine;
using UnityEditor;

public class GroundPlacerWindow : EditorWindow
{
    private GameObject groundPrefab;
    private int count = 10;
    private float spacing = 1f;
    private Vector3 startPosition = Vector3.zero;

    private Axis axis = Axis.Z;

    private enum Axis
    {
        X,
        Y,
        Z
    }

    [MenuItem("Tools/Ground Placer")]
    private static void Open()
    {
        GetWindow<GroundPlacerWindow>("Ground Placer");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Ground Auto Placement", EditorStyles.boldLabel);

        groundPrefab = (GameObject)EditorGUILayout.ObjectField(
            "Ground Prefab",
            groundPrefab,
            typeof(GameObject),
            false
        );

        count = EditorGUILayout.IntField("Count", count);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);
        startPosition = EditorGUILayout.Vector3Field("Start Position", startPosition);
        axis = (Axis)EditorGUILayout.EnumPopup("Axis", axis);

        GUILayout.Space(10);

        GUI.enabled = groundPrefab != null && count > 0;

        if (GUILayout.Button("Place Grounds"))
        {
            Place();
        }

        GUI.enabled = true;
    }

    private void Place()
    {
        Transform parent = new GameObject("Grounds").transform;

        Vector3 dir = GetDirection();

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = startPosition + dir * spacing * i;

            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(groundPrefab);
            obj.transform.position = pos;
            obj.transform.SetParent(parent);
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        Debug.Log($"Placed {count} ground objects.");
#endif
    }

    private Vector3 GetDirection()
    {
        switch (axis)
        {
            case Axis.X: return Vector3.right;
            case Axis.Y: return Vector3.up;
            case Axis.Z: return Vector3.forward;
            default: return Vector3.forward;
        }
    }
}

