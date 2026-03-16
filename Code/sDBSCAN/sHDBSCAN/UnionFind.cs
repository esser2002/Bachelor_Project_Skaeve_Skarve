namespace sHDBSCAN;
/// <summary>
/// inspired by Algorithms fourth edition by Sedgewick and Wayne page 228.
/// </summary>
public class UnionFind
{
    private int[] _id;
    public int Count { get; private set; } //number of components 
    private int _nextParentId; 
    public UnionFind(int n){
        _nextParentId = n + 1; 
        Count = n;
        _id = new int[2 * n - 1];
        for (int i = 0; i < _id.Length; i++)
        {
            _id[i] = i; // check if still necessary at the end
        }
    }

    public bool Connected(int p, int q)
    {
        return Find (p) == Find (q); 
    }

    private int Find(int p)
    {
        while (p != _id[p])
        {
            p = _id[p];
        }
        return p;
    }

    public int[] Union(int p, int q)
    {
        int i = Find(p);
        int j = Find(q);
        if (i == j) return [];

        _id[i] = _nextParentId;
        _id[j] = _nextParentId;
        _nextParentId++;
        Count--;
        return [i, j, _nextParentId - 1];
    }
}