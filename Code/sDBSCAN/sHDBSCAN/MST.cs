
using OOPsDBSCAN;

namespace sHDBSCAN;

public class MST
{
    private HashSet<HNode> visited = new HashSet<HNode>();
    private PriorityQueue<Edge, double> edges = new ();
    private PriorityQueue<Edge, double> graph = new ();
    
    public void CreateSpanningTree(HNode initialNode)
    {
        edges.Enqueue(new Edge(initialNode, initialNode), 0);


        while (edges.Count > 0)
        {
            edges.TryDequeue(out Edge edge, out double weight);
            HNode toNode = edge.To; 
            
            if (visited.Contains(toNode))
            {
                continue;
            }
            graph.Enqueue(edge, -weight);
            visited.Add(toNode);

            foreach (var item in toNode.mutualReachability)
            {
                edges.Enqueue(new Edge(toNode, item.Key), -item.Value);
            }
        }
    }
}
