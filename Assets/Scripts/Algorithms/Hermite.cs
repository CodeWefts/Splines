using UnityEngine;

public class Hermite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    // Step 1 : Write a function that takes in parameters P0, P1, T0, T1 and t. Return H(t)

    // Step 2 : Write a function that takes in parameters (float t) and returns the scalar basis functions h1(t), h2(t), h3(t), h4(t)
    
}
