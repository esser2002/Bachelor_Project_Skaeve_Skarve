using System.Diagnostics;
using Core;
using sHDBSCAN;
using Exporter = sHDBSCAN.Exporter;
using Node = Core.Node;

public class Program
{
    public static void Main(string[] args)
    {
        const int numberOfThreads = 8;

// --- PARAMETERS --- //
        int D = 1024; //amount of random vectors 
        int l = 2; //amount of random vectors each datapoint knows
        int k = 5; //k is the amount of points for creating core distance

        if (args.Length != 3)
        {
            throw new Exception("Must give 3 arguments (path to data, path to dendrogram output, path to stats output)");
        }

        Util.dataPoints = Importer.ImportNodes(args[0]);

        int m = (int)(2*Math.Sqrt(Util.dataPoints.Length));

// --- Normalise data --- //
        foreach (HNode node in Util.dataPoints)
        {
            node.Normalise();
        }

// --- Stopwatch init --- //
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        TimeSpan lastTime = stopWatch.Elapsed;

// --- Generate random vectors --- // 
        Console.WriteLine("Generating random vectors");
        List<Node> randomVectors = Preprocessing.GenerateRandomVectors(D, Util.dataPoints[0].Vector.Length);

        foreach (Node node in randomVectors)
        {
            node.Normalise();
        }
        PrintLap("Random vectors");

// --- Preprocessing --- // 
        Console.WriteLine("Preprocessing");
        Preprocessing.Preprocess(Util.dataPoints, randomVectors.ToArray(), l, m);
        PrintLap("Preprocessing");

// --- Set visible nodes --- // 
        Console.WriteLine("Set visible nodes");
        foreach (HNode n in Util.dataPoints)
        {
            n.SetVisibleNodes();
        }
        PrintLap("Set visible nodes");

// --- Set core distance --- //
        Console.WriteLine("Set core dist");

        List<Task> threads = new ();

        for (int i = 0; i < numberOfThreads; i++)
        {
            var from = Util.dataPoints.Length * i / numberOfThreads ;
            var to = Util.dataPoints.Length * (i + 1) / numberOfThreads ;
            Task t = Task.Run(() =>
            {
                for (int j = from; j < to; j++)
                {
                    Util.dataPoints[j].SetCoreDist(k);
                }
            }); 
            threads.Add(t);
        }

        Task.WaitAll(threads);

        PrintLap("Set Core Dist");

// --- Set mutual reachability --- //
        Console.WriteLine("Set mutual reachability");
        threads = new ();

        for (int i = 0; i < numberOfThreads; i++)
        {
            var from = Util.dataPoints.Length * i / numberOfThreads ;
            var to = Util.dataPoints.Length * (i + 1) / numberOfThreads ;
            Task t = Task.Run(() =>
            {
                for (int j = from; j < to; j++)
                {
                    Util.dataPoints[j].SetMutualReachability();
                }
            }); 
            threads.Add(t);
        }
        Task.WaitAll(threads);
        PrintLap("Set Mutual reachability");

// --- Create MST --- //
        Console.WriteLine("CreateMST");
        UnionFind uf = new UnionFind(Util.dataPoints.Length);
        var mst = MST.Kruskals(Util.dataPoints, uf);

        Console.WriteLine("MST size: " + mst.Length);
        PrintLap("MST");

// --- Cluster tree --- //
        Console.WriteLine("Cluster tree");
        var dendrogram = Dendrogram.CreateDendrogram(mst, Util.dataPoints.Length);

        PrintLap("Cluster tree");

        stopWatch.Stop();
        Exporter.ExportDendrogram(args[1],dendrogram);

// --- Timer cleanup --- //
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);


        Exporter.ExportsHdbscanStats(args[2], D, l, m, k, Util.dataPoints.Length, ts.TotalSeconds );


        Console.WriteLine($"sHDBSCAN ran on dataset of size {Util.dataPoints.Length} with D {D}, l {l}, m {m}, k {k}");
        Console.WriteLine("Total time " + elapsedTime);

// Prints the time since last lap and the name input
        void PrintLap(string lapName)
        {
            var lap = stopWatch.Elapsed - lastTime;
            Console.WriteLine(lapName + " done: " + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", lap.Hours, lap.Minutes, lap.Seconds, lap.Milliseconds));
            lastTime = stopWatch.Elapsed;
        }
    }
}


