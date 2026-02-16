using System.Numerics;
using Microsoft.FSharp.Collections;
using sDBSCAN;

namespace OOPsDBSCAN;

public class Node
{
    public int Label;
    public double[] Vector;
    public Node[]? Nearest;
    public Node[]? Furthest;
    public bool CorePoint;

    public Node(string[] input)
    {
        Label = int.Parse(input[0]);
        Vector = new double[input.Length - 1];
        for (int i = 1; i < input.Length; i++)
        {
            Vector[i - 1] = int.Parse(input[i]);
        }
        
    }

    public Node(int dimensions)
    {   
        Label = -1; // Value for nodes not in the dataset
        Vector = new double[dimensions];
    }

    public double AbsScalar(Node other)
    {
        return double.Abs(algoDBSCAN.scalar(ListModule.OfSeq(Vector) , ListModule.OfSeq(other.Vector)));
    }

    public void Normalise()
    {
        Vector = algoDBSCAN.normalise(ListModule.OfSeq(Vector)).ToArray();
    }

    public double Length()
    {
        return algoDBSCAN.length(ListModule.OfSeq(Vector));
    }

    public override string ToString()
    {
        return "label " + Label + " : " + string.Join(", ", Vector);
    }
}