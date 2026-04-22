namespace sHDBSCAN;

public static class Dendrogram
{
    //https://docs.scipy.org/doc/scipy/reference/generated/scipy.cluster.hierarchy.cophenet.html
    //type shit
    
    public static (int l, int r, double dist, int size)[] CreateDendrogram (PriorityQueue<Edge, double> mst, int size)
    {
        var dendrogram = new (int l, int r, double dist, int size)[mst.Count];
        Dictionary<int, int> ambassadors = new();
        UnionFind uf = new(size);
        for (int i = 0; mst.TryDequeue(out Edge edge, out double dist); i++)
        {
            int fromId = edge.From.Id;
            int toId = edge.To.Id;
    
            if(uf.Connected(fromId, toId)) {Console.WriteLine("Something wrong, edges are already connected");}
            var union = uf.Union(edge.From.Id, edge.To.Id);
            int from = ambassadors.GetValueOrDefault(union[0], union[0]);
            int to = ambassadors.GetValueOrDefault(union[1], union[1]);
            ambassadors[union[3]] = i+size;
            dendrogram[i] = (from, to, dist,union[2]);
        }

        return dendrogram;
    }

}