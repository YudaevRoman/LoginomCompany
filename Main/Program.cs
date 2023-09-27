using Clope;
using Source;
using Cluster;
using Transaction;

using System.Text;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        TransactionSourceFile transactionSource = new("MushroomSource_BigData.data", ",", "?");
        ClusterSourceList clusterSource = new();

        Clope<int, string> clope = new(transactionSource, clusterSource);

        List<ICluster<int, string>> clusters = clope.Execution(2.6);

        StringBuilder buffer = new();
        ShowClope(clope, buffer);
        ShowClusters(clusters, buffer);

        Console.WriteLine(buffer);
        Console.ReadKey();
    }

    private static void ShowClope(Clope<int, string> clope, StringBuilder buffer)
    {
        buffer.Append("\n\nAlgorithm time: ");
        buffer.Append(clope.Timer.Elapsed);
        buffer.Append("\nIteration counter: ");
        buffer.Append(clope.IterationCounter);
    }

    private static void ShowClusters(List<ICluster<int, string>> clusters, StringBuilder buffer)
    {
        int i = 1;
        IAttribute<int, string> edibility = new Attribute<int, string>(0, "e");
        foreach (ICluster<int, string> cluster in clusters)
        {
            buffer.Append("\n\nCluster: ");
            buffer.Append(i++);

            edibility = new Attribute<int, string>(0, "e");
            if (cluster.Occ.ContainsKey(edibility))
            {
                buffer.Append("\nEdible: ");
                buffer.Append(cluster.Occ[edibility]);
            }

            edibility = new Attribute<int, string>(0, "p");
            if (cluster.Occ.ContainsKey(edibility))
            {
                buffer.Append("\nPoisonous: ");
                buffer.Append(cluster.Occ[edibility]);
            }
        }
    }
}