using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public class SplineManager : MonoBehaviour
{

    public List<List<GameObject>> splines;

    public void AddingNewSpline()
    {
        if (splines == null)
        {
            splines = new List<List<GameObject>>();
        }

        int tmpIdx = GetEmptySplineIdx();

        if (tmpIdx > -1)
        {
            Debug.Log("Spline is null.");
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
            if (splines[i][0] != null)
            {
                Debug.Log("Spline " + i.ToString() + " has " + splines[i].Count.ToString() + " control points.");
            }
            else
            {
                return i;
                Debug.Log("Spline is null.");
                break;
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
}
