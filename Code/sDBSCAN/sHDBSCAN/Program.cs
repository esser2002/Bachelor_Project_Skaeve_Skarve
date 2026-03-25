using Microsoft.VisualBasic.FileIO;
using OOPsDBSCAN;
using sHDBSCAN;
using System.Diagnostics;
using Exporter = sHDBSCAN.Exporter;

if (args.Length < 2)
{
    throw new Exception("Must give 2 arguments (path to data, path to output)")
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

//Exporter.ExportNormalisedData(args[1], dataPoints);

int D = 1000; // amount of random vectors 
int k = 30; //k is the amount off points for creating core distance
int m = 100; //amount of datapoints each random vector knows
int l = 2; //amount of random vectors each datapoint knows

// start timer
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
int i = 0;
while (MST.TryDequeue(out Edge edge, out double dist))
{
    int fromId = edge.From.id;
    int toId = edge.To.id;
    
    if(uf.Connected(fromId, toId)) {Console.WriteLine("Something wrong, edges are already connected");}
    var union = uf.Union(edge.From.id, edge.To.id);
    dendrogram[i] = (union[0], union[1], dist,union[2]);
    i++;
}

stopWatch.Stop();
Exporter.ExportDendrogram(args[1],dendrogram);

//Stopwatch
TimeSpan ts = stopWatch.Elapsed;
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

Exporter.ExportsHdbscanStats(args[2], D, k, l, m, elapsedTime );

Console.WriteLine(uf.Count);
Console.WriteLine("done");
