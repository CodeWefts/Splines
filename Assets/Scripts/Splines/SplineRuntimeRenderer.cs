using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SplineRuntimeRenderer : MonoBehaviour
{
    public enum AlgorithmType
    {
        Hermite,
        Bezier,
        B_Spline,
        Catmull_Rom
    }

    [Header("Curve type")]
    public AlgorithmType algorithm = AlgorithmType.Hermite;

    [Header("Rendering")]
    [Range(2, 200)] public int samplesPerSegment = 50;
    public Color lineColor = Color.green;
    public float lineWidth = 0.03f;

    private List<GameObject> controlPoints = new List<GameObject>();
    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.alignment = LineAlignment.View;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.startColor = lineColor;
        line.endColor = lineColor;

        CacheControlPoints();
        UpdateLine();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;

        CacheControlPoints();
        UpdateLine();

        if (line != null)
        {
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.startColor = lineColor;
            line.endColor = lineColor;
        }
    }

    private void CacheControlPoints()
    {
        controlPoints.Clear();
        foreach (Transform child in transform)
        {
            if (child != null && child.GetComponent<ControlPoint>() != null)
                controlPoints.Add(child.gameObject);
        }
    }

    public void UpdateLine()
    {
        if (line == null || controlPoints.Count < 2)
            return;

        int n = controlPoints.Count;
        int maxSegments = algorithm switch
        {
            AlgorithmType.Hermite => n - 1,
            AlgorithmType.Bezier => Mathf.Max(1, (n - 1) / 3),
            AlgorithmType.B_Spline => Mathf.Max(1, n - 3),
            AlgorithmType.Catmull_Rom => Mathf.Max(1, n - 3),
            _ => 1
        };

        int totalSamples = Mathf.Max(1, maxSegments * samplesPerSegment);
        List<Vector3> pts = new List<Vector3>(totalSamples + 1);

        for (int i = 0; i <= totalSamples; i++)
        {
            float t = (float)i / totalSamples;
            pts.Add(EvaluateSpline(algorithm, t));
        }

        line.positionCount = pts.Count;
        line.SetPositions(pts.ToArray());
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
            AlgorithmType.Hermite => EvalHermite(segmentIdx, localT),
            AlgorithmType.Bezier => EvalBezier(segmentIdx, localT),
            AlgorithmType.B_Spline => EvalBSpline(segmentIdx, localT),
            AlgorithmType.Catmull_Rom => EvalCatmull(segmentIdx, localT),
            _ => controlPoints[0].transform.position
        };
    }

    // Hermite (utilise ta classe Hermite existante)
    private Vector3 EvalHermite(int segmentIdx, float localT)
    {
        if (segmentIdx < 0 || segmentIdx >= controlPoints.Count - 1)
            return controlPoints[0].transform.position;

        GameObject[] pts = controlPoints.ToArray();

        Vector3 P0 = controlPoints[segmentIdx].transform.position;
        Vector3 P1 = controlPoints[segmentIdx + 1].transform.position;

        ControlPoint cp0 = controlPoints[segmentIdx].GetComponent<ControlPoint>();
        ControlPoint cp1 = controlPoints[segmentIdx + 1].GetComponent<ControlPoint>();
        if (cp0 == null || cp1 == null) return P0;

        Vector3 T0 = Hermite.Tangent(pts, segmentIdx, cp0.tension);
        Vector3 T1 = Hermite.Tangent(pts, segmentIdx + 1, cp1.tension);

        return Hermite.SH(P0, P1, T0, T1, localT);
    }

    // Bezier
    private Vector3 EvalBezier(int segmentIdx, float localT)
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

    // B-Spline
    private Vector3 EvalBSpline(int segmentIdx, float localT)
    {
        if (controlPoints.Count < 4) return controlPoints[0].transform.position;

        int maxSegments = controlPoints.Count - 3;
        segmentIdx = Mathf.Clamp(segmentIdx, 0, maxSegments - 1);

        int startIdx = segmentIdx;

        Vector3 P0 = controlPoints[startIdx].transform.position;
        Vector3 P1 = controlPoints[startIdx + 1].transform.position;
        Vector3 P2 = controlPoints[startIdx + 2].transform.position;
        Vector3 P3 = controlPoints[startIdx + 3].transform.position;

        return B_Spline.SBS(P0, P1, P2, P3, localT);
    }

    // Catmull-Rom
    private Vector3 EvalCatmull(int segmentIdx, float localT)
    {
        if (controlPoints.Count < 4) return controlPoints[0].transform.position;

        int maxSegments = controlPoints.Count - 3;
        segmentIdx = Mathf.Clamp(segmentIdx, 0, maxSegments - 1);

        int startIdx = segmentIdx;

        Vector3 P0 = controlPoints[startIdx].transform.position;
        Vector3 P1 = controlPoints[startIdx + 1].transform.position;
        Vector3 P2 = controlPoints[startIdx + 2].transform.position;
        Vector3 P3 = controlPoints[startIdx + 3].transform.position;

        return Catmull_Rom.SCR(P0, P1, P2, P3, localT);
    }
}
