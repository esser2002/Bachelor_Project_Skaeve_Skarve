namespace sHDBSCAN;

public static class MST
{
    /// <summary>
    /// Create a minimum spanning tree.
    /// </summary>

    public static (Edge, double)[] Kruskals(HNode[] nodes, UnionFind uf)
    {
        PriorityQueue<Edge, double> allEdges = new PriorityQueue<Edge, double>();
        foreach (HNode node in nodes)
        {
            foreach (KeyValuePair<int,double> keyValuePair in node.MutualReachability)
            {
                int otherId = keyValuePair.Key;
                double weight = keyValuePair.Value;
                allEdges.Enqueue(new Edge(node.Id, otherId), weight);
            }

            node.MutualReachability = null;
        }

        (Edge, double)[] mst = new (Edge, double)[nodes.Length - 1];
        for (int i = 0; i < mst.Length;)
        {
            allEdges.TryDequeue(out Edge nextEdge, out double weight);
            if (uf.Connected(nextEdge.From, nextEdge.To))
            {
                continue;
            }

            uf.Union(nextEdge.From, nextEdge.To);
            mst[i] = (nextEdge, weight);
            i++;
        }

        return mst;
    }
}
