using UnityEngine;

public class Catmull_Rom : MonoBehaviour
{
    // STEPS BY STEPS
    // --------------

    // The curve is defined by four points
    //                              P0 and P3
    //                              P1 and P2
    // The Catmull_Rom curve is defined like so : SCR(t) = 0.5 * ( 2 * P1 + (-P0 + P2) * t + (2 * P0 - 5 * P1 + 4 * P2 - P3) * t^2 + (-P0 + 3 * P1 - 3 * P2 + P3) * t^3 )
    // with
    // 

    // FUNCTION TO COMPUTE Catmull_Rom POINT => SCR(t) = T . MSCR . GSCR


    public static Vector3 SCR(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
    {
        if (t < 0f) t = 0f;
        if (t > 1f) t = 1f;

        float t2 = t * t;
        float t3 = t2 * t;

        Vector3 term0 = 2f * P1;
        Vector3 term1 = (-P0 + P2) * t;
        Vector3 term2 = (2f * P0 - 5f * P1 + 4f * P2 - P3) * t2;
        Vector3 term3 = (-P0 + 3f * P1 - 3f * P2 + P3) * t3;

        return 0.5f * (term0 + term1 + term2 + term3);
    }
}
