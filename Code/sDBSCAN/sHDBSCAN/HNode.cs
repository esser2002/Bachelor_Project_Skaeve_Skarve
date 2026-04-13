using Node = Core.Node;

namespace sHDBSCAN;

public class HNode : Node
{
    private static int nextId = 0;
    public int id = nextId++;
    public Dictionary<HNode, double> mutualReachability;
    public double CoreDist { get; private set; }

    private HashSet<HNode> visibleNodes = new();
    
    public HNode(string[] input) : base(input)
    {
    }

    public HNode(int dimensions) : base(dimensions)
    {
    }

    public void setCoreDist(int k)
    {
        PriorityQueue<Node, double> kclosest = new PriorityQueue<Node, double>(); 
        addKClosestPoints(kclosest, visibleNodes, k);

        CoreDist = kclosest.Dequeue().Dist(this);
    }

    private void addKClosestPoints(PriorityQueue<Node, double> queue, IEnumerable<Node> nodes, int k)
    {
        HashSet<Node> touchedNodes = new HashSet<Node>();
        foreach (Node n in nodes)
        {
            double dist = Dist(n);
            if (touchedNodes.Contains(n)) { continue; } else { touchedNodes.Add(n);}
            
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
                visibleNodes.Add(nearNearNode);
                nearNearNode.visibleNodes.Add(this);
            }
        }
        foreach (Node farRandomNode in Furthest!)
        {
            foreach (var node in farRandomNode.Furthest!)
            {
                var farFarNode = (HNode)node;
                visibleNodes.Add(farFarNode);
                farFarNode.visibleNodes.Add(this);
            }
        }
    }

    public void SetMutualReachability()
    {
        mutualReachability = new();

        foreach (HNode node in visibleNodes)
        {
            double dist = new List<double> { CoreDist, node.CoreDist, Dist(node)}.Max();
            mutualReachability.Add(node, dist);
        }
    }

    public double GetReachability(HNode node)
    {
        return mutualReachability[node];
    }
}