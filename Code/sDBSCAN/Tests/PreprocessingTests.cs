using OOPsDBSCAN;

namespace TestProject1;


public class Tests
{
    [Test]
    public void furthestclosestpoints()
    {
        List<Node> nodes = 
        [
            new Node(2){Vector = [1,1]},
        ];

        List<Node> RandomNodes =
        [
            //Closest
            new Node(2){Label = 0, Vector = [-1,-1]},
            new Node(2){Label = 1,Vector = [6,7]},
            //Middle
            new Node(2){Label = 2,Vector = [2,1]},
            new Node(2){Label = 3,Vector = [3,4]},
            //Furthest
            new Node(2){Label = 4,Vector = [1,-1]},
            new Node(2){Label = 5,Vector = [6,-7]},
        ];

        foreach (Node node in nodes)
        {
            node.Normalise();
        }
        
        foreach (Node node in RandomNodes)
        {
            node.Normalise();
        }
        
        Preprocessing.Preprocess(nodes, RandomNodes, 2, 1);

        foreach (Node node in RandomNodes)
        {
            Console.WriteLine($"similarity to {node.Label}: " + nodes[0].AbsScalar(node));
        }
        
        Assert.That(nodes[0].Nearest, Contains.Item(RandomNodes[0]));
        Assert.That(nodes[0].Nearest, Contains.Item(RandomNodes[1]));
        
        Assert.That(nodes[0].Furthest, Contains.Item(RandomNodes[4]));
        Assert.That(nodes[0].Furthest, Contains.Item(RandomNodes[5]));
    }
    
    [Test]
    public void Makegoodclusters()
    {
        List<Node> nodes = 
        [
            new Node(2){Label = 0, Vector = [12,1]},
            new Node(2){Label = 0, Vector = [11,2]},
            new Node(2){Label = 0, Vector = [10,1]},
            
            new Node(2){Label = 1, Vector = [1,12]},
            new Node(2){Label = 1, Vector = [1,13]},
            new Node(2){Label = 1, Vector = [2,14]},
        ];

        List<Node> randomNodes =
        [
            //Closest
            new Node(2){Label = 0, Vector = [11,1]},
            new Node(2){Label = 1,Vector = [20,20]},
            new Node(2){Label = 2,Vector = [2,13]},
            new Node(2){Label = 3,Vector = [-10,-10]},
        ];

        foreach (Node node in nodes)
        {
            node.Normalise();
        }
        
        foreach (Node node in randomNodes)
        {
            node.Normalise();
        }
        
        Preprocessing.Preprocess(nodes, randomNodes, 2, 3);

        var neighborhoods = FindCorePoints.FindCorePointsAndNeighbors(nodes, 0.5, 2);
        
        DBSCAN.DoDBSCAN(neighborhoods);

        List<HashSet<Node>> clusters = DBSCAN.GetClusters(neighborhoods.Keys.ToList());
        Assert.That(clusters.Count(), Is.EqualTo(2));
        

        var cluster0 = clusters.First(c => c.First().Label == 0);
        Assert.That(cluster0.Count(), Is.EqualTo(3));

        Assert.That(cluster0, Contains.Item(nodes[0]));
        Assert.That(cluster0, Contains.Item(nodes[1]));
        Assert.That(cluster0, Contains.Item(nodes[2]));
        
        var cluster1 = clusters.First(c => c.First().Label == 1);
        Assert.That(cluster1.Count(), Is.EqualTo(3));

        Assert.That(cluster1, Contains.Item(nodes[3]));
        Assert.That(cluster1, Contains.Item(nodes[4]));
        Assert.That(cluster1, Contains.Item(nodes[5]));
    }
}