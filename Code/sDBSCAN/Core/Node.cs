using System.Collections.Concurrent;
using Microsoft.FSharp.Collections;

namespace Core;

public class Node
{
    public int Label;
    public double[] Vector;
    public Node[]? Nearest;
    public Node[]? Furthest;
    public bool CorePoint;

    private ConcurrentDictionary<Node, double> distances = new ();
    
    //Graph stuff
    public List<Node> Edges = [];
    public bool Connected;

    public Node(string[] input)
    {
        Label = int.Parse(input[0]);
        Vector = new double[input.Length - 1];
        for (int i = 1; i < input.Length; i++)
        {
            Vector[i - 1] = int.Parse(input[i]);
        }
        distances.TryAdd(this, 0);
    }

    public Node(int dimensions)
    {   
        Label = -1; // Value for nodes not in the dataset
        Vector = new double[dimensions];
        distances.TryAdd(this, 0);
    }

    public double Dist(Node other)
    {
        if (distances.TryGetValue(other, out double dist))
        {
            return dist;
        }
        
        double scalar = GetScalar(other.Vector);
        var newdist = (1.0 - scalar);
        distances.TryAdd(other, newdist);
        other.distances.TryAdd(this, newdist);
        return newdist;
    }

    private double GetScalar(double[] otherVector)
    {
        double scalar = 0;
        for (int i = 0; i < Vector.Length; i++)
        {
            scalar += Vector[i] * otherVector[i];
        }

        return scalar;
    }
    
    public void Normalise()
    {
        double length = GetLength();
        for (int i = 0; i < Vector.Length; i++)
        {
            Vector[i] /= length;
        }
    }

    private double GetLength()
    {
        double squaredSum = 0;
        for (int i = 0; i < Vector.Length; i++)
        {
            squaredSum += Vector[i] * Vector[i];
        }

        return Math.Sqrt(squaredSum);
    }

    public override string ToString()
    {
        return "label " + Label + " : " + string.Join(", ", Vector);
    }
}