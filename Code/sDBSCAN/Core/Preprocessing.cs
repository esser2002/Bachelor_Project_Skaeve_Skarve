using MathNet.Numerics.Distributions;

namespace Core;

public static class Preprocessing
{
    const int NumberOfThreads = 8; // number of threads is set to 8 since most laptops have 8 or fewer cores 
    /// <summary>
    /// The preprocessing step as described under Algorithm 3 in SDCRP by Xu and Pham
    /// </summary>
    /// <param name="X">the set of all vectors</param>
    /// <param name="randomVectors">the set of random vectors ri</param>
    /// <param name="k">amount of random vectors each datapoint knows</param>
    /// <param name="m">amount of datapoints each random vector knows </param>
    public static void Preprocess(List<Node> X, List<Node> randomVectors, int k, int m)
    {
       FindNearestAndFurthestVectors(X,randomVectors, k);
       FindNearestAndFurthestVectors(randomVectors,X, m);
    }

    /// <summary>
    /// For each node in <paramref name="source"/> the amount nearest and furthest points in target is found and stored
    /// </summary>
    /// <param name="source"> List of nodes that need to find and store their nearest and furthest nodes </param>
    /// <param name="target"> List of nodes that source targets </param>
    /// <param name="amount"> amount of nodes that is stored </param>
    private static void FindNearestAndFurthestVectors(List<Node> source, List<Node> target, int amount)
    {
        List<Task> threads = new();
        
        for (int i = 0; i < NumberOfThreads; i++)
        {
            var from = source.Count * i / NumberOfThreads ;
            var to = source.Count * (i + 1) / NumberOfThreads ;
            Task t = Task.Run(() => ScopedFindNearestAndFurthestVectors(from, to, source, target, amount)); 
            threads.Add(t);
        }

        Task.WaitAll(threads);
    }

    private static void ScopedFindNearestAndFurthestVectors(int from, int to, List<Node> source, List<Node> target, int amount) 
    {
        for (int i = from; i < to; i++)
        {
            Node sourceNode = source[i];
            PriorityQueue<Node, double> nearest = new ();
            PriorityQueue<Node, double> furthest = new ();
            
            foreach (Node targetNode in target)
            {
                double dist = sourceNode.Dist(targetNode);
                if (nearest.Count < amount) // if the queue is not full
                {
                    nearest.Enqueue(targetNode, -dist);
                    furthest.Enqueue(targetNode, dist);
                }
                else
                {
                    nearest.TryPeek( out _, out double nearestOtherDist);
                    // the list needs to be ordered from smallest to largest. Therefore, the negative dist is used
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
    /// Generate gaussian random vectors
    /// </summary>
    /// <param name="D">The number of random vectors</param>
    /// <param name="d">The amount of dimensions</param>
    public static List<Node> GenerateRandomVectors(int D, int d)
    {
        List<Node> randomVectors = new ();
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
