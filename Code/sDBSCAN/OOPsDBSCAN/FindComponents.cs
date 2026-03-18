using System.Diagnostics;
using FSharpx.Collections.Tagged;

namespace OOPsDBSCAN;

public static class FindComponents
{
    public static void DoDBSCAN(Dictionary<Node, HashSet<Node>> C)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Dictionary<Node, HashSet<Node>> neighborhoods = new();
        foreach (KeyValuePair<Node,HashSet<Node>> keyValuePair in C)
        {
            Node q = keyValuePair.Key;
            q.Connected = true;
            
            foreach (Node x in keyValuePair.Value)
            {
                if (x.CorePoint)
                {
                    q.Edges.Add(x);
                }
                else if (!x.Connected)
                {
                    x.Connected = true;
                    q.Edges.Add(x);
                }
            }
        }
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("RunTime " + elapsedTime);
    }
    
    public static List<HashSet<Node>> GetClusters(List<Node> CorePoints)
    {
        List<HashSet<Node>> clusters = new();
        HashSet<Node> looseCorePoints = new HashSet<Node>(CorePoints);

        while (looseCorePoints.Count > 0)
        {
            Node q = looseCorePoints.First();
            
            List<Node> openList = new List<Node>();
            HashSet<Node> closedList = new HashSet<Node>();
            
            openList.Add(q);
            closedList.Add(q);
            
            HashSet<Node> cluster = new HashSet<Node>();
            while (openList.Count > 0)
            {
                Node node = openList[0];
                openList.RemoveAt(0);
                cluster.Add(node);
                if (node.CorePoint)
                {
                    looseCorePoints.Remove(node);
                    foreach (Node edge in node.Edges)
                    {
                        if (!closedList.Contains(edge))
                        {
                            openList.Add(edge);
                            closedList.Add(edge);
                        }
                    }
                }
            }
            clusters.Add(cluster);
        }

        return clusters;
    }
}
