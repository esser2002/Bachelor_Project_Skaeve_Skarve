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
    public static List<Node> Preprocess(List<Node> X, List<Node> Rd, int k, int m)
    {
        throw new NotImplementedException();
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
