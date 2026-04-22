namespace sHDBSCAN;
/// <summary>
/// Inspired by Algorithms fourth edition by Sedgewick and Wayne page 228.
/// </summary>
public class UnionFind
{
    private int[] _id;
    private int[] _size;
    public int Count { get; private set; }//number of components 

    public UnionFind(int N)
    {
        Count = N;
        _id = new int[N];
        for (int i = 0; i < N; i++)
        {
            _id[i] = i;
        }
        _size = new int[N];
        for (int i = 0; i < N; i++)
        {
            _size[i] = 1;
        }
    }

    public bool Connected(int p, int q)
    {
        return Find (p) == Find (q); 
    }

    private int Find(int p)
    {
        int root = p; 
        while (root != _id[root])
        {
            root = _id[root];
        }

        while (root != p) //path compression
        {
            int newp = _id[p];
            _id[p] = root;
            p = newp;
        }
        return root;
    }

    public int[] Union(int p, int q)
    {
        int i = Find(p);
        int j = Find(q);
        if (i == j) return [];

        int newsize;
        int leader;
        if (_size[i] < _size[j])
        {
            _id[i] = j;
            _size[j] += _size[i];
            newsize = _size[j];
            leader = j;
        }
        else
        {
            _id[j] = i;
            _size[i] += _size[j];
            newsize = _size[i];
            leader = i;
        }

        Count--;
        return [i, j, newsize, leader];
    }
}