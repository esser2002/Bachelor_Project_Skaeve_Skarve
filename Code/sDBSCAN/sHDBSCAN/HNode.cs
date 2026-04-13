using Node = Core.Node;

namespace sHDBSCAN;

public class HNode : Node
{
    private static int _nextId;
    public int Id = _nextId++;
    public Dictionary<HNode, double> MutualReachability = null!;
    public double CoreDist { get; private set; }

    private HashSet<HNode> _visibleNodes = new();
    
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
        PriorityQueue<Node, double> kclosest = new PriorityQueue<Node, double>(); 
        AddKClosestPoints(kclosest, _visibleNodes, k);

        CoreDist = kclosest.Dequeue().Dist(this);
    }
    /// <summary>
    /// Find the k closest nodes.
    /// </summary>
    /// <param name="queue">Empty priority queue</param>
    /// <param name="nodes">The set of visible nodes</param>
    /// <param name="k">Amount of nodes to add to the queue</param>
    private void AddKClosestPoints(PriorityQueue<Node, double> queue, IEnumerable<Node> nodes, int k)
    {
        HashSet<Node> touchedNodes = new HashSet<Node>();
        foreach (Node n in nodes)
        {
            double dist = Dist(n);
            if (touchedNodes.Contains(n)) { continue; }

            touchedNodes.Add(n);

            if (queue.Count < k)
            {
                queue.Enqueue(n,-dist);
            }
            else
            {
                queue.TryPeek(out _, out double otherDist); //check the distance of the furthest point in queue
                if (otherDist < -dist)
                {
                    queue.Dequeue();
                    queue.Enqueue(n,-dist);
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
                _visibleNodes.Add(nearNearNode);
                nearNearNode._visibleNodes.Add(this);
            }
        }
        foreach (Node farRandomNode in Furthest!)
        {
            foreach (var node in farRandomNode.Furthest!)
            {
                var farFarNode = (HNode)node;
                _visibleNodes.Add(farFarNode);
                farFarNode._visibleNodes.Add(this);
            }
        }
    }
    /// <summary>
    /// Set mutual reachability to all visible nodes
    /// </summary>
    public void SetMutualReachability()
    {
        MutualReachability = new();

        foreach (HNode node in _visibleNodes)
        {
            double dist = new List<double> { CoreDist, node.CoreDist, Dist(node)}.Max();
            MutualReachability.Add(node, dist);
        }
    }

    public double GetReachability(HNode node)
    {
        return MutualReachability[node];
    }
}