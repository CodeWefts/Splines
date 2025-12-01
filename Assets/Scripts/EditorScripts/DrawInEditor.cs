using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[ExecuteAlways]
public class DrawInEditor : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        SplineManager splineManager = gameObject.transform.GetComponent<SplineManager>();
        if(splineManager.splines == null || splineManager.splines.Count == 0)
        {
            Debug.Log("RHAAAAAAAAAAAAAA");
            return;
        }

        if (splineManager.splines != null && splineManager.splines.Count > 0)
        {
            for (int i =0; i < splineManager.splines.Count; i++)
            {
                var spline = splineManager.splines[i];
                for (int j = 0; j < spline.Count - 1; j++)
                {
                    if (spline[j] != null && spline[j + 1] != null)
                    {
                        Gizmos.color = Color.black;
                        Gizmos.DrawLine(spline[j].transform.position, spline[j + 1].transform.position);
                    }
                }
            }
        }
    }
}
