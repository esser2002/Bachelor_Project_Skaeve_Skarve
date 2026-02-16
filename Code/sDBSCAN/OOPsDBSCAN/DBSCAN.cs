using FSharpx.Collections.Tagged;

namespace OOPsDBSCAN;

public static class DBSCAN
{
    public static void DoDBSCAN(Dictionary<Node, HashSet<Node>> C)
    {
        foreach (KeyValuePair<Node,HashSet<Node>> keyValuePair in C)
        {
            Node q = keyValuePair.Key;
            q.Connected = true;
            
            foreach (Node x in keyValuePair.Value)
            {
                if (x.CorePoint)
                {
                    q.Edges.Add(x);
                }
                else if (!x.Connected)
                {
                    x.Connected = true;
                    q.Edges.Add(x);
                }
            }
        }
    }

}
