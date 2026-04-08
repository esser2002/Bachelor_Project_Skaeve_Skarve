using System.Diagnostics;

namespace OOPsDBSCAN;

public static class FindComponents
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
    
    public static List<HashSet<Node>> GetClusters(List<Node> CorePoints)
    {
        List<HashSet<Node>> clusters = new();
        HashSet<Node> looseCorePoints = new HashSet<Node>(CorePoints);

        int clusterId = 0;
        
        while (looseCorePoints.Count > 0)
        {
            Node q = looseCorePoints.First();
            
            List<Node> openList = new List<Node>();
            HashSet<Node> closedList = new HashSet<Node>();
            
            openList.Add(q);
            closedList.Add(q);
            
            HashSet<Node> cluster = new HashSet<Node>();
            while (openList.Count > 0)
            {
                Node node = openList[0];
                openList.RemoveAt(0);
                cluster.Add(node);
                node.ClusterId = clusterId;
                if (node.CorePoint)
                {
                    looseCorePoints.Remove(node);
                    foreach (Node edge in node.Edges)
                    {
                        if (!closedList.Contains(edge))
                        {
                            openList.Add(edge);
                            closedList.Add(edge);
                        }
                    }
                }
            }
            clusters.Add(cluster);
            clusterId++;
        }

        return clusters;
    }
}
