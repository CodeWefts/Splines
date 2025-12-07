using System.Collections;
using System.Numerics;
using UnityEngine;

[ExecuteInEditMode]
public class AlgorithmSelection : MonoBehaviour
{
    public enum AlgorithmType
    {
        Hermite,
        Bezier,
        B_Spline,
        Catmull_Rom
    }

    [SerializeField] public AlgorithmType selectedAlgorithm = AlgorithmType.Hermite;

    private void OnEnable()
    {
        switch(selectedAlgorithm)
        {
            case AlgorithmType.Hermite:
                Debug.Log("################################## HERMITE #########################################");
                Hermite.Testing();
                break;
            case AlgorithmType.Bezier:
                Debug.Log("################################## BEZIER #########################################");
                Bezier.Testing();
                break;
            case AlgorithmType.B_Spline:
                // Initialize B-Spline algorithm
                break;
            case AlgorithmType.Catmull_Rom:
                // Initialize Catmull-Rom algorithm
                break;
            default:
                break;
        }
    }
}
