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
    private Hermite hermite = new Hermite();

    [SerializeField] public AlgorithmType selectedAlgorithm = AlgorithmType.Hermite;


    private void OnEnable()
    {
        switch (selectedAlgorithm)
        {
            case AlgorithmType.Hermite:
                hermite.Testing();
                //hermite.HermiteCalc(gameObject);
                break;
            case AlgorithmType.Bezier:

                break;
            case AlgorithmType.B_Spline:

                break;
            case AlgorithmType.Catmull_Rom:

                break;
            default:
                break;
        }
    }
}
