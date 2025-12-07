using System.Collections.Generic;
using UnityEngine;
using static AlgorithmSelection;

public class SplineAnimation : MonoBehaviour
{
    [SerializeField] private GameObject[] m_ObjectToAnimate;
    [SerializeField][Range(0f, 1f)] private float pos_t = 0f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool loop = false;

    private GameObject[] instantiatedObjects;
    private List<GameObject> controlPoints = new List<GameObject>();

    private void Awake()
    {
        CacheControlPoints();
        InstantiateObjects();
    }

    private void InstantiateObjects()
    {
        if (m_ObjectToAnimate == null || m_ObjectToAnimate.Length == 0)
            return;

        instantiatedObjects = new GameObject[m_ObjectToAnimate.Length];

        for (int i = 0; i < m_ObjectToAnimate.Length; i++)
        {
            if (m_ObjectToAnimate[i] == null) continue;

            Vector3 startPos = GetStartPosition();
            GameObject newObj = Instantiate(m_ObjectToAnimate[i], startPos, Quaternion.identity);
            newObj.name = $"{m_ObjectToAnimate[i].name}_Follower_{i}";

            instantiatedObjects[i] = newObj;
        }
    }

    private void CacheControlPoints()
    {
        controlPoints.Clear();
        foreach (Transform child in transform)
        {
            if (child == null) continue;
            if (child.GetComponent<ControlPoint>() != null)
                controlPoints.Add(child.gameObject);
        }
    }

    private Vector3 GetStartPosition()
    {
        if (controlPoints.Count > 0 && controlPoints[0] != null)
            return controlPoints[0].transform.position;
        return transform.position;
    }

    private void Update()
    {
        if (instantiatedObjects == null || instantiatedObjects.Length == 0 ||
        controlPoints.Count < 2 || !HasValidAlgorithm())
            return;

        if(pos_t < 1f)
            pos_t += Time.deltaTime * speed * 0.1f;

        bool shouldLoop = loop;
        if (shouldLoop && pos_t >= 1f)
            pos_t = 0f;

        AlgorithmType type = GetComponent<AlgorithmSelection>().selectedAlgorithm;

        for (int i = 0; i < instantiatedObjects.Length; i++)
        {
            if (instantiatedObjects[i] == null) continue;
            instantiatedObjects[i].transform.position = EvaluateSpline(type, pos_t);

            float offsetT = pos_t + (i * 0.1f);
            offsetT = Mathf.Clamp01(offsetT);
            Vector3 position = EvaluateSpline(type, offsetT);
            instantiatedObjects[i].transform.position = position;
        }
    }

    private bool HasValidAlgorithm()
    {
        AlgorithmSelection algo = GetComponent<AlgorithmSelection>();
        return algo != null;
    }

    private Vector3 EvaluateSpline(AlgorithmType type, float t)
    {
        t = Mathf.Clamp01(t);
        int n = controlPoints.Count;

        if (n < 2) return transform.position;

        int maxSegments = type switch
        {
            AlgorithmType.Hermite => n - 1,
            AlgorithmType.Bezier => Mathf.Max(1, (n - 1) / 3),
            AlgorithmType.B_Spline => Mathf.Max(1, n - 3),
            AlgorithmType.Catmull_Rom => Mathf.Max(1, n - 3), 
            _ => 1
        };

        int segmentIdx = Mathf.FloorToInt(t * maxSegments);
        float localT = t * maxSegments - segmentIdx;

        return type switch
        {
            AlgorithmType.Hermite => EvaluateHermite(segmentIdx, localT),
            AlgorithmType.Bezier => EvaluateBezier(segmentIdx, localT),
            AlgorithmType.B_Spline => EvaluateBSpline(segmentIdx, localT),
            AlgorithmType.Catmull_Rom => EvaluateCatmullRom(segmentIdx, localT),
            _ => controlPoints[0].transform.position
        };
    }

    private Vector3 EvaluateHermite(int segmentIdx, float localT)
    {
        if (segmentIdx < 0 || segmentIdx >= controlPoints.Count - 1)
            return controlPoints[0].transform.position;

        var pts = controlPoints.ToArray();
        Vector3 P0 = controlPoints[segmentIdx].transform.position;
        Vector3 P1 = controlPoints[segmentIdx + 1].transform.position;

        ControlPoint cp0 = controlPoints[segmentIdx].GetComponent<ControlPoint>();
        ControlPoint cp1 = controlPoints[segmentIdx + 1].GetComponent<ControlPoint>();
        if (cp0 == null || cp1 == null) return P0;

        Vector3 T0 = Hermite.Tangent(pts, segmentIdx, cp0.tension);
        Vector3 T1 = Hermite.Tangent(pts, segmentIdx + 1, cp1.tension);

        return Hermite.SH(P0, P1, T0, T1, localT);
    }

    private Vector3 EvaluateBezier(int segmentIdx, float localT)
    {
        if (controlPoints.Count < 4) return controlPoints[0].transform.position;

        int startIdx = segmentIdx * 3;
        if (startIdx + 3 >= controlPoints.Count)
            startIdx = controlPoints.Count - 4;

        Vector3 P0 = controlPoints[startIdx].transform.position;
        Vector3 P1 = controlPoints[startIdx + 1].transform.position;
        Vector3 P2 = controlPoints[startIdx + 2].transform.position;
        Vector3 P3 = controlPoints[startIdx + 3].transform.position;

        return Bezier.SB(P0, P1, P2, P3, localT);
    }

    private Vector3 EvaluateBSpline(int segmentIdx, float localT)
    {
        if (controlPoints.Count < 4) return controlPoints[0].transform.position;

        int startIdx = Mathf.Max(0, segmentIdx - 2);
        if (startIdx + 3 >= controlPoints.Count)
            startIdx = controlPoints.Count - 4;

        segmentIdx = Mathf.Clamp(segmentIdx, 0, controlPoints.Count - 4);

        Vector3 P0 = controlPoints[startIdx].transform.position;
        Vector3 P1 = controlPoints[startIdx + 1].transform.position;
        Vector3 P2 = controlPoints[startIdx + 2].transform.position;
        Vector3 P3 = controlPoints[startIdx + 3].transform.position;

        return B_Spline.SBS(P0, P1, P2, P3, localT);
    }

    private Vector3 EvaluateCatmullRom(int segmentIdx, float localT)
    {
        if (controlPoints.Count < 4) return controlPoints[0].transform.position;

        int startIdx = Mathf.Max(0, segmentIdx - 2);
        if (startIdx + 3 >= controlPoints.Count)
            startIdx = controlPoints.Count - 4;

        Vector3 P0 = controlPoints[startIdx].transform.position;
        Vector3 P1 = controlPoints[startIdx + 1].transform.position;
        Vector3 P2 = controlPoints[startIdx + 2].transform.position;
        Vector3 P3 = controlPoints[startIdx + 3].transform.position;

        return Catmull_Rom.SCR(P0, P1, P2, P3, localT);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (instantiatedObjects != null)
        {
            for (int i = 0; i < instantiatedObjects.Length; i++)
            {
                if (instantiatedObjects[i] != null)
                    instantiatedObjects[i].transform.SetParent(transform, true);
            }
        }
    }
#endif
}