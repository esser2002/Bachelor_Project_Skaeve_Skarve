using Microsoft.VisualBasic.FileIO;

namespace sHDBSCAN;

public static class Importer
{
    public static HNode[] ImportNodes(string path)
    {
        using TextFieldParser csvParser = new TextFieldParser(path);

        csvParser.SetDelimiters(",");
        csvParser.HasFieldsEnclosedInQuotes = true;
        csvParser.ReadLine(); // Skip the row with the column names
        
        List<HNode> dataPoints = new();

        while (!csvParser.EndOfData)
        {
            // Read current line fields, pointer moves to the next line.
            string[] fields = csvParser.ReadFields() ?? throw new InvalidOperationException();
            HNode node = new HNode(fields);
            dataPoints.Add(node);
        }
        return dataPoints.ToArray();
    }
    
    
    
}