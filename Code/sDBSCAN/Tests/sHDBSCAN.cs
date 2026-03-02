using OOPsDBSCAN;
using sHDBSCAN;

namespace TestProject1;

public class sHDBSCAN
{
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
        
        foreach (Node node in nodes)
        {
            node.Normalise();
        }
        
        foreach (Node node in randomNodes)
        {
            node.Normalise();
        }
 
        Preprocessing.Preprocess(nodes.Cast<Node>().ToList(), randomNodes, l, m);
        
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
        
        foreach (Node node in nodes)
        {
            node.Normalise();
        }
        
        foreach (Node node in randomNodes)
        {
            node.Normalise();
        }
 
        Preprocessing.Preprocess(nodes.Cast<Node>().ToList(), randomNodes, l, m);
        
        foreach (HNode n in nodes)
        {
            n.setCoreDist(k);
        }
        
        double expecteddistance = nodes[0].Dist(nodes[3]);
        Assert.That(nodes[0].CoreDist, Is.EqualTo(expecteddistance));
       
    }
}