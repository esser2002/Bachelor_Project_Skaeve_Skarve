using ConcurrentCollections;

namespace sDBSCAN;

public static class FindCorePoints
{
    private const int NumberOfThreads = 8;
    /// <summary>
    /// Finds the approximate set of core points and their neighborhoods.
    /// </summary>
    /// <param name="X">A preprocessed collection of nodes</param>
    /// <param name="epsilon">Distance required to be neighbors</param>
    /// <param name="minPts">The minimum number of neighbors required to be considered core</param>
    /// <returns>A dictionary of all core points, mapping to their neighborhood</returns>
    public static Dictionary<Node, HashSet<Node>> FindCorePointsAndNeighbors(List<Node> X, double epsilon, int minPts)
    {
        Dictionary<Node, ConcurrentHashSet<Node>> neighborhoods = new();

        foreach (Node node in X)
        {
            neighborhoods[node] = new ();
        }
        
        List<Task> threads = new();
        
        for (int i = 0; i < NumberOfThreads; i++)
        {
            var from = X.Count * i / NumberOfThreads ;
            var to = X.Count * (i + 1) / NumberOfThreads ;
            Task t = Task.Run(() => BuildNeighbourhoods(from, to, X, epsilon, neighborhoods));
            threads.Add(t);
        }

        Task.WaitAll(threads);

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
        return neighborhoods.Select(x => (x.Key,x.Value.ToHashSet())).ToDictionary();
    }

    /// <summary>
    /// Compares all vectors in the range between from and to, to their approximately nearby nodes, and adds them to their neighbourhoods
    /// </summary>
    /// <param name="from">starting index</param>
    /// <param name="to">ending index (exclusive)</param>
    /// <param name="X">Set of all data points</param>
    /// <param name="epsilon">Distance required to be neighbors</param>
    /// <param name="neighbourhoods"></param>
    private static void BuildNeighbourhoods(int from, int to, List<Node> X, double epsilon, Dictionary<Node, ConcurrentHashSet<Node>> neighbourhoods)
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
                        neighbourhoods[q].Add((Node)x);
                        neighbourhoods[(Node)x].Add(q);
                    }
                }
            }
            
            foreach (Core.Node r in q.Furthest!)//These are the k-furthest random vectors
            {
                foreach (var x in r.Furthest!)//These are the m-furthest vectors
                {
                    if (x.Dist(q) <= epsilon)
                    {
                        neighbourhoods[q].Add((Node)x);
                        neighbourhoods[(Node)x].Add(q);
                    }
                }
            }
        }
    }
}