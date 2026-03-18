using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using OOPsDBSCAN;

namespace sHDBSCAN;

public static class Exporter
{
    public static void ExportMST(string path, PriorityQueue<Edge, double> MST)
    {
        var newMST = clonePriorityQueue(MST);

        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            outputFile.WriteLine("fromId;fromLabel;toId;toLabel;distance");
            while (newMST.TryDequeue(out Edge edge, out double distance))
            {
                StringBuilder builder = new();
                builder.Append($"{edge.From.id};{edge.From.Label};");
                builder.Append($"{edge.To.id};{edge.To.Label};");
                builder.Append(distance);
                outputFile.WriteLine(builder);
            }
            Console.WriteLine("MST exported to " + ((FileStream)outputFile.BaseStream).Name);
        }
        
    }
    public static void ExportNormalisedData(string path, List<HNode> nodelist)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");//double uses . instead of ,  
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            foreach (HNode node in nodelist)
            {
                StringBuilder builder = new();
                for (int i = 0 ; i < (node.Vector.Length); i++)
                {
                    if (i == node.Vector.Length - 1)
                    {
                        builder.Append($"{node.Vector[i]}");
                    }
                    else
                    {
                        builder.Append($"{node.Vector[i]};");
                    }
                }
                outputFile.WriteLine(builder);
            }
            Console.WriteLine("Normalised nodes added to csv file complete");
        }

    }

    public static void ExportDendrogram(string path, (int l, int r, double dist, int size)[] dendrogram)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");//double uses . instead of ,  
        
        Directory.CreateDirectory(Path.GetDirectoryName(path));
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
    private static PriorityQueue<Edge, double> clonePriorityQueue(PriorityQueue<Edge, double> queue)
    {
        PriorityQueue<Edge,double> newQueue = new();
        foreach (var (edge, priority) in queue.UnorderedItems)
        {
            newQueue.Enqueue(edge, priority);
        }

        return newQueue;
    }
}