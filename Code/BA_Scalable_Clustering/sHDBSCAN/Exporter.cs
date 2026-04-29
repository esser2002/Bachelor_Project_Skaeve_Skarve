using System.Globalization;
using System.Text;

namespace sHDBSCAN;

public static class Exporter
{
    public static void ExportMst(string path, PriorityQueue<Edge, double> mst)
    {
        var newMst = ClonePriorityQueue(mst);

        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            outputFile.WriteLine("fromId;fromLabel;toId;toLabel;distance");
            while (newMst.TryDequeue(out Edge edge, out double distance))
            {
                StringBuilder builder = new();
                builder.Append($"{edge.From.Id};{edge.From.Label};");
                builder.Append($"{edge.To.Id};{edge.To.Label};");
                builder.Append(distance);
                outputFile.WriteLine(builder);
            }
            Console.WriteLine("MST exported to " + ((FileStream)outputFile.BaseStream).Name);
        }
        
    }

    public static void ExportDendrogram(string path, (int l, int r, double dist, int size)[] dendrogram)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");//double uses . instead of ,  
        
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException());
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            outputFile.WriteLine("left;right;distance;clusterSize");
            foreach (var tuple in dendrogram)
            {
                StringBuilder builder = new();
                builder.Append($"{tuple.l};{tuple.r};{tuple.dist};{tuple.size}");
                outputFile.WriteLine(builder);
            }
            Console.WriteLine("Dendrogram exported to " + ((FileStream)outputFile.BaseStream).Name);
        }

        
    }
    private static PriorityQueue<Edge, double> ClonePriorityQueue(PriorityQueue<Edge, double> queue)
    {
        PriorityQueue<Edge,double> newQueue = new();
        foreach (var (edge, priority) in queue.UnorderedItems)
        {
            newQueue.Enqueue(edge, priority);
        }

        return newQueue;
    }
    
    public static void ExportsHdbscanStats(string path, int D,int l, int m, int  k, int datasize, string elapsedTime )
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? throw new InvalidOperationException("No path was provided."));
      
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            outputFile.WriteLine($"{D},{l},{m},{k},{datasize},{elapsedTime}");
            Console.WriteLine("sHDBSCAN exported to " + ((FileStream)outputFile.BaseStream).Name);
        }
    }
}