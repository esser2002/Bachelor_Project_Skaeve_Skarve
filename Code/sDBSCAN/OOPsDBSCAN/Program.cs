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

Preprocessing.Preprocess(dataPoints, randomVectors, 2, 1);
