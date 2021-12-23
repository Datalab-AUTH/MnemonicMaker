using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AutoPrefab : MonoBehaviour
{
    
    public GameObject cloneThisGameObject;
    //public GameObject cloneItUnder;
    public int numberOfClones;
    public string path; //World/Dungeon
    public static int horizontal = 4;
    public int offestY;
    public int offsetX;

    public void Generate()
    {
        Debug.Log("### Generating Prefabs ###");

        int vertical = numberOfClones / horizontal;
        string localPath = "Assets/Resources/Prefabs/" + path + "/Chunk";

        for (int i = 0; i < vertical; i++)
        {
            for (int j = 0; j < horizontal; j++)
            {
                cloneThisGameObject.transform.position = new Vector3(offsetX+(49*j), offestY+(27*i), 0);
                Object prefab = PrefabUtility.CreatePrefab(localPath + "" + (i * horizontal + j) + ".prefab", cloneThisGameObject);
            }
        }

        cloneThisGameObject.transform.position = new Vector3(0, 0, 0);
    }
}

[CustomEditor(typeof(AutoPrefab))]
public class AutoPrefabEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            var generator = (AutoPrefab)target;
            generator.Generate();
        }
    }
}
