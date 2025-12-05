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
        }
    }
}

[CustomEditor(typeof(AlgorithmSelection))]
public class AlgorithmSelectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
        AlgorithmSelection algoSelection = (AlgorithmSelection)target;
        if (GUILayout.Button("Add new control point"))
        {
            GameObject controlPoint = SplineManager.CreateControlPoint(Vector3.zero);
            controlPoint.transform.parent = algoSelection.transform;

            SplineManager splineManager = GameObject.Find("Manager").GetComponent<SplineManager>();
            splineManager.DelayedValidate();
        }
    }
}

