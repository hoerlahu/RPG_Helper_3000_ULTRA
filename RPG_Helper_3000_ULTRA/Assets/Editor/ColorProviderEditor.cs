using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorProvider))]
public class ColorProviderEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        //SaveManager myScript = (SaveManager)target;
        
    }

}
