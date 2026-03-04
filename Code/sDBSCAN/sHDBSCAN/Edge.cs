using System.Security.Cryptography;

namespace sHDBSCAN;

public class Edge
{
    public HNode From;
    public HNode To;

    public Edge(HNode from, HNode to)
    {
        this.From = from;
        this.To = to;
    }
}