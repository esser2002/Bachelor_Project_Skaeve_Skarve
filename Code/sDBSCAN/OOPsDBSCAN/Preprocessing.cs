using MathNet.Numerics.Distributions;

namespace OOPsDBSCAN;

public static class Preprocessing
{
    /// <summary>
    /// The preprocessing step, as described under 'Algorithm 3' in SDCRP by Xu & Pham
    /// </summary>
    ///<param name="X">the set of all vectors</param>
    ///<param name="randomVectors">the set of random vectors ri</param>
    ///<param name="k">k nearest and furthest random vectors</param>
    ///<param name="m">minPoints ish</param>
    /// <returns></returns>
    public static void Preprocess(List<Node> X, List<Node> randomVectors, int k, int m)
    {
        foreach (Node qNode in X)
        {
            PriorityQueue<Node, double> nearest = new ();
            PriorityQueue<Node, double> furthest = new ();
            
            foreach (Node randomvector in randomVectors)
            {
                Double similarity = qNode.AbsScalar(randomvector);
                if (nearest.Count < k) // if they are not full
                {
                    nearest.Enqueue(randomvector, similarity);
                    furthest.Enqueue(randomvector, -similarity);
                }
                else
                {
                    nearest.TryPeek( out _, out Double nearestOtherSimilarity);
                    if (nearestOtherSimilarity < similarity)
                    {
                        nearest.Dequeue();
                        nearest.Enqueue(randomvector, similarity);
                        
                    }
                    
                    furthest.TryPeek( out _, out double furthestOtherSimilarity);
                    if (furthestOtherSimilarity < -similarity)
                    {
                        furthest.Dequeue();
                        furthest.Enqueue(randomvector, -similarity);
                    }
                }
            }
            qNode.Nearest = nearest.UnorderedItems.Select(n => n.Element).ToArray();
            qNode.Furthest = furthest.UnorderedItems.Select(n => n.Element).ToArray();
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
