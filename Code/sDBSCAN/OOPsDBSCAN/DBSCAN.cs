using System.Diagnostics;
using FSharpx.Collections.Tagged;

namespace OOPsDBSCAN;

public static class DBSCAN
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
    
}
