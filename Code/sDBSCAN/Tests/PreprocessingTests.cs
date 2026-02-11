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
        
        Preprocessing.Preprocess(nodes, RandomNodes, 2, 0);

        foreach (Node node in RandomNodes)
        {
            Console.WriteLine($"similarity to {node.Label}: " + nodes[0].AbsScalar(node));
        }
        
        Assert.That(nodes[0].Nearest, Contains.Item(RandomNodes[0]));
        Assert.That(nodes[0].Nearest, Contains.Item(RandomNodes[1]));
    }
}