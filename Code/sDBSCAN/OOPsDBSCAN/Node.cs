using System.Numerics;
using Microsoft.FSharp.Collections;
using sDBSCAN;

namespace OOPsDBSCAN;

public class Node
{
    public int Tag;
    public double[] Vector;

    public Node(string[] input)
    {
        Tag = int.Parse(input[0]);
        Vector = new double[input.Length - 1];
        for (int i = 1; i < input.Length; i++)
        {
            Vector[i - 1] = int.Parse(input[i]);
        }
    }

    public double Scalar(Node other)
    {
        return algoDBSCAN.scalar(ListModule.OfSeq(Vector) , ListModule.OfSeq(other.Vector));
    }

    public void normalise()
    {
        Vector = algoDBSCAN.normalise(ListModule.OfSeq(Vector)).ToArray();
    }

    public double length()
    {
        return algoDBSCAN.length(ListModule.OfSeq(Vector));
    }
}