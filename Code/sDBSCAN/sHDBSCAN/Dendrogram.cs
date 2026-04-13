namespace sHDBSCAN;

public static class Dendrogram
{

    public static (int l, int r, double dist, int size)[] CreateDendrogram (PriorityQueue<Edge, double> mst, UnionFind uf)
    {
        var dendrogram = new (int l, int r, double dist, int size)[mst.Count];
        for (int i = 0; mst.TryDequeue(out Edge edge, out double dist); i++)
        {
            int fromId = edge.From.Id;
            int toId = edge.To.Id;
    
            if(uf.Connected(fromId, toId)) {Console.WriteLine("Something wrong, edges are already connected");}
            var union = uf.Union(edge.From.Id, edge.To.Id);
            dendrogram[i] = (union[0], union[1], dist,union[2]);
        }

        return dendrogram;
    }

}