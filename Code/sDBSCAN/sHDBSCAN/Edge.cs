namespace sHDBSCAN;

public class Edge
{
    public HNode From;
    public HNode To;

    public Edge(HNode from, HNode to)
    {
        From = from;
        To = to;
    }
}