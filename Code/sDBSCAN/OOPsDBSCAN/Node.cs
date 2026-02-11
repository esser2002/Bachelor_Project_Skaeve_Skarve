using Microsoft.FSharp.Collections;
using sDBSCAN;

namespace OOPsDBSCAN;

public class Node
{
    public int Tag;
    public int[] Vector;

    public Node(string[] input)
    {
        Tag = int.Parse(input[0]);
        Vector = new int[input.Length - 1];
        for (int i = 1; i < input.Length; i++)
        {
            Vector[i - 1] = int.Parse(input[i]);
        }
    }

    public int Scalar(Node other)
    {
        return algoDBSCAN.scalar(ListModule.OfSeq(Vector) , ListModule.OfSeq(other.Vector));
    }
}