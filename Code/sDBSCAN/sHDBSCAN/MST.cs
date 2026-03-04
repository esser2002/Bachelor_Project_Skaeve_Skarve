
using OOPsDBSCAN;

namespace sHDBSCAN;

public static class MST
{
    public static PriorityQueue<Edge, double> CreateSpanningTree(HNode initialNode)
    {
        HashSet<HNode> visited = new HashSet<HNode>();
        PriorityQueue<Edge, double> edges = new ();
        PriorityQueue<Edge, double> graph = new ();
        
        visited.Add(initialNode);

        foreach (var item in initialNode.mutualReachability)
        {
            edges.Enqueue(new Edge(initialNode, item.Key), item.Value);
        }

        while (edges.Count > 0)
        {
            edges.TryDequeue(out Edge edge, out double weight);
            
            HNode toNode = edge!.To; 
            
            if (visited.Contains(toNode))
            {
                continue;
            }
            graph.Enqueue(edge, weight);
            visited.Add(toNode);

            foreach (var item in toNode.mutualReachability)
            {
                edges.Enqueue(new Edge(toNode, item.Key), item.Value);
            }
        }

        return graph;
    }
}
