using System.Runtime.InteropServices.Swift;
using FSharpx.Collections.Tagged;
using OOPsDBSCAN;

namespace sHDBSCAN;

public class HNode : Node
{
    public Dictionary<HNode, double> mutualReachability;
    public double CoreDist { get; private set; }
    public HNode(string[] input) : base(input)
    {
    }

    public HNode(int dimensions) : base(dimensions)
    {
    }

    public void setCoreDist(int k)
    {
        PriorityQueue<Node, double> kclosest = new PriorityQueue<Node, double>(); 
        addKClosestPoints(kclosest, getVisibleNodes() , k);

        CoreDist = kclosest.Dequeue().Dist(this);
    }

    private void addKClosestPoints(PriorityQueue<Node, double> queue, IEnumerable<Node> nodes, int k)
    {
        foreach (Node n in nodes)
        {
            double dist = Dist(n);
            if (queue.UnorderedItems.Select(s => s.Element).Contains(n))
            {
                continue;
            }
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

    private HashSet<HNode> getVisibleNodes()
    {
        HashSet<Node> visibleNodes = new();
        foreach (Node nearRandomNode in Nearest!)
        {
            foreach (Node nearNearNode in nearRandomNode.Nearest!)
            {
                visibleNodes.Add(nearNearNode);
            }
        }
        foreach (Node farRandomNode in Furthest!)
        {
            foreach (Node farFarNode in farRandomNode.Furthest!)
            {
                visibleNodes.Add(farFarNode);
            }
        }

        return visibleNodes.Cast<HNode>().ToHashSet();
    }

    public void SetMutualReachability()
    {
        mutualReachability = new();

        foreach (HNode node in getVisibleNodes())
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