using System.Diagnostics;
using ConcurrentCollections;

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
        Dictionary<Node, ConcurrentHashSet<Node>> neighborhoods = new();

        foreach (Node node in X)
        {
            neighborhoods[node] = new ();
        }

        int nOfTheads = 8;
        List<Task> theads = new();
        
        for (int i = 0; i < nOfTheads; i++)
        {
            var from = X.Count * i / nOfTheads ;
            var to = X.Count * (i + 1) / nOfTheads ;
            Console.WriteLine("Starting find core points thread to process from " + from + " to " + to);
            Task t = Task.Run(() => compareVectors(from, to, X, epsilon, neighborhoods));
            theads.Add(t);
        }

        Task.WaitAll(theads);

        foreach (Node q in X)
        {
            if (neighborhoods[q].Count >= minPts)
            {
                q.CorePoint = true;
            }
            else
            {
                neighborhoods.Remove(q,out _);
            }
        }   
        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("RunTime " + elapsedTime);
        return neighborhoods.Select(x => (x.Key,x.Value.ToHashSet())).ToDictionary();
    }

    private static void compareVectors(int from, int to, List<Node> X, double epsilon, Dictionary<Node, ConcurrentHashSet<Node>> neighborhoods)
    {
        for(int i = from; i < to; i++)
        {
            Node q = X[i];
            foreach (Core.Node r in q.Nearest!)//These are the k-nearest random vectors
            {
                foreach (var x in r.Nearest!)//These are the m-nearest vectors
                {
                    if (x.Dist(q) <= epsilon)
                    {
                        neighborhoods[q].Add((Node)x);
                        neighborhoods[(Node)x].Add(q);
                    }
                }
            }
            
            foreach (Core.Node r in q.Furthest!)//These are the k-furthest random vectors
            {
                foreach (var x in r.Furthest!)//These are the m-furthest vectors
                {
                    if (x.Dist(q) <= epsilon)
                    {
                        neighborhoods[q].Add((Node)x);
                        neighborhoods[(Node)x].Add(q);
                    }
                }
            }
        }
        Console.WriteLine("Thread from " + from + " to " + to + " finished");
    }
}