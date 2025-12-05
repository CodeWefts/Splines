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
        if (splineManager.splines == null || splineManager.splines.Count == 0)
        {
            return;
        }

        for (int i = 0; i < splineManager.splines.Count; i++)
        {
            var spline = splineManager.splines[i];

            if (spline == null || spline.Count < 2)
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

            AlgorithmType selected = spline[i].gameObject.GetComponentInParent<AlgorithmSelection>().selectedAlgorithm;

            GameObject[] pts = spline.ToArray();
            Gizmos.color = Color.red;

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
        //Hermite Drawing
        for (int j = 0; j < pts.Length - 1; j++)
        {
            if (pts[j] == null || pts[j + 1] == null)
                continue;

            Vector3 P0 = pts[j].transform.position;
            Vector3 P1 = pts[j + 1].transform.position;

            Vector3 T0 = Hermite.Tangent(pts, j);
            Vector3 T1 = Hermite.Tangent(pts, j + 1);

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
        for (int j = 0; j < pts.Length - 1; j++)
        {
            if (pts[j] == null || pts[j + 1] == null)
                continue;
        }
    }

    private void DrawBSpline(GameObject[] pts)
    {
        for (int j = 0; j < pts.Length - 1; j++)
        {
            if (pts[j] == null || pts[j + 1] == null)
                continue;
        }
    }

    private void DrawCatmullRom(GameObject[] pts)
    {
        for (int j = 0; j < pts.Length - 1; j++)
        {
            if (pts[j] == null || pts[j + 1] == null)
                continue;
        }
    }
}