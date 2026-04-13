//Code from https://visualstudiomagazine.com/articles/2023/11/15/dbscan-data-clustering.aspx
using System;
using System.IO;
using System.Collections.Generic;

namespace DBSCAN
{
  internal class DBSCANProgram
  {
    static void Main(string[] args)
    {
      Console.WriteLine("\nBegin DBSCAN clustering C# demo");

      
      //load data from a text file
      string fn = args[0];
      double[][] X = MatLoad(fn, Enumerable.Range(1,784).ToArray(),
      ',', "#", out int[] labels);
      
      /*
      // hard-coded data
      double[][] X = new double[10][];
      X[0] = new double[] { 0.1, 1.0 };
      X[1] = new double[] { 0.2, 0.9 };
      X[2] = new double[] { 0.3, 1.0 };
      X[3] = new double[] { 0.4, 0.6 };
      X[4] = new double[] { 0.5, 0.6 };
      X[5] = new double[] { 0.6, 0.5 };
      X[6] = new double[] { 0.7, 0.8 }; // outlier
      X[7] = new double[] { 0.8, 0.1 };
      X[8] = new double[] { 0.9, 0.2 };
      X[9] = new double[] { 1.0, 0.1 };

      int[] labels = new[] { 2, 2, 2, 0, 0, 0, 0, 1, 1, 1 };
      */
      
      //Console.WriteLine("\nData: ");
      //MatShow(X, 2, 5);
      
      Console.WriteLine("Normalise data");
      foreach (var vector in X)
      {
        double length = 0;
        for (int i = 0; i < vector.Length; i++)
        {
          length += vector[i] * vector[i];
        }
        length = Math.Sqrt(length);
        
        for (int i = 0; i < vector.Length; i++)
        {
          vector[i] /= length;
        }
      }

      double epsilon = .14;
      int minPoints = 10;
      Console.WriteLine("\nClustering with epsilon = " +
        epsilon.ToString("F2") + " min points = " +
        minPoints);
      MyDBSCAN dbscan = new MyDBSCAN(epsilon, minPoints);
      int[] clustering = dbscan.Cluster(X);
      Console.WriteLine("Done ");
      
      /*
      Console.WriteLine("\nclustering results: ");
      VecShow(clustering, 3);

      Console.WriteLine("\nclustering results: ");
      for (int i = 0; i < X.Length; ++i)
      {
        for (int j = 0; j < X[i].Length; ++j)
          Console.Write(X[i][j].ToString("F2").PadLeft(6));
        Console.Write(" | ");
        Console.WriteLine(clustering[i].ToString().PadLeft(2));
      }*/

      Dictionary<int, int[]> clusters = new();
      
      for (int i = 0; i < clustering.Length; i++)
      {
        int clusterId = clustering[i];
        if(clusterId == -1) continue;
        if (!clusters.ContainsKey(clusterId))
        {
          clusters[clusterId] = new int[10];
        }
        
        clusters[clusterId][labels[i]] += 1;
      }

      Console.WriteLine("\nclustering results: ");
      Console.WriteLine("clusters:" + clusters.Count);
      int totalPoints = 0;
      foreach (var (_, cluster) in clusters)
      {
        Console.WriteLine("\nCluster");
        for (int i = 0; i < 10; i++)
        {
          Console.WriteLine(i + ": " + cluster[i]);
          totalPoints += cluster[i];
        }
      }
      Console.WriteLine("Total points: " + totalPoints);
      Console.WriteLine("Coverage: " + 100*totalPoints/(double)X.Length + " %");
      

      //Console.WriteLine("\nclustering results: ");
      //for (int cid = -1; cid < 3; ++cid)
      //{
      //  Console.WriteLine("\ncid = " + cid);
      //  for (int i = 0; i < X.Length; ++i)
      //  {
      //    if (clustering[i] == cid)
      //    {
      //      for (int j = 0; j < X[i].Length; ++j)
      //        Console.Write(X[i][j].
      //          ToString("F2").PadLeft(6));
      //      Console.WriteLine();
      //    }
      //  }
      //}

      Console.WriteLine("\nEnd demo ");
      //Console.ReadLine();
    } // Main

    static void MatShow(double[][] m, int dec, int wid)
    {
      
      for (int i = 0; i < m.Length; ++i)
      {
        for (int j = 0; j < m[0].Length; ++j)
        {
          double v = m[i][j];
          if (Math.Abs(v) < 1.0e-5) v = 0.0;  // avoid "-0.0"
          Console.Write(v.ToString("F" + dec).PadLeft(wid));
        }
        Console.WriteLine("");
      }
    }

    static void VecShow(int[] vec, int wid)
    {
      for (int i = 0; i < vec.Length; ++i)
        Console.Write(vec[i].ToString().PadLeft(wid));
      Console.WriteLine("");
    }

