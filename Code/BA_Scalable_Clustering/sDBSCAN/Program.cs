using System.Diagnostics;
using System.Globalization;
using Core;
using sDBSCAN;
using Node = sDBSCAN.Node;


if (args.Length != 8)
{
    throw new Exception("Must give 8 arguments (path to data, path to cluster output, path to stats output, D, k, m, epsilon, minPts)");
}

// --- PARAMETERS --- //
int D = Int32.Parse(args[3]); //amount of random vectors 
int k = Int32.Parse(args[4]); //amount of random vectors each datapoint knows
int m = Int32.Parse(args[5]); //amount of datapoints each random vector knows
double epsilon = double.Parse(args[6], CultureInfo.InvariantCulture); //distance requirement for nodes to be neighbors
int minPts = Int32.Parse(args[7]); //minimum neighborhood size to be core point


// --- Load Data --- //
List<Node> dataPoints = Importer.ImportNodes(args[0]);

// --- Normalise data --- //
foreach (Node node in dataPoints)
{
    node.Normalise();
}

// --- Stopwatch init --- //
Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();
TimeSpan lastTime = stopWatch.Elapsed;

// --- Generate random vectors --- //
Console.WriteLine("Generating random vectors");
List<Core.Node> randomVectors = Preprocessing.GenerateRandomVectors(D, dataPoints[0].Vector.Length);

foreach (Core.Node node in randomVectors)
{
    node.Normalise();
}
PrintLap("Random vectors");

// --- Preprocessing --- //
Console.WriteLine("Preprocessing");
Preprocessing.Preprocess(dataPoints.ToArray(), randomVectors.ToArray(), k, m);
PrintLap("Preprocessing");

// --- Find core points --- //
Console.WriteLine("Finding core points");
var neighborhoods = FindCorePoints.FindCorePointsAndNeighbors(dataPoints, epsilon, minPts);
PrintLap("Finding core points");

// --- Do DBSCAN --- //
Console.WriteLine("DBSCAN initiated");
FindComponents.DoDBSCAN(neighborhoods);
PrintLap("Do DBSCAN");

// --- Get results --- //
Console.WriteLine($"sDBSCAN with D {D}, k {k}, m {m}, epsilon {epsilon}, minPts {minPts}");

List<HashSet<Node>> clusters = FindComponents.GetClusters(neighborhoods.Keys.ToList());
stopWatch.Stop();
Console.WriteLine(Metrics.GetMetrics(clusters, dataPoints));
// --- Timer cleanup --- //
TimeSpan ts = stopWatch.Elapsed;
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);


Console.WriteLine("Exporting clusters");
Exporter.ExportClusters(args[1], dataPoints);
Exporter.ExportsDBSCANStats(args[2], D, k, m, epsilon, minPts, dataPoints.Count, elapsedTime);

// Prints the time since last lap and the name input
void PrintLap(string lapName)
{
    var lap = stopWatch.Elapsed - lastTime;
    Console.WriteLine(lapName + " done: " + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", lap.Hours, lap.Minutes, lap.Seconds, lap.Milliseconds));
    lastTime = stopWatch.Elapsed;
}
