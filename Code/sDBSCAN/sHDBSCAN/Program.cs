using Microsoft.VisualBasic.FileIO;
using OOPsDBSCAN;
using sHDBSCAN;

if (args.Length == 0)
{
    throw new Exception("Must give 1 argument (path to data)")
        ;}

var path = args[0];
using TextFieldParser csvParser = new TextFieldParser(path);

csvParser.SetDelimiters(",");
csvParser.HasFieldsEnclosedInQuotes = true;

// Skip the row with the column names
csvParser.ReadLine();

List<HNode> dataPoints = [];

while (!csvParser.EndOfData)
{
    // Read current line fields, pointer moves to the next line.
    string[] fields = csvParser.ReadFields() ?? throw new InvalidOperationException();
    dataPoints.Add(new HNode(fields));
}

//Normalise data
foreach (Node node in dataPoints)
{
    node.Normalise();
}

int D = 128; // amount of random vectors 
int k = 2; //k is the amount off points for creating core distance
int m = 300; //amount of datapoints each random vector knows
int l = 2; //amount of random vectors each datapoint knows

List<Node> randomVectors = Preprocessing.GenerateRandomVectors(D, dataPoints[0].Vector.Length);

foreach (Node node in randomVectors)
{
    node.Normalise();
}

Console.WriteLine("Preprocessing");
Preprocessing.Preprocess(dataPoints.Cast<Node>().ToList(), randomVectors, l, m);

Console.WriteLine("Set visible nodes");
foreach (HNode n in dataPoints)
{
    n.SetVisibleNodes();
}

Console.WriteLine("Set core dist");
foreach (HNode n in dataPoints)
{
     n.setCoreDist(k);
}

Console.WriteLine("Set mutual reachability");
foreach (HNode n in dataPoints)
{
    n.SetMutualReachability();
}


Console.WriteLine("CreateMST");
var MST = sHDBSCAN.MST.CreateSpanningTree(dataPoints[0]);
Console.WriteLine("MST size: " + MST.Count);

Console.WriteLine("Cluster tree");
UnionFind uf = new UnionFind(dataPoints.Count);
while (MST.TryDequeue(out Edge edge, out _))
{
    int fromId = edge.From.id;
    int toId = edge.To.id;
    
    if(uf.connected(fromId, toId)) {Console.WriteLine("Something wrong, edges are already connected");}
    uf.union(edge.From.id, edge.To.id);
}

Console.WriteLine(uf.count);