    //static void VecShow(double[] vec, int dec, int wid, bool nl)
    //{
    //  for (int i = 0; i < vec.Length; ++i)
    //  {
    //    double x = vec[i];
    //    if (Math.Abs(x) < 1.0e-5) x = 0.0;  // avoid "-0.00"
    //    Console.Write(x.ToString("F" + dec).PadLeft(wid));
    //  }
    //  if (nl == true)
    //    Console.WriteLine("");
    //}

    public static void ListShow(List<int> list, int wid)
    {
      for (int i = 0; i < list.Count; ++i)
        Console.Write(list[i].ToString().PadLeft(wid));
      Console.WriteLine("");
    }

    static double[][] MatCreate(int rows, int cols)
    {
      double[][] result = new double[rows][];
      for (int i = 0; i < rows; ++i)
        result[i] = new double[cols];
      return result;
    }

    static int NumNonCommentLines(string fn,
      string comment)
    {
      int ct = 0;
      string line = "";
      FileStream ifs = new FileStream(fn, FileMode.Open);
      StreamReader sr = new StreamReader(ifs);
      while ((line = sr.ReadLine()) != null)
        if (line.StartsWith(comment) == false)
          ++ct;
      sr.Close(); ifs.Close();
      return ct;
    }

    static double[][] MatLoad(string fn, int[] usecols,
      char sep, string comment, out int[] labels, int limit = -1)
    {
      // count number of non-comment lines
      int nRows = NumNonCommentLines(fn, comment);
      if (limit != -1) nRows = limit;
      
      int nCols = usecols.Length;
      double[][] result = MatCreate(nRows, nCols);
      labels = new int[nRows];
      string line = "";
      string[] tokens = null;
      FileStream ifs = new FileStream(fn, FileMode.Open);
      StreamReader sr = new StreamReader(ifs);

      int i = 0;
      while (i != limit && (line = sr.ReadLine()) != null)
      {
        if (line.StartsWith(comment) == true)
          continue;
        tokens = line.Split(sep);

        labels[i] = int.Parse(tokens[0]); 
        for (int j = 0; j < nCols; ++j)
        {
          int k = usecols[j];  // into tokens
          result[i][j] = double.Parse(tokens[k]);
        }
        ++i;
      }
      sr.Close(); ifs.Close();
      return result;
    }
  } // Program

  // --------------------------------------------------------

  public class MyDBSCAN
  {
    public double eps;
    public int minPts;
    public double[][] data;  // supplied in cluster()
    public int[] labels;  // supplied in cluster()

    public MyDBSCAN(double eps, int minPts)
    {
      this.eps = eps;
      this.minPts = minPts;
    }

    public int[] Cluster(double[][] data)
    {
      this.data = data;  // by reference
      this.labels = new int[this.data.Length];
      for (int i = 0; i < labels.Length; ++i)
        this.labels[i] = -2;  // unprocessed

      int cid = -1;  // offset the start
      for (int i = 0; i < this.data.Length; ++i)
      {
        if (this.labels[i] != -2)  // has been processed
          continue;

        List<int> neighbors = this.RegionQuery(i);
        if (neighbors.Count < this.minPts)
        {
          this.labels[i] = -1;  // noise
        }
        else
        {
          ++cid;
          this.Expand(i, neighbors, cid);
        }
      }

      return this.labels;
    }

    private List<int> RegionQuery(int p)
    {
      List<int> result = new List<int>();
      for (int i = 0; i < this.data.Length; ++i)
      {
        double dist = CosineDistance(this.data[p], this.data[i]);
        if (dist < this.eps)
          result.Add(i);
      }
      return result;
    }

    private void Expand(int p, List<int> neighbors, int cid)
    {
      this.labels[p] = cid;
      //int i = 0;
      //while(i < neighbors.Count)
      for (int i = 0; i < neighbors.Count; ++i)
      {
        int pn = neighbors[i];
        if (this.labels[pn] == -1)  // noise
          this.labels[pn] = cid;
        else if (this.labels[pn] == -2)  // unprocessed
        {
          this.labels[pn] = cid;
          List<int> newNeighbors = this.RegionQuery(pn);
          if (newNeighbors.Count >= this.minPts)
            neighbors.AddRange(newNeighbors); // modifies loop
        }
        //++i;
      }
    }

    private static double CosineDistance(double[] x1,
      double[] x2)
    {
      double sum = 0;
      for (int i = 0; i < x1.Length; i++)
      {
        sum += x1[i] * x2[i];
      }

      return 1 - sum;
    }
    
    private static double EucDistance(double[] x1,
      double[] x2)
    {
      int dim = x1.Length;
      double sum = 0.0;
      for (int i = 0; i < dim; ++i)
        sum += (x1[i] - x2[i]) * (x1[i] - x2[i]);
      return Math.Sqrt(sum);
    }

  } // class DBSCAN

  // --------------------------------------------------------

} // ns