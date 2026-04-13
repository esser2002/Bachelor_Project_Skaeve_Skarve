using Core;
using sDBSCAN;
using Node = sDBSCAN.Node;

// --- PARAMETERS --- //
int D = 64; //amount of random vectors
int k = 2; //amount of random vectors each datapoint knows
int m = 32; //amount of datapoints each random vector knows
double epsilon = .15; //distance requirement for nodes to be neighbors
int minPts = 50; //minimum neighborhood size to be core point

if (args.Length != 2)
{
    throw new Exception("Must give 2 arguments (path to data, path to cluster output)");
}

// --- Load Data --- //
List<Node> dataPoints = Importer.ImportNodes(args[0]);

// --- Normalise data --- //
foreach (Node node in dataPoints)
{
    node.Normalise();
}

// --- Generate random vectors --- //
List<Core.Node> randomVectors = Preprocessing.GenerateRandomVectors(D, dataPoints[0].Vector.Length);

foreach (Core.Node node in randomVectors)
{
    node.Normalise();
}

// --- Preprocessing --- //
Console.WriteLine("Preprocessing");
Preprocessing.Preprocess(dataPoints.Select(Core.Node (x) => x).ToList(), randomVectors, k, m);

// --- Find core points --- //
Console.WriteLine("Finding core points");
var neighborhoods = FindCorePoints.FindCorePointsAndNeighbors(dataPoints, epsilon, minPts);

// --- Do DBSCAN --- //
Console.WriteLine("DBSCAN initiated");
FindComponents.DoDBSCAN(neighborhoods);

// --- Get results --- //
Console.WriteLine($"sDBSCAN with D {D}, k {k}, m {m}, epsilon {epsilon}, minPts {minPts}");

List<HashSet<Node>> clusters = FindComponents.GetClusters(neighborhoods.Keys.ToList());
Console.WriteLine(Metrics.GetMetrics(clusters, dataPoints));

Console.WriteLine("Exporting clusters");
Exporter.ExportClusters(args[1], dataPoints);
