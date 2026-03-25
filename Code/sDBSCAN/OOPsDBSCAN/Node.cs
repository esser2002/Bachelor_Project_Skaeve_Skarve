namespace OOPsDBSCAN;

public class Node : Core.Node
{
    public int ClusterId = -1;
    
    public Node(string[] input) : base(input)
    {
    }

    public Node(int dimensions) : base(dimensions)
    {
    }
}