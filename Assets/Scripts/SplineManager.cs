using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;



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

    public string FindIndex(int idx = -1)
    {
        if(idx != -1)
        {
            return idx.ToString();
        }
        else
        {
            return splines.Count.ToString();
        }
    }

    public void AddControlPoint(int idx = -1)
    {
        GameObject spline = new GameObject("Spline_" + FindIndex(idx));
        spline.AddComponent<AlgorithmSelection>();

        GameObject controlPoint0 = CreateControlPoint(Vector3.zero);
        controlPoint0.transform.parent = spline.transform;

        GameObject controlPoint1 = CreateControlPoint(Vector3.one);
        controlPoint1.transform.parent = spline.transform;


        List<GameObject> newSpline = new List<GameObject>();
        newSpline.Add(controlPoint0);
        newSpline.Add(controlPoint1);

        if (idx != -1)
        {
            splines[idx] = newSpline;
        }
        else
        {
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

    // REFRESH SPLINES IN EDITOR 
    // -------------------------

    // Reminder : Execute when the object is enabled (After the Awake)
    private void OnEnable()
    {
        StartCoroutine(DelayedRefreshSplines());
    }
    
#if UNITY_EDITOR
    // Reminder : Execute when the scene is loading
    private void OnValidate()
    {
        EditorApplication.delayCall -= DelayedValidate;
        EditorApplication.delayCall += DelayedValidate;
    }

    private void DelayedValidate()
    {
        RefreshSplines();
    }
#endif

    private IEnumerator DelayedRefreshSplines()
    {
        yield return new WaitForSeconds(0.1f);
        RefreshSplines();
    }

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

        Scene scene = GetValidScene();

        if (!scene.IsValid())
        {
            Debug.LogError("No valid scene found for SplineManager.");
        }

        var roots = scene.GetRootGameObjects();

        foreach (var root in roots)
        {
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

    private Scene GetValidScene()
    {
        Scene scene = gameObject.scene;
        if (scene.IsValid() && scene.isLoaded)
            return scene;

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            scene = SceneManager.GetActiveScene();
            if (scene.IsValid() && scene.isLoaded)
                return scene;
        }
#endif

        // 3. Last resort
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            scene = SceneManager.GetSceneAt(i);
            if (scene.IsValid() && scene.isLoaded)
                return scene;
        }

        return default;
    }


#if UNITY_EDITOR
    [ContextMenu("Refresh Splines Now")]
    private void ContextRefresh()
    {
        RefreshSplines();
    }
#endif
}