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
}
