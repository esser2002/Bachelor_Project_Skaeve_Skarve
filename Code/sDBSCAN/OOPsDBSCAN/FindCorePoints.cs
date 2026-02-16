using System.Diagnostics;
using FSharpx.Collections.Tagged;

namespace OOPsDBSCAN;

public static class FindCorePoints
{
    /// <summary>
    /// Finds the approximate set of core points and their neighborhoods.
    /// </summary>
    /// <param name="X">A preprocessed collection of nodes</param>
    /// <param name="epsilon">Distance required to be neighbors</param>
    /// <param name="minPts">The minimum number of neighbors required to be considered core</param>
    /// <returns>A dictionary of all core points, mapping to their neighborhood</returns>
    public static Dictionary<Node, HashSet<Node>> FindCorePointsAndNeighbors(List<Node> X, double epsilon, int minPts)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Dictionary<Node, HashSet<Node>> neighborhoods = new();

        foreach (Node node in X)
        {
            neighborhoods[node] = new ();
        }
        
        foreach (Node q in X)
        {
            foreach (Node r in q.Nearest!)//These are the k-nearest random vectors
            {
                foreach (var x in r.Nearest!)//These are the m-nearest vectors
                {
                    if (x.AbsScalar(q) >= epsilon)
                    {
                        neighborhoods[q].Add(x);
                        neighborhoods[x].Add(q);
                    }
                }
            }
            
            foreach (Node r in q.Furthest!)//These are the k-furthest random vectors
            {
                foreach (var x in r.Furthest!)//These are the m-furthest vectors
                {
                    if (x.AbsScalar(q) >= epsilon)
                    {
                        neighborhoods[q].Add(x);
                        neighborhoods[x].Add(q);
                    }
                }
            }
        }

        foreach (Node q in X)
        {
            if (neighborhoods[q].Count >= minPts)
            {
                q.CorePoint = true;
            }
            else
            {
                neighborhoods.Remove(q);
            }
        }   
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("RunTime " + elapsedTime);
        return neighborhoods;
    }
}