using System.Diagnostics;
using MathNet.Numerics.Distributions;

namespace OOPsDBSCAN;

public static class Preprocessing
{
    /// <summary>
    /// The preprocessing step as described under Algorithm 3 in SDCRP by Xu and Pham
    /// </summary>
    /// <param name="X">the set of all vectors</param>
    /// <param name="randomVectors">the set of random vectors ri</param>
    /// <param name="k">k nearest and furthest random vectors</param>
    /// <param name="m">minPoints ish</param>
    public static void Preprocess(List<Node> X, List<Node> randomVectors, int k, int m)
    {
        Stopwatch stopwatch = new Stopwatch(); 
        stopwatch.Start();
       findNearestAndFurthestVectors(X,randomVectors, k);
       findNearestAndFurthestVectors(randomVectors,X, m);
       stopwatch.Stop();
       TimeSpan ts = stopwatch.Elapsed;
       string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
           ts.Hours, ts.Minutes, ts.Seconds,
           ts.Milliseconds / 10);
       Console.WriteLine("RunTime " + elapsedTime);
    }

    private static void findNearestAndFurthestVectors(List<Node> source, List<Node> target, int amount) 
    {
        foreach (Node sourceNode in source)
        {
            PriorityQueue<Node, double> nearest = new ();
            PriorityQueue<Node, double> furthest = new ();
            
            foreach (Node targetNode in target)
            {
                Double dist = sourceNode.Dist(targetNode);
                if (nearest.Count < amount) // if they are not full
                {
                    Console.WriteLine("inserting " + -dist);
                    nearest.Enqueue(targetNode, -dist);
                    furthest.Enqueue(targetNode, dist);
                }
                else
                {
                    nearest.TryPeek( out _, out Double nearestOtherDist);
                    if (nearestOtherDist < -dist)
                    {
                        nearest.Dequeue();
                        nearest.Enqueue(targetNode, -dist);
                    }
                    
                    furthest.TryPeek( out _, out double furthestOtherDist);
                    if (furthestOtherDist < dist)
                    {
                        furthest.Dequeue();
                        furthest.Enqueue(targetNode, dist);
                    }
                }
            }
            sourceNode.Nearest = nearest.UnorderedItems.Select(n => n.Element).ToArray();
            sourceNode.Furthest = furthest.UnorderedItems.Select(n => n.Element).ToArray();
        }
    }

    /// <summary>
    /// Generated gaussian random vectors
    /// </summary>
    /// <param name="D">The number of random vectors</param>
    /// <param name="d">The amount of dimensions</param>
    /// <returns></returns>
    public static List<Node> GenerateRandomVectors(int D, int d)
    {
        List<Node> randomVectors = new List<Node>();
        Normal random = new Normal(); // Gaussian normal - mean 0, standard deviation 1
        
        for (int i = 0; i < D; i++)
        {
            Node node = new Node(d);
            randomVectors.Add(node);
            for (int j = 0; j < d; j++)
            {
                double value = random.Sample();
                node.Vector[j] = value;
            }
        }
        return randomVectors;
    }
}
