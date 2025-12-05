//using System.Numerics;
using UnityEngine;

[ExecuteInEditMode]
public class Hermite
{
    // CONTROL POINTS AND TANGENTS (random example values) useful for testing
    //[SerializeField] Vector3 T0 = new Vector3(1, 0, 0); // Tangent at Point 0
    //[SerializeField] Vector3 T1 = new Vector3(1, 0, 0); // Tangent at Point 1
    
    public void HermiteCalc(GameObject spline)
    {
        if (spline == null && spline.gameObject.transform.childCount == 0) return;

        int count = spline.gameObject.transform.childCount;
        Debug.Log(count);

        for (int i = 1; i < count; i++)
        {
            Vector3 P0 = spline.gameObject.transform.GetChild(i - 1).position;
            Vector3 P1 = spline.gameObject.transform.GetChild(i).position;

            //Vector3 result = SH(P0, P1, 1);
            //if(i % 2 != 0)
                //Debug.Log("result : " + result);
        }
    }

    public void Testing()
    {
        Debug.Log("Hermite Testing Started");

        // First scenario (basic)
        Debug.Log("BASIC --------------------------------------------------------------------------------");

        Vector3 P0 = new Vector3(0f, 0f, 0f);
        Vector3 P1 = new Vector3(1f, 0f, 0f);

        Vector3 T0 = new Vector3(1f, 0f, 0f);
        Vector3 T1 = new Vector3(1f, 0f, 0f);

        Debug.Log($"t = 0; Result : {SH(P0, P1, T0, T1, 0f)} , Expected: (0.00, 0.00, 0.00) ");
        Debug.Log($"t = 0.25; Result : {SH(P0, P1, T0, T1, 0.25f)} , Expected: (0.25, 0.00, 0.00) ");
        Debug.Log($"t = 0.5; Result : {SH(P0, P1, T0, T1, 0.5f)} , Expected: (0.50, 0.00, 0.00) ");
        Debug.Log($"t = 0.75; Result : {SH(P0, P1, T0, T1, 0.75f)} , Expected: (0.75, 0.00, 0.00) ");
        Debug.Log($"t = 1; Result : {SH(P0, P1, T0, T1, 1f)} , Expected: (1.00, 0.00, 0.00) ");

        // Second scenario (NULL)
        Debug.Log("NULL --------------------------------------------------------------------------------");

        T0 = new Vector3(0f, 0f, 0f);
        T1 = new Vector3(0f, 0f, 0f);

        Debug.Log($"t = 0; Result : {SH(P0, P1, T0, T1, 0f)} , Expected: (0.00, 0.00, 0.00) ");
        Debug.Log($"t = 0.25; Result : {SH(P0, P1, T0, T1, 0.25f)} , Expected: (0.16, 0.00, 0.00) ");
        Debug.Log($"t = 0.5; Result : {SH(P0, P1, T0, T1, 0.5f)} , Expected: (0.50, 0.00, 0.00) ");
        Debug.Log($"t = 0.75; Result : {SH(P0, P1, T0, T1, 0.75f)} , Expected: (0.84, 0.00, 0.00) ");
        Debug.Log($"t = 1; Result : {SH(P0, P1, T0, T1, 1f)} , Expected: (1.00, 0.00, 0.00) ");

        // Third scenario (inverse)
        Debug.Log("INVERSE --------------------------------------------------------------------------------");

        T0 = new Vector3(1f, 0f, 0f);
        T1 = new Vector3(-1f, 0f, 0f);

        Debug.Log($"t = 0; Result : {SH(P0, P1, T0, T1, 0f)} , Expected: (0.00, 0.00, 0.00) ");
        Debug.Log($"t = 0.25; Result : {SH(P0, P1, T0, T1, 0.25f)} , Expected: (0.40, 0.00, 0.00) ");
        Debug.Log($"t = 0.5; Result : {SH(P0, P1, T0, T1, 0.5f)} , Expected: (0.50, 0.00, 0.00) ");
        Debug.Log($"t = 0.75; Result : {SH(P0, P1, T0, T1, 0.75f)} , Expected: (0.60, 0.00, 0.00) ");
        Debug.Log($"t = 1; Result : {SH(P0, P1, T0, T1, 1f)} , Expected: (1.00, 0.00, 0.00) ");

        // Fourth scenario (3D)
        Debug.Log("3D --------------------------------------------------------------------------------");

        P0 = new Vector3(1f, 2f, 3f);
        P1 = new Vector3(4f, 5f, 6f);

        T0 = new Vector3(0f, 1f, 0f);
        T1 = new Vector3(0f, -1f, 0f);

        Debug.Log($"t = 0; Result : {SH(P0, P1, T0, T1, 0f)} , Expected: (1.00, 2.00, 3.00) ");
        Debug.Log($"t = 0.25; Result : {SH(P0, P1, T0, T1, 0.25f)} , Expected: (1.9, 2.7, 3.75) ");
        Debug.Log($"t = 0.5; Result : {SH(P0, P1, T0, T1, 0.5f)} , Expected: (2.5, 3.0, 4.5) ");
        Debug.Log($"t = 0.75; Result : {SH(P0, P1, T0, T1, 0.75f)} , Expected: (3.4, 2.7, 5.25) ");
        Debug.Log($"t = 1; Result : {SH(P0, P1, T0, T1, 1f)} , Expected: (4.00, 5.00, 6.00) ");
    }

    // STEPS BY STEPS
    // --------------

    // The curve is defined by two points and two tangents
    //                              P0 and P1 => Begin and End points
    //                              T0 and T1 => Begin and End tangents
    // The Hermite curve is defined like so : H(t) = h1(t) * P0 + h2(t) * P1 + h3(t) * T0 + h4(t) * T1
    // with
    //                             h1(t) = 2t^3 - 3t^2 + 1
    //                             h2(t) = -2t^3 + 3t^2
    //                             h3(t) = t^3 - 2t^2 + t
    //                             h4(t) = t^3 - t^2
    // H(0) = P0 ; H(1) = P1
    // The derivative H'(t) = T0 at t = 0
    //            and H'(t) = T1 at t = 1

    // FUNCTION TO COMPUTE HERMITE POINT => SH(t) = T . MH . GH
    private Vector3 SH(Vector3 P0, Vector3 P1, Vector3 T0, Vector3 T1, float t)
    {                
        Debug.Assert(t >= 0f && t <= 1f, "t must be in [0,1]");

        float t3 = t * t * t;
        float t2 = t * t;

        Vector3 h1P0 = ( (2f * t3)  - (3f * t2) + 1f) * P0;
        Vector3 h2P1 = ( (-2f * t3) + (3f * t2))  * P1;
        Vector3 h3T0 = ( t3 - (2f * t2) + t ) * T0;
        Vector3 h4T1 = ( t3 - t2 )  * T1;

        Vector3 result = h1P0 + h2P1 + h3T0 + h4T1;
        return result;
    }
}
