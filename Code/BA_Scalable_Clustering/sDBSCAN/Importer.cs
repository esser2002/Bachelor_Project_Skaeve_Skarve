using Microsoft.VisualBasic.FileIO;

namespace sDBSCAN;

public static class Importer
{
    public static List<Node> ImportNodes(string inputPath)
    {
        using TextFieldParser csvParser = new TextFieldParser(inputPath);
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

        return dataPoints;
    }
}