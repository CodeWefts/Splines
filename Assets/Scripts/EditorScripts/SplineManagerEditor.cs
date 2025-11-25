using UnityEditor;

using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

[CustomEditor(typeof(SplineManager))]

public class AlgoManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawDefaultInspector();
        SplineManager splineManager = (SplineManager)target;

        if(GUILayout.Button("New Spline"))
        {
            splineManager.AddingNewSpline();
            Debug.Log("Button Pressed in Custom Editor");
        }
    }
}
