using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteAlways]
public class SplineManager : MonoBehaviour
{
    [Header("List of Splines")]
    [SerializeField] public List<List<GameObject>> splines = new List<List<GameObject>>();

    public void AddingNewSpline()
    {

        int tmpIdx = GetEmptySplineIdx();

        if (tmpIdx > -1)
        {
            Debug.Log("Spline is null..");
            AddControlPoint(tmpIdx);
        }
        else
        {
            AddControlPoint();
        }
    }

    private int GetEmptySplineIdx()
    {
        for (int i = 0; i < splines.Count; i++)
        {
            if (splines[i][0] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public void AddControlPoint(int idx = -1)
    {
        if(idx != -1)
        {
            GameObject spline = new GameObject("Spline_" + idx.ToString());

            GameObject controlPoint0 = CreateControlPoint(Vector3.zero);
            controlPoint0.transform.parent = spline.transform;
            GameObject controlPoint1 = CreateControlPoint(Vector3.one);
            controlPoint1.transform.parent = spline.transform;

            List<GameObject> newSpline = new List<GameObject>();
            newSpline.Add(controlPoint0);
            newSpline.Add(controlPoint1);

            splines[idx] = newSpline;
        }
        else
        {
            GameObject spline = new GameObject("Spline_" + splines.Count.ToString());

            GameObject controlPoint0 = CreateControlPoint(Vector3.zero);
            controlPoint0.transform.parent = spline.transform;
            GameObject controlPoint1 = CreateControlPoint(Vector3.one);
            controlPoint1.transform.parent = spline.transform;

            List<GameObject> newSpline = new List<GameObject>();
            newSpline.Add(controlPoint0);
            newSpline.Add(controlPoint1);

            splines.Add(newSpline);
        }
    }

    public GameObject CreateControlPoint(Vector3 position)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        cube.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        var renderer = cube.gameObject.GetComponent<Renderer>();
        cube.AddComponent<ControlPoint>();

        var tmpMaterial = new Material(renderer.sharedMaterial);
        tmpMaterial.color = Color.black;
        renderer.sharedMaterial = tmpMaterial;
        return cube;
    }

    private void OnEnable()
    {
        RefreshSplines();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        RefreshSplines();
    }
#endif

    private void RefreshSplines()
    {
        FillSplinesFromScene();

#if UNITY_EDITOR
        if (!Application.isPlaying)
            EditorUtility.SetDirty(this);
#endif
    }

    public void FillSplinesFromScene()
    {
        splines.Clear();

        // Parcourt tous les GameObjects racine de la scène active
        Scene scene = gameObject.scene;
        var roots = scene.GetRootGameObjects();

        foreach (var root in roots)
        {
            // On parcourt toute la hiérarchie du root
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            {
                if (!t.name.StartsWith("Spline_"))
                    continue;

                var controlPoints = new List<GameObject>();
                foreach (Transform cp in t)
                    controlPoints.Add(cp.gameObject);

                if (controlPoints.Count > 0)
                    splines.Add(controlPoints);
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Refresh Splines Now")]
    private void ContextRefresh()
    {
        RefreshSplines();
    }
#endif
}