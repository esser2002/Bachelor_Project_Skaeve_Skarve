using OOPsDBSCAN;
using sHDBSCAN;

namespace TestProject1;

public class sHDBSCAN
{
    private void prepareData(List<HNode> nodes, List<Node> randomNodes, int l, int m)
    {
        foreach (Node node in nodes)
        {
            node.Normalise();
        }
        
        foreach (Node node in randomNodes)
        {
            node.Normalise();
        }
 
        Preprocessing.Preprocess(nodes.Cast<Node>().ToList(), randomNodes, l, m);
        
    }
    [Test]
    public void CoreDistance()
    {
        int k = 3;
        int l = 1; 
        int m = 2;
        
        List<HNode> nodes = 
        [
            new HNode(2){Label = 0,Vector = [1,1]},
            new HNode(2){Label = 1,Vector = [1,-2]},
            new HNode(2){Label = 2,Vector = [1,0.5]},
            new HNode(2){Label = 3,Vector = [0,1]} // node 1 should have distance to node 4 as core distance
        ];

        List<Node> randomNodes =
        [
            new Node(2){Vector = [1,0]},
        ];
        
        prepareData(nodes, randomNodes, l, m);
        
        foreach (HNode n in nodes)
        {
            n.setCoreDist(k);
        }
        
        double expecteddistance = nodes[0].Dist(nodes[3]);
        Assert.That(nodes[0].CoreDist, Is.EqualTo(expecteddistance));
       
    }
    
    [Test]
    public void CoreDistance_MultipleRandomNodes()
    {
        int k = 3;
        int l = 1;
        int m = 4;
        
        List<HNode> nodes = 
        [
            new HNode(2){Label = 0,Vector = [1,1]},
            new HNode(2){Label = 1,Vector = [1,-2]},
            new HNode(2){Label = 2,Vector = [1,0.5]},
            new HNode(2){Label = 3,Vector = [0,1]} // node 1 should have distance to node 4 as core distance
        ];

        List<Node> randomNodes =
        [
            new Node(2){Vector = [1,0]},
            new Node(2){Vector = [0,1]},
            new Node(2){Vector = [-0.5,0.5]},
        ];
        
        prepareData(nodes, randomNodes, l, m);
        
        foreach (HNode n in nodes)
        {
            n.setCoreDist(k);
        }
        
        double expecteddistance = nodes[0].Dist(nodes[3]);
        Assert.That(nodes[0].CoreDist, Is.EqualTo(expecteddistance));
       
    }
    
    /// <summary>
    /// Checks the case where the mutual reachability is equal to the distance between two points
    /// </summary>
    [Test]
    public void MutualReachabilityDistAB() 
    {
        int k = 2;
        int l = 1;
        int m = 3;
        
        List<HNode> nodes = 
        [
            new HNode(2){Label = 0,Vector = [1,1]},
            new HNode(2){Label = 1,Vector = [0,1]},
            new HNode(2){Label = 2,Vector = [-1,-1]},
        ];

        List<Node> randomNodes =
        [
            new Node(2){Vector = [1,1]},
        ];
        
        prepareData(nodes, randomNodes, l, m);
        
        foreach (HNode node in nodes)
        {
            node.setCoreDist(k);
        }

        foreach (HNode node in nodes)
        {
            node.SetMutualReachability();
        }

        foreach (HNode node in nodes)
        {
            Console.WriteLine($"Mutual reach node 0 to {node.Label} is {nodes[0].GetReachability(node)}");
        }
        
        Assert.That(nodes[0].GetReachability(nodes[2]), Is.EqualTo(nodes[0].Dist(nodes[2])));
    }
    
    /// <summary>
    /// Checks the case where the mutual reachability of two points is equal to the core distance of the first point
    /// </summary>
    [Test]
    public void MutualReachabilityCoreA()
    {
        int k = 3;
        int l = 1;
        int m = 3;
        
        List<HNode> nodes = 
        [
            new HNode(2){Label = 0,Vector = [1,1]},
            new HNode(2){Label = 1,Vector = [1,0]},
            new HNode(2){Label = 2,Vector = [1,0.5]},
        ];

        List<Node> randomNodes =
        [
            new Node(2){Vector = [1,1]},
        ];
        
        prepareData(nodes, randomNodes, l, m);
        
        foreach (HNode node in nodes)
        {
            node.setCoreDist(k);
        }

        foreach (HNode node in nodes)
        {
            node.SetMutualReachability();
        }

        foreach (HNode node in nodes)
        {
            Console.WriteLine($"Mutual reach node 0 to {node.Label} is {nodes[0].GetReachability(node)}");
        }
        
        Assert.That(nodes[0].GetReachability(nodes[2]), Is.EqualTo(nodes[0].CoreDist));
    }
    
    /// <summary>
    /// Checks the case where the mutual reachability of two points is equal to the core distance of the second point
    /// </summary>
    [Test]
    public void MutualReachabilityCoreB()
    {
        int k = 2;
        int l = 1;
        int m = 3;
        
        List<HNode> nodes = 
        [
            new HNode(2){Label = 0,Vector = [1,1]},
            new HNode(2){Label = 1,Vector = [1,0]},
            new HNode(2){Label = 2,Vector = [-1,0]},
        ];

        List<Node> randomNodes =
        [
            new Node(2){Vector = [1,1]},
        ];
        
        prepareData(nodes, randomNodes, l, m);
        
        foreach (HNode node in nodes)
        {
            node.setCoreDist(k);
        }

        foreach (HNode node in nodes)
        {
            node.SetMutualReachability();
        }

        foreach (HNode node in nodes)
        {
            Console.WriteLine($"Mutual reach node 0 to {node.Label} is {nodes[0].GetReachability(node)}");
        }
        
        Assert.That(nodes[0].GetReachability(nodes[2]), Is.EqualTo(nodes[2].CoreDist));
    }

    [Test]
    public void CreateMST()
    {
        int k = 2;
        int l = 1;
        int m = 4;
        
        List<HNode> nodes = 
        [
            new HNode(2){Label = 0,Vector = [0.4,1]},
            new HNode(2){Label = 1,Vector = [0.8,1]},
            new HNode(2){Label = 2,Vector = [1,0.6]},
            new HNode(2){Label = 3,Vector = [1,0]},
        ];

        List<Node> randomNodes =
        [
            new Node(2){Vector = [1,1]},
        ];
        
        prepareData(nodes, randomNodes, l, m);
        
        foreach (HNode node in nodes)
        {
            node.setCoreDist(k);
        }

        foreach (HNode node in nodes)
        {
            node.SetMutualReachability();
        }

        var mst = MST.CreateSpanningTree(nodes[3]).UnorderedItems.Select(x => x.Element);
        
        Assert.That(mst.Count, Is.EqualTo(3));

        Assert.That(hasEdge(mst, 0, 1));
        Assert.That(hasEdge(mst, 1, 2));
        Assert.That(hasEdge(mst, 2, 3));
    }

    private bool hasEdge(IEnumerable<Edge> graph, int from, int to)
    {
        return (graph.Select(x => x.From.Label == from && x.To.Label == to) != null
                || graph.Select(x => x.To.Label == from && x.From.Label == to) != null);
    }
}