using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using Core;
using OOPsDBSCAN;
using sHDBSCAN;
using Exporter = sHDBSCAN.Exporter;
using Node = Core.Node;

// --- PARAMETERS --- //
int k = 4; //k is the amount of points for creating core distance
int D = 16; //amount of random vectors 
int l = 2; //amount of random vectors each datapoint knows
int m = 10; //amount of datapoints each random vector knows

Dictionary<int, HNode> dataPoints = Importer.ImportNodes(args[0]);

if (args.Length < 3)
{
    throw new Exception("Must give 3 arguments (path to data, path to output)");
}


// --- Normalise data --- //
foreach (HNode node in dataPoints.Values)
{
    node.Normalise();
}

Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();
TimeSpan lastTime = stopWatch.Elapsed;

Console.WriteLine("Generating random vectors");
List<Node> randomVectors = Preprocessing.GenerateRandomVectors(D, dataPoints[0].Vector.Length);

foreach (Node node in randomVectors)
{
    node.Normalise();
}
printLap("Random vectors");

Console.WriteLine("Preprocessing");
Preprocessing.Preprocess(dataPoints.Values.Cast<Node>().ToList(), randomVectors, l, m);
printLap("Preprocessing");

Console.WriteLine("Set visible nodes");
foreach (HNode n in dataPoints.Values)
{
    n.SetVisibleNodes();
}
printLap("Set visible nodes");

Console.WriteLine("Set core dist");
foreach (HNode n in dataPoints.Values)
{
     n.setCoreDist(k);
}
printLap("Set Core Dist");

Console.WriteLine("Set mutual reachability");
foreach (HNode n in dataPoints.Values)
{
    n.SetMutualReachability();
}
printLap("Set Mutual reachability");

Console.WriteLine("CreateMST");
var MST = sHDBSCAN.MST.CreateSpanningTree(dataPoints[0]);

Console.WriteLine("MST size: " + MST.Count);
printLap("MST");

Console.WriteLine("Cluster tree");
UnionFind uf = new UnionFind(dataPoints.Count);

var dendrogram = new (int l, int r, double dist, int size)[dataPoints.Count - 1];

for (int i = 0; MST.TryDequeue(out Edge edge, out double dist); i++)
{
    int fromId = edge.From.id;
    int toId = edge.To.id;
    
    if(uf.Connected(fromId, toId)) {Console.WriteLine("Something wrong, edges are already connected");}
    var union = uf.Union(edge.From.id, edge.To.id);
    dendrogram[i] = (union[0], union[1], dist,union[2]);
}
printLap("Cluster tree");

stopWatch.Stop();
Exporter.ExportDendrogram(args[1],dendrogram);

//Stopwatch
TimeSpan ts = stopWatch.Elapsed;
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

Exporter.ExportsHdbscanStats(args[2], D, k, l, m, elapsedTime );


Console.WriteLine($"SkarvHDBSCAN with  k {k}, D {D}, l {l}, m {m}");
Console.WriteLine("Total time " + elapsedTime);

/*
//Print the resulting clusters

var clusters = uf.getcomponents();

Console.WriteLine("number of clusters " + clusters.Length);
Console.WriteLine("coverage: " + (100*clusters.Sum(c => (c.Length > 5)?c.Length:0)/(double)dataPoints.Count) + " %");

foreach (int[] cluster in clusters)
{
    if (cluster.Length < 5)//OBS: this is cluster min size
        continue;
    int[] freq = new int[10];
    foreach (HNode node in cluster.Select(x => getNode[x]))
    {
        freq[node.Label] += 1;
    }
    
    Console.WriteLine("Cluster length " + cluster.Length + " contents:");
    for (int i = 0; i < freq.Length; i++)
    {
        Console.WriteLine(" - " + i + ": " + freq[i]);
    }
}

Console.WriteLine(uf.Count);
Console.WriteLine("done");
*/


void printLap(string lapName)
{
    var lap = stopWatch.Elapsed - lastTime;
    Console.WriteLine(lapName + " done: " + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", lap.Hours, lap.Minutes, lap.Seconds, lap.Milliseconds));
    lastTime = stopWatch.Elapsed;
}