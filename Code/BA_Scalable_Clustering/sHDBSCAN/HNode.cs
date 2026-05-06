using Node = Core.Node;
using sHDBSCAN;
namespace sHDBSCAN;

public class HNode : Node
{
    private static int _nextId;
    public int Id = _nextId++;
    public double CoreDist { get; private set; }

    public HashSet<int> VisibleNodes = new();
    
    public HNode(string[] input) : base(input)
    {
    }

    public HNode(int dimensions) : base(dimensions)
    {
    }
    
    /// <summary>
    /// Set the distance to the kth nearest neighbour
    /// </summary>
    /// <param name="k"></param>
    public void SetCoreDist(int k)
    {
        PriorityQueue<int, double> kclosest = new PriorityQueue<int, double>(); 
        AddKClosestPoints(kclosest, VisibleNodes, k);
        CoreDist = Dist(Util.dataPoints[kclosest.Dequeue()]);
    }
    /// <summary>
    /// Find the k closest nodes.
    /// </summary>
    /// <param name="queue">Empty priority queue</param>
    /// <param name="nodes">The set of visible nodes</param>
    /// <param name="k">Amount of nodes to add to the queue</param>
    private void AddKClosestPoints(PriorityQueue<int, double> queue, IEnumerable<int> nodes, int k)
    {
        HashSet<Node> touchedNodes = new HashSet<Node>();
        foreach (int nId in nodes)
        {
            HNode n = Util.dataPoints[nId];
            double dist = Dist(n);
            if (touchedNodes.Contains(n)) { continue; }

            touchedNodes.Add(n);

            if (queue.Count < k)
            {
                queue.Enqueue(n.Id,-dist);
            }
            else
            {
                queue.TryPeek(out _, out double otherDist); //check the distance of the furthest point in queue
                if (otherDist < -dist)
                {
                    queue.Dequeue();
                    queue.Enqueue(n.Id,-dist);
                }
            }
        }
    }
    
    /// <summary>
    /// For each node store the visible nodes. The visible nodes in this context are the nearest nodes
    /// to the nearest random vector and the furthest nodes from the furthest random vector
    /// </summary>
    public void SetVisibleNodes()
    {
        foreach (Node nearRandomNode in Nearest!)
        {
            foreach (var node in nearRandomNode.Nearest!)
            {
                var nearNearNode = (HNode)node;
                VisibleNodes.Add(nearNearNode.Id);
                nearNearNode.VisibleNodes.Add(Id);
            }
        }
        foreach (Node farRandomNode in Furthest!)
        {
            foreach (var node in farRandomNode.Furthest!)
            {
                var farFarNode = (HNode)node;
                VisibleNodes.Add(farFarNode.Id);
                farFarNode.VisibleNodes.Add(Id);
            }
        }
    }

    public double GetReachability(HNode node)
    {
        double dist = new List<double> { CoreDist, node.CoreDist, Dist(node)}.Max();
        return dist;
    }
}