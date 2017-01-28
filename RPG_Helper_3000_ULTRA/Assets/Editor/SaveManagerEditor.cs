using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveManager myScript = (SaveManager)target;
        if (GUILayout.Button("Save"))
        {
            myScript.Save();
        }

        if (GUILayout.Button("Load"))
        {
            myScript.Load();
        }
    }

}
