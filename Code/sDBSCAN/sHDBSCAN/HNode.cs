using System.Runtime.InteropServices.Swift;
using FSharpx.Collections.Tagged;
using OOPsDBSCAN;

namespace sHDBSCAN;

public class HNode : Node
{
    private Dictionary<Node, double> mutualReachability;
    public double CoreDist; 
    public HNode(string[] input) : base(input)
    {
    }

    public HNode(int dimensions) : base(dimensions)
    {
    }

    public void setCoreDist(int k)
    {
        PriorityQueue<Node, double> kclosest = new PriorityQueue<Node, double>(); 
        addKClosestPoints(kclosest, true , k);
        addKClosestPoints(kclosest, false , k);

        CoreDist = kclosest.Dequeue().Dist(this);
    }

    private void addKClosestPoints(PriorityQueue<Node, double> queue, bool closest, int k)
    {
        List<Node> nodes = new List<Node>(closest ? Nearest : Furthest);
        foreach (Node randomN in nodes)
        {
            List<Node> othernodes = new List<Node>(closest ? randomN.Nearest : randomN.Furthest);
            foreach (Node n in othernodes)
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
    }
}