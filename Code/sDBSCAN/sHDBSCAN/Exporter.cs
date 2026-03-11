using System.Text;

namespace sHDBSCAN;

public static class Exporter
{
    public static void ExportMST(string path, PriorityQueue<Edge, double> MST)
    {
        var newMST = clonePriorityQueue(MST);

        Directory.CreateDirectory(Path.GetDirectoryName(path));
        using (StreamWriter outputFile = new StreamWriter(path))
        {
            outputFile.WriteLine("fromId,fromLabel,toId,toLabel,distance");
            while (newMST.TryDequeue(out Edge edge, out double distance))
            {
                StringBuilder builder = new();
                builder.Append($"{edge.From.id},{edge.From.Label},");
                builder.Append($"{edge.To.id},{edge.To.Label},");
                builder.Append(distance);
                outputFile.WriteLine(builder);
            }
            Console.WriteLine("MST exported to " + ((FileStream)outputFile.BaseStream).Name);
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