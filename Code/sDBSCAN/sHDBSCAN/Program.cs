using Microsoft.VisualBasic.FileIO;
using OOPsDBSCAN;
using sHDBSCAN;

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
foreach (HNode n in dataPoints)
{
     n.setCoreDist(k);
}