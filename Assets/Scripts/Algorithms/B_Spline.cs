using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class B_Spline : MonoBehaviour
{
    // STEPS BY STEPS
    // --------------

    // The curve is defined by four points
    //                              P0 and P3
    //                              P1 and P2
    // The B_Spline curve is defined like so : SBS(t) = (1/6) [(1-t)^3 * P[i-3] + (3 * t^3 - 6 * t^2 + 4) * P[i-2] + (-3 * t^3 + 3 * t^2 + 3 * t + 1) * P[i-1] + t^3 * P[i]]
    // with
    // 

    // FUNCTION TO COMPUTE B_Spline POINT => SBS(t) = T . MBS . GBS

    public static Vector3 SBS(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
    {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float omt3 = omt2 * omt;

        float t2 = t * t;
        float t3 = t2 * t;

        Vector3 term0 = omt3 * P0;
        Vector3 term1 = (3f * t3 - 6f * t2 + 4f) * P1;
        Vector3 term2 = (-3f * t3 + 3f * t2 + 3f * t + 1f) * P2;
        Vector3 term3 = t3 * P3;

        Vector3 result = (term0 + term1 + term2 + term3) / 6f;
        return result;
    }

    public static void Testing()
    { }
}
