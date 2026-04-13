namespace OOPsDBSCAN;

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
}