namespace sHDBSCAN;

public static class MST
{
    /// <summary>
    /// Create a minimum spanning tree from the initial node.
    /// </summary>
    public static PriorityQueue<Edge, double> CreateSpanningTree(HNode initialNode)
    {
        HashSet<HNode> visited = new HashSet<HNode>();
        PriorityQueue<Edge, double> edges = new ();
        PriorityQueue<Edge, double> graph = new ();
        
        visited.Add(initialNode);

        foreach (var item in initialNode.VisibleNodes)
        {
            HNode node = Util.dataPoints[item];
            edges.Enqueue(new Edge(initialNode, node), initialNode.GetReachability(node));
        }

        while (edges.Count > 0)
        {
            edges.TryDequeue(out Edge? edge, out double weight);
            
            HNode toNode = edge!.To; 
            
            if (visited.Contains(toNode))
            {
                continue;
            }
            graph.Enqueue(edge, weight);
            visited.Add(toNode);

            foreach (var item in toNode.VisibleNodes)
            {
                HNode node = Util.dataPoints[item];
                edges.Enqueue(new Edge(toNode, node), toNode.GetReachability(node));
            }
        }
        return graph;
    }
}
