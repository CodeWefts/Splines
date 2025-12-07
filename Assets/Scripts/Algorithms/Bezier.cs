using UnityEngine;

public class Bezier : MonoBehaviour
{
    // STEPS BY STEPS
    // --------------

    // The curve is defined by four points
    //                              P0 and P3
    //                              P1 and P2
    // The Bezier curve is defined like so : B(t) = (1-t)^3 * P0 + 3(1-t)^2 * t * P1 + 3 * (1-t) * t^2 * P2 + t^3 * P3
    // with
    //                             B(0) = P0
    //                             B(1) = P3
    //                             B'(t) = 3 * (P1 - P0) at t = 0
    //                             B'(t) = 3 * (P3 - P2) at t = 1
    // 

    // FUNCTION TO COMPUTE BEZIER POINT => SB(t) = T . MB . GB

    public static Vector3 SB(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
    { 
        float t2 = (1 - t) * (1 - t);
        float t3 = t2 * (1 - t);

        Vector3 term0 = t3 * P0;
        Vector3 term1 = 3 * t2 * t * P1;
        Vector3 term2 = 3 * (1 - t) * t * t * P2;
        Vector3 term3 = t * t * t * P3;

        Vector3 result = term0 + term1 + term2 + term3;
        return result;
    }

    public static void Testing()
    {
        Debug.Log("Bezier Testing Started");

        // First scenario (ALIGNED)
        Debug.Log("ALIGNED --------------------------------------------------------------------------------");
       
        Vector3 P0 = new Vector3(0f, 0f, 0f);
        Vector3 P1 = new Vector3(1f, 0f, 0f);
        Vector3 P2 = new Vector3(2f, 0f, 0f);
        Vector3 P3 = new Vector3(3f, 0f, 0f);

        Debug.Log($"t = 0; Result : {SB(P0, P1, P2, P3, 0f)} , Expected: (0.00, 0.00, 0.00) ");
        Debug.Log($"t = 0.25; Result : {SB(P0, P1, P2, P3, 0.25f)} , Expected: (0.75, 0.00, 0.00) ");
        Debug.Log($"t = 0.5; Result : {SB(P0, P1, P2, P3, 0.5f)} , Expected: (1.50, 0.00, 0.00) ");
        Debug.Log($"t = 0.75; Result : {SB(P0, P1, P2, P3, 0.75f)} , Expected: (2.25, 0.00, 0.00) ");
        Debug.Log($"t = 1; Result : {SB(P0, P1, P2, P3, 1f)} , Expected: (3.00, 0.00, 0.00) ");

        // Second scenario (CLASSIC SEMICIRCLE)
        Debug.Log("CLASSIC SEMICIRCLE --------------------------------------------------------------------------------");

        P0 = new Vector3(0f, 0f, 0f);
        P1 = new Vector3(1f, 1f, 0f);
        P2 = new Vector3(2f, -1f, 0f);
        P3 = new Vector3(3f, 0f, 0f);

        Debug.Log($"t = 0; Result : {SB(P0, P1, P2, P3, 0f)} , Expected: (0.00, 0.00, 0.00) ");
        Debug.Log($"t = 0.25; Result : {SB(P0, P1, P2, P3, 0.25f)} , Expected: (0.45, 0.56, 0.00) ");
        Debug.Log($"t = 0.5; Result : {SB(P0, P1, P2, P3, 0.5f)} , Expected: (1.50, 0.00, 0.00) ");
        Debug.Log($"t = 0.75; Result : {SB(P0, P1, P2, P3, 0.75f)} , Expected: (2.58, -0.56, 0.00) ");
        Debug.Log($"t = 1; Result : {SB(P0, P1, P2, P3, 1f)} , Expected: (3.00, 0.00, 0.00) ");

        // Third scenario (S CURVE)
        Debug.Log("S CURVE --------------------------------------------------------------------------------");

        P0 = new Vector3(0f, 0f, 0f);
        P1 = new Vector3(0.5f, 1f, 0f);
        P2 = new Vector3(2.5f, -1f, 0f);
        P3 = new Vector3(3f, 0f, 0f);

        Debug.Log($"t = 0; Result : {SB(P0, P1, P2, P3, 0f)} , Expected: (0.00, 0.00, 0.00) ");
        Debug.Log($"t = 0.25; Result : {SB(P0, P1, P2, P3, 0.25f)} , Expected: (0.15, 0.42, 0.00) ");
        Debug.Log($"t = 0.5; Result : {SB(P0, P1, P2, P3, 0.5f)} , Expected: (1.50, 0.00, 0.00) ");
        Debug.Log($"t = 0.75; Result : {SB(P0, P1, P2, P3, 0.75f)} , Expected: (2.83, -0.42, 0.00) ");
        Debug.Log($"t = 1; Result : {SB(P0, P1, P2, P3, 1f)} , Expected: (3.00, 0.00, 0.00) ");

        // Fourth scenario (IDENTICAL POINTS)
        Debug.Log("IDENTICAL POINTS --------------------------------------------------------------------------------");

        P0 = new Vector3(0f, 0f, 0f);
        P1 = new Vector3(0f, 0f, 0f);
        P2 = new Vector3(0f, 0f, 0f);
        P3 = new Vector3(0f, 0f, 0f);

        Debug.Log($"t = 0; Result : {SB(P0, P1, P2, P3, 0f)} , Expected: (0.00, 0.00, 0.00) ");
        Debug.Log($"t = 0.25; Result : {SB(P0, P1, P2, P3, 0.25f)} , Expected: (0.00, 0.00, 0.00) ");
        Debug.Log($"t = 0.5; Result : {SB(P0, P1, P2, P3, 0.5f)} , Expected: (0.00, 0.00, 0.00) ");
        Debug.Log($"t = 0.75; Result : {SB(P0, P1, P2, P3, 0.75f)} , Expected: (0.00, 0.00, 0.00) ");
        Debug.Log($"t = 1; Result : {SB(P0, P1, P2, P3, 1f)} , Expected: (0.00, 0.00, 0.00) ");

        // Fifth scenario (3D COMPLEXE)
        Debug.Log("3D COMPLEXE --------------------------------------------------------------------------------");

        P0 = new Vector3(1f, 2f, 3f);
        P1 = new Vector3(2f, 4f, 1f);
        P2 = new Vector3(5f, 1f, 4f);
        P3 = new Vector3(6f, 3f, 2f);

        Debug.Log($"t = 0; Result : {SB(P0, P1, P2, P3, 0f)} , Expected: (1.00, 2.00, 3.00) ");
        Debug.Log($"t = 0.25; Result : {SB(P0, P1, P2, P3, 0.25f)} , Expected: (1.42, 2.56, 2.25) ");
        Debug.Log($"t = 0.5; Result : {SB(P0, P1, P2, P3, 0.5f)} , Expected: (3.50, 2.50, 2.50) ");
        Debug.Log($"t = 0.75; Result : {SB(P0, P1, P2, P3, 0.75f)} , Expected: (5.08, 2.44, 2.75) ");
        Debug.Log($"t = 1; Result : {SB(P0, P1, P2, P3, 1f)} , Expected: (6.00, 3.00, 2.00) ");

    }

}
