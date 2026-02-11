using MathNet.Numerics.Distributions;

namespace OOPsDBSCAN;

public static class Preprocessing
{
    /// <summary>
    /// The preprocessing step, as described under 'Algorithm 3' in SDCRP by Xu & Pham
    /// </summary>
    ///<param name="X">the set of all vectors</param>
    ///<param name="Rd">the set of random vectors ri</param>
    ///<param name="k">k nearest and furthest random vectors</param>
    ///<param name="m">minPoints ish</param>
    /// <returns></returns>
    public static void Preprocess(List<Node> X, List<Node> RandomVectors, int k, int m)
    {
        foreach (Node node in X)
        {
            PriorityQueue<Node, Double> nearest = new PriorityQueue<Node, Double>();
            PriorityQueue<Node, Double> furthest = new PriorityQueue<Node, Double>();
            
            foreach (Node randomvector in RandomVectors)
            {
                Double similarity = node.Scalar(randomvector);
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
                    
                    furthest.TryPeek( out _, out Double furthestOtherSimilarity);
                    if (furthestOtherSimilarity < -similarity)
                    {
                        nearest.Dequeue();
                        nearest.Enqueue(randomvector, -similarity);
                    }
                }
            }
            node.Nearest = nearest.UnorderedItems.Select(n => n.Element).ToArray();
            node.Furthest = furthest.UnorderedItems.Select(n => n.Element).ToArray();
        }
    }

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
