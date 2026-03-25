using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using Core;
using OOPsDBSCAN;
using sHDBSCAN;
using Exporter = sHDBSCAN.Exporter;
using Node = Core.Node;

if (args.Length < 2)
{
    throw new Exception("Must give 2 arguments (path to data, path to output)")
        ;}

Stopwatch w = new Stopwatch();
w.Start();

var path = args[0];
using TextFieldParser csvParser = new TextFieldParser(path);

csvParser.SetDelimiters(",");
csvParser.HasFieldsEnclosedInQuotes = true;

// Skip the row with the column names
csvParser.ReadLine();

List<HNode> dataPoints = [];
Dictionary<int, HNode> getNode = new();

while (!csvParser.EndOfData)
{
    // Read current line fields, pointer moves to the next line.
    string[] fields = csvParser.ReadFields() ?? throw new InvalidOperationException();
    HNode node = new HNode(fields);
    dataPoints.Add(node);
    getNode.Add(node.id, node);
}

//Normalise data
foreach (Node node in dataPoints)
{
    node.Normalise();
}

//Exporter.ExportNormalisedData(args[1], dataPoints);

int k = 4; //k is the amount off points for creating core distance
int D = 16; //amount of random vectors 
int l = 2; //amount of random vectors each datapoint knows
int m = 10; //amount of datapoints each random vector knows

Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();

List<Node> randomVectors = Preprocessing.GenerateRandomVectors(D, dataPoints[0].Vector.Length);

stopWatch.Stop();
foreach (Node node in randomVectors)
{
    node.Normalise();
}

stopWatch.Start();

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

//Exporter.ExportMST(args[1],MST);

Console.WriteLine("MST size: " + MST.Count);

Console.WriteLine("Cluster tree");
UnionFind uf = new UnionFind(dataPoints.Count);

var dendrogram = new (int l, int r, double dist, int size)[dataPoints.Count - 1];

for (int i = 0; MST.TryDequeue(out Edge edge, out double dist) && uf.Count > 9000; i++)
{
    int fromId = edge.From.id;
    int toId = edge.To.id;
    Console.WriteLine("union " + fromId + ", " + toId + ": " + dist + "(" + edge.From.CoreDist + ", " + edge.To.CoreDist + ", " + edge.From.Dist(edge.To) + ")");
    
    if(uf.Connected(fromId, toId)) {Console.WriteLine("Something wrong, edges are already connected");}
    var union = uf.Union(edge.From.id, edge.To.id);
    dendrogram[i] = (union[0], union[1], dist,union[2]);
}

stopWatch.Stop();
Exporter.ExportDendrogram(args[1],dendrogram);

//Stopwatch
TimeSpan ts = stopWatch.Elapsed;
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

Exporter.ExportsHdbscanStats(args[2], D, k, l, m, elapsedTime );

Console.WriteLine(uf.Count);
Console.WriteLine("done");
