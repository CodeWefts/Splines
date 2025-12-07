using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static AlgorithmSelection;
using static UnityEngine.GraphicsBuffer;

[ExecuteAlways]
public class DrawInEditor : MonoBehaviour
{
    [Range(2, 100)]
    public int samplesPerSegment = 20;
    

    public void OnDrawGizmos()
    {
        SplineManager splineManager = gameObject.transform.GetComponent<SplineManager>();
        if (splineManager == null || splineManager.splines == null || splineManager.splines.Count == 0)
            return;

        for (int i = 0; i < splineManager.splines.Count; i++)
        {
            var spline = splineManager.splines[i];

            if (spline[0] == null || spline[0].transform == null || spline[0].transform.parent == null)
                continue;

            Gizmos.color = Color.black;

            //Polyline Drawing
            for (int j = 0; j < spline.Count - 1; j++)
            {
                if (spline[j] != null && spline[j + 1] != null)
                {
                    Gizmos.DrawLine(spline[j].transform.position, spline[j + 1].transform.position);
                }
            }

            AlgorithmType selected = spline[0].transform.parent.GetComponent<AlgorithmSelection>().selectedAlgorithm;

            GameObject[] pts = spline.ToArray();
            

            switch (selected)
            {
                case AlgorithmType.Hermite:
                    DrawHermite(pts);
                    break;
                case AlgorithmType.Bezier:
                    DrawBezier(pts);
                    break;
                case AlgorithmType.B_Spline:
                    DrawBSpline(pts);
                    break;
                case AlgorithmType.Catmull_Rom:
                    DrawCatmullRom(pts);
                    break;
                default:
                    break;
            }

        }
    }

    private void DrawHermite(GameObject[] pts)
    {
        if (pts == null || pts.Length < 2)
            return;

        //Hermite Drawing
        for (int j = 0; j < pts.Length - 1; j++)
        {
            if (pts[j] == null || pts[j + 1] == null) continue;
            if (pts[j].transform == null || pts[j + 1].transform == null) continue;

            Gizmos.color = Color.red;

            Vector3 P0 = pts[j].transform.position;
            Vector3 P1 = pts[j + 1].transform.position;

            ControlPoint cp0 = pts[j].GetComponent<ControlPoint>();
            ControlPoint cp1 = pts[j + 1].GetComponent<ControlPoint>();

            if (cp0 == null || cp1 == null) continue;

            Vector3 T0 = Hermite.Tangent(pts, j, cp0.tension);
            Vector3 T1 = Hermite.Tangent(pts, j + 1, cp1.tension);

            Vector3 prev = P0;
            for (int s = 1; s <= samplesPerSegment; s++)
            {
                float t = (float)s / samplesPerSegment;
                Vector3 curr = Hermite.SH(P0, P1, T0, T1, t);
                Gizmos.DrawLine(prev, curr);
                prev = curr;
            }
        }
    }

    private void DrawBezier(GameObject[] pts)
    {
        if (pts.Length < 4)
        { 
            GameObject controlPoint = SplineManager.CreateControlPoint(Vector3.zero);
            controlPoint.transform.parent = pts[0].transform.parent;

            SplineManager splineManager = GameObject.Find("Manager").GetComponent<SplineManager>();
            splineManager.DelayedValidate();
        }

        for (int j = 0; j < pts.Length - 3; j++)
        {
            if (pts[j] == null || pts[j + 1] == null || pts[j + 2] == null || pts[j + 3] == null)
                continue;

            Gizmos.color = Color.magenta;

            Vector3 P0 = pts[j].transform.position;
            Vector3 P1 = pts[j + 1].transform.position;
            Vector3 P2 = pts[j + 2].transform.position;
            Vector3 P3 = pts[j + 3].transform.position;

            Vector3 prev = P0;
            for (int s = 1; s <= samplesPerSegment; s++)
            {
                float t = (float)s / samplesPerSegment;
                Vector3 curr = Bezier.SB(P0, P1, P2, P3, t);
                Gizmos.DrawLine(prev, curr);
                prev = curr;
            }
        }
    }

    private void DrawBSpline(GameObject[] pts)
    {
        if (pts.Length < 4)
        {
            GameObject controlPoint = SplineManager.CreateControlPoint(Vector3.zero);
            controlPoint.transform.parent = pts[0].transform.parent;

            SplineManager splineManager = GameObject.Find("Manager").GetComponent<SplineManager>();
            splineManager.DelayedValidate();
        }

        for (int j = 0; j < pts.Length - 3; j++)
        {
            if (pts[j] == null || pts[j + 1] == null || pts[j + 2] == null || pts[j + 3] == null)
                continue;

            Gizmos.color = Color.green;

            Vector3 P0 = pts[j].transform.position;
            Vector3 P1 = pts[j + 1].transform.position;
            Vector3 P2 = pts[j + 2].transform.position;
            Vector3 P3 = pts[j + 3].transform.position;

            Vector3 prev = P0;
            for (int s = 1; s <= samplesPerSegment; s++)
            {
                float t = (float)s / samplesPerSegment;
                Vector3 curr = B_Spline.SBS(P0, P1, P2, P3, t);
                Gizmos.DrawLine(prev, curr);
                prev = curr;
            }
        }
    }

    private void DrawCatmullRom(GameObject[] pts)
    {
        if (pts == null || pts.Length < 4)
        {
            GameObject controlPoint = SplineManager.CreateControlPoint(Vector3.zero);
            controlPoint.transform.parent = pts[0].transform.parent;

            SplineManager splineManager = GameObject.Find("Manager").GetComponent<SplineManager>();
            splineManager.DelayedValidate();
        }

        for (int j = 0; j < pts.Length - 3; j++)
        {
            if (pts[j] == null || pts[j + 1] == null || pts[j + 2] == null || pts[j + 3] == null)
                continue;

            Gizmos.color = Color.cyan;

            Vector3 P0 = pts[j].transform.position;
            Vector3 P1 = pts[j + 1].transform.position;
            Vector3 P2 = pts[j + 2].transform.position;
            Vector3 P3 = pts[j + 3].transform.position;

            Vector3 prev = Catmull_Rom.SCR(P0, P1, P2, P3, 0f);

            for (int s = 1; s <= samplesPerSegment; s++)
            {
                float t = (float)s / samplesPerSegment;
                Vector3 curr = Catmull_Rom.SCR(P0, P1, P2, P3, t);

                Gizmos.DrawLine(prev, curr);
                prev = curr;
            }
        }
    }
}