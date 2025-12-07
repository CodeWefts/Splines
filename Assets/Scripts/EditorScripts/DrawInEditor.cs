using System.Collections.Generic;
using UnityEngine;
using static AlgorithmSelection;

[ExecuteAlways]
public class DrawInEditor : MonoBehaviour
{
    [Range(2, 100)]
    public int samplesPerSegment = 20;

    [Tooltip("Optional explicit reference to the SplineManager. If null, the script will try to find it on this GameObject.")]
    public SplineManager splineManager;

    private void Reset()
    {
        if (splineManager == null)
            splineManager = GetComponent<SplineManager>();
    }

    public void OnDrawGizmos()
    {
        if (splineManager == null)
            splineManager = GetComponent<SplineManager>();

        if (splineManager == null || splineManager.splines == null || splineManager.splines.Count == 0)
            return;

        for (int i = 0; i < splineManager.splines.Count; i++)
        {
            List<GameObject> spline = splineManager.splines[i];
            if (spline == null || spline.Count == 0)
                continue;

            if (spline[0] == null || spline[0].transform == null || spline[0].transform.parent == null)
                continue;

            // Draw control polygon
            Gizmos.color = Color.black;
            for (int j = 0; j < spline.Count - 1; j++)
            {
                if (spline[j] != null && spline[j + 1] != null)
                {
                    Gizmos.DrawLine(spline[j].transform.position, spline[j + 1].transform.position);
                }
            }
            
            AlgorithmType selected =
                spline[0].transform.parent.GetComponent<AlgorithmSelection>().selectedAlgorithm;

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
            }
        }
    }

    private void DrawHermite(GameObject[] pts)
    {
        if (pts == null || pts.Length < 2)
            return;

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
        if (pts == null || pts.Length < 4)
            return;

        Gizmos.color = Color.magenta;

        for (int j = 0; j <= pts.Length - 4; j++)
        {
            if (pts[j] == null || pts[j + 1] == null || pts[j + 2] == null || pts[j + 3] == null)
                continue;

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
        if (pts == null || pts.Length < 4)
            return;

        Gizmos.color = Color.green;

        for (int j = 0; j <= pts.Length - 4; j++)
        {
            if (pts[j] == null || pts[j + 1] == null || pts[j + 2] == null || pts[j + 3] == null)
                continue;

            Vector3 P0 = pts[j].transform.position;
            Vector3 P1 = pts[j + 1].transform.position;
            Vector3 P2 = pts[j + 2].transform.position;
            Vector3 P3 = pts[j + 3].transform.position;

            Vector3 prev = B_Spline.SBS(P0, P1, P2, P3, 0f);

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
            return;

        Gizmos.color = Color.cyan;

        for (int j = 0; j <= pts.Length - 4; j++)
        {
            if (pts[j] == null || pts[j + 1] == null || pts[j + 2] == null || pts[j + 3] == null)
                continue;

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
