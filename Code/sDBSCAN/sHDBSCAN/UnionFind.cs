using FSharpx.Collections;

namespace sHDBSCAN;
/// <summary>
/// inspired by Algorithms fourth edition by Sedgewick and Wayne page 228.
/// </summary>
public class UnionFind
{
    private readonly int n;
    private int[] _id;
    private int[] _size; 
    public int Count { get; private set; } //number of components 
    private int _nextParentId; 
    public UnionFind(int n)
    {
        this.n = n;
        _nextParentId = n; 
        Count = n;
        _id = new int[2 * n - 1];
        _size = new int[_id.Length];
        for (int i = 0; i < _id.Length; i++)
        {
            _size[i] = 1; 
            _id[i] = i; 
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
        _size[_nextParentId] = _size[i] + _size[j];
        _nextParentId++;
        Count--;
        return [i, j, _size[_nextParentId-1]];
    }

    public int[][] getcomponents()
    {
        Dictionary<int, HashSet<int>> components = new();
        for (int i = 0; i < n; i++)
        {
            int root = Find(i);
            if (!components.ContainsKey(root))
            {
                components.Add(root, new HashSet<int>());
            }
            components[root].Add(i);
        }

        return components.Select(x => x.Value.ToArray()).ToArray();
    }
}