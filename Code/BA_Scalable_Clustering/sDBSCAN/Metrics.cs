using System.Text;

namespace sDBSCAN;

public static class Metrics
{
    public static string GetMetrics(List<HashSet<Node>> Clusters, List<Node> dataPoints)
    {
        StringBuilder builder = new();
        builder.AppendLine("number of clusters " + Clusters.Count);
        builder.AppendLine("coverage: " + (100 * Clusters.Sum(c => c.Count) / (double)dataPoints.Count) + " %");
        builder.AppendLine("first cluster: ");
        int stopper = 0;
        foreach (Node node in Clusters[0])
        {
            if (stopper++ > 1000)
            {
                break;
            }

            builder.Append(node.Label);
        }

        builder.AppendLine();

        foreach (HashSet<Node> cluster in Clusters)
        {
            int[] freq = new int[10];
            foreach (Node node in cluster)
            {
                freq[node.Label] += 1;
            }

            builder.AppendLine("Cluster length " + cluster.Count + " contents:");
            for (int i = 0; i < freq.Length; i++)
            {
                builder.AppendLine(" - " + i + ": " + freq[i]);
            }
        }

        return builder.ToString();
    }
}