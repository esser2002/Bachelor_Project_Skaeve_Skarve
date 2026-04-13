using Core;
using Microsoft.VisualBasic.FileIO;
using sDBSCAN;
using Node = sDBSCAN.Node;

var path = args[0];
using TextFieldParser csvParser = new TextFieldParser(path);

csvParser.SetDelimiters(",");
csvParser.HasFieldsEnclosedInQuotes = true;

// Skip the row with the column names
csvParser.ReadLine();

List<Node> dataPoints = [];

while (!csvParser.EndOfData)
{
    // Read current line fields, pointer moves to the next line.
    string[] fields = csvParser.ReadFields() ?? throw new InvalidOperationException();
    dataPoints.Add(new Node(fields));
}

//Normalise data
foreach (Node node in dataPoints)
{
    node.Normalise();
}

int D = 64;
int k = 2;
int m = 32;
double epsilon = .08;
int minPts = 50;

List<Core.Node> randomVectors = Preprocessing.GenerateRandomVectors(D, dataPoints[0].Vector.Length);

foreach (Core.Node node in randomVectors)
{
    node.Normalise();
}

Console.WriteLine("Preprocessing");
Preprocessing.Preprocess(dataPoints.Select(Core.Node (x) => x).ToList(), randomVectors, k, m);
Console.WriteLine("Finding corepoints");
var neighborhoods = FindCorePoints.FindCorePointsAndNeighbors(dataPoints, epsilon, minPts);
Console.WriteLine("DBSCAN initiated");
FindComponents.DoDBSCAN(neighborhoods);

List<HashSet<Node>> Clusters = FindComponents.GetClusters(neighborhoods.Keys.ToList());
Console.WriteLine($"sDBSCAN with D {D}, k {k}, m {m}, epsilon {epsilon}, minPts {minPts}");
Console.WriteLine("number of clusters " + Clusters.Count);
Console.WriteLine("coverage: " + (100*Clusters.Sum(c => c.Count)/(double)dataPoints.Count) + " %");
Console.WriteLine("first cluster: " );
int stopper = 0;
foreach (Node node in Clusters[0])
{
    if (stopper++ > 1000)
    {
        break;}
    Console.Write(node.Label);
}
Console.WriteLine();

foreach (HashSet<Node> cluster in Clusters)
{
    int[] freq = new int[10];
    foreach (Node node in cluster)
    {
        freq[node.Label] += 1;
    }
    Console.WriteLine("Cluster length " + cluster.Count + " contents:");
    for (int i = 0; i < freq.Length; i++)
    {
        Console.WriteLine(" - " + i + ": " + freq[i]);
    }
}

Console.WriteLine("Exporting clusters");
Exporter.ExportClusters(args[1], dataPoints);

