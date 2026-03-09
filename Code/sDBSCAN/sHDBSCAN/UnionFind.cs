namespace sHDBSCAN;
/// <summary>
/// inspired by Algorithms fourth edition by Sedgewick and Wayne page 228.
/// </summary>
public class UnionFind
{
    private int[] id;
    private int[] size;
    public int count { get; private set; }//number of components 

    public UnionFind(int N)
    {
        count = N;
        id = new int[N];
        for (int i = 0; i < N; i++)
        {
            id[i] = i;
        }
        size = new int[N];
        for (int i = 0; i < N; i++)
        {
            size[i] = 1;
        }
        
    }

    public bool connected(int p, int q)
    {
        return find (p) == find (q); 
    }

    private int find(int p)
    {
        int root = p; 
        while (root != id[root])
        {
            root = id[root];
        }

        while (root != p) //path compression
        {
            int newp = id[p];
            id[p] = root;
            p = newp;
        }
        return root;
    }

    public void union(int p, int q)
    {
        int i = find(p);
        int j = find(q);
        if (i == j) return;
        
        if (size[i] < size[j])
        {
            id[i] = j;
            size[j] += size[i];
        }
        else
        {
            id[j] = i;
            size[i] += size[j];
        }

        count--;
    }
}