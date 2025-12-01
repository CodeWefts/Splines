using UnityEngine;

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

    public void Switch()
    {
        switch (selectedAlgorithm)
        {
            case AlgorithmType.Hermite:
                
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
