using System.Diagnostics;
using Core;
using sHDBSCAN;
using Exporter = sHDBSCAN.Exporter;
using Node = Core.Node;

const int numberOfThreads = 8;

// --- PARAMETERS --- //
int D = 256; //amount of random vectors 
int l = 2; //amount of random vectors each datapoint knows
int m = 50; //amount of datapoints each random vector knows
int k = 4; //k is the amount of points for creating core distance

if (args.Length != 3)
{
    throw new Exception("Must give 3 arguments (path to data, path to dendrogram output, path to stats output)");
}

Dictionary<int, HNode> dataPoints = Importer.ImportNodes(args[0]);

// --- Normalise data --- //
foreach (HNode node in dataPoints.Values)
{
    node.Normalise();
}

// --- Stopwatch init --- //
Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();
TimeSpan lastTime = stopWatch.Elapsed;

// --- Generate random vectors --- // 
Console.WriteLine("Generating random vectors");
List<Node> randomVectors = Preprocessing.GenerateRandomVectors(D, dataPoints[0].Vector.Length);

foreach (Node node in randomVectors)
{
    node.Normalise();
}
PrintLap("Random vectors");

// --- Preprocessing --- // 
Console.WriteLine("Preprocessing");
Preprocessing.Preprocess(dataPoints.Values.Cast<Node>().ToList(), randomVectors, l, m);
PrintLap("Preprocessing");

// --- Set visible nodes --- // 
Console.WriteLine("Set visible nodes");
foreach (HNode n in dataPoints.Values)
{
    n.SetVisibleNodes();
}
PrintLap("Set visible nodes");

// --- Set core distance --- //
Console.WriteLine("Set core dist");

List<Task> threads = new ();

HNode[] allNodes = dataPoints.Values.ToArray();

for (int i = 0; i < numberOfThreads; i++)
{
    var from = allNodes.Length * i / numberOfThreads ;
    var to = allNodes.Length * (i + 1) / numberOfThreads ;
    Task t = Task.Run(() =>
    {
        for (int j = from; j < to; j++)
        {
            allNodes[j].SetCoreDist(k);
        }
    }); 
    threads.Add(t);
}

Task.WaitAll(threads);

PrintLap("Set Core Dist");

// --- Set mutual reachability --- //
Console.WriteLine("Set mutual reachability");
foreach (HNode n in dataPoints.Values)
{
    n.SetMutualReachability();
}
PrintLap("Set Mutual reachability");

// --- Create MST --- //
Console.WriteLine("CreateMST");
var mst = MST.CreateSpanningTree(dataPoints[0]);

Console.WriteLine("MST size: " + mst.Count);
PrintLap("MST");

// --- Cluster tree --- //
Console.WriteLine("Cluster tree");
UnionFind uf = new UnionFind(dataPoints.Count);
var dendrogram = Dendrogram.CreateDendrogram(mst, uf);

PrintLap("Cluster tree");

stopWatch.Stop();
Exporter.ExportDendrogram(args[1],dendrogram);

// --- Timer cleanup --- //
TimeSpan ts = stopWatch.Elapsed;
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);


Exporter.ExportsHdbscanStats(args[2], D, l, m, k, elapsedTime );


Console.WriteLine($"sHDBSCAN ran on dataset of size {dataPoints.Count} with D {D}, l {l}, m {m}, k {k}");
Console.WriteLine("Total time " + elapsedTime);

// Prints the time since last lap and the name input
void PrintLap(string lapName)
{
    var lap = stopWatch.Elapsed - lastTime;
    Console.WriteLine(lapName + " done: " + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", lap.Hours, lap.Minutes, lap.Seconds, lap.Milliseconds));
    lastTime = stopWatch.Elapsed;
}