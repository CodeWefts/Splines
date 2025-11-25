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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Switch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch()
    {
        switch (selectedAlgorithm)
        {
            case AlgorithmType.Hermite:
                selectedAlgorithm = AlgorithmType.Bezier;
                break;
            case AlgorithmType.Bezier:
                selectedAlgorithm = AlgorithmType.B_Spline;
                break;
            case AlgorithmType.B_Spline:
                selectedAlgorithm = AlgorithmType.Catmull_Rom;
                break;
            case AlgorithmType.Catmull_Rom:
                selectedAlgorithm = AlgorithmType.Hermite;
                break;
        }
    }
}
