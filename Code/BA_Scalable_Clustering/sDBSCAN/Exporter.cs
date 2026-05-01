namespace sDBSCAN;

public static class Exporter
{
    public static void ExportClusters(string path, List<Node> nodes)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);

        using (StreamWriter outputFile = new StreamWriter(path))
        {
            foreach (Node node in nodes)
            {
                outputFile.WriteLine(node.ClusterId);
            }

            Console.WriteLine("List of nodes with cluster labels exported to " +
                              ((FileStream)outputFile.BaseStream).Name);
        }
    }
    public static void ExportsDBSCANStats(string path, int D, int k, int m, double epsilon, int minPts, int datasize, string elapsedTime )
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException("No path was provided."));
      
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            outputFile.WriteLine($"{D},{k},{m},{epsilon},{minPts},{datasize},{elapsedTime}");
            Console.WriteLine("sDBSCANstats exported to " + ((FileStream)outputFile.BaseStream).Name);
        }
    }
}