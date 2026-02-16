using Microsoft.FSharp.Core;
using Microsoft.VisualBasic.FileIO;
using OOPsDBSCAN;

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

List<Node> randomVectors = Preprocessing.GenerateRandomVectors(100, dataPoints[0].Vector.Length);

foreach (Node node in randomVectors)
{
    node.Normalise();
}

Console.WriteLine("preprosess");
Preprocessing.Preprocess(dataPoints, randomVectors, 2, 50);
Console.WriteLine("find corepoints");
var neighborhoods = FindCorePoints.FindCorePointsAndNeighbors(dataPoints, 0.11, 50);
Console.WriteLine("dbscan");
DBSCAN.DoDBSCAN(neighborhoods);
for (int i = 0; i < 10000; i++)
{
    Console.WriteLine("List of datapoint " + i + "and has the label " + dataPoints[i].Label + ":");
    foreach (var edge in  dataPoints[i].Edges)
    {
        Console.WriteLine(edge.Label);
    }
}

