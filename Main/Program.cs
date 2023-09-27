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
        TransactionSourceFile transactionSource = new("MushroomSource.data", ",", "?");
        ClusterSourceList clusterSource = new();

        Clope<int, string> clope = new(transactionSource, clusterSource);

        List<ICluster<int, string>> clusters = clope.Execution(2.6);

        Show(clusters);
    }

    private static void Show(List<ICluster<int, string>> clusters)
    {
        int i = 1;
        StringBuilder buffer = new();
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

        Console.WriteLine(buffer);
        Console.Read();
    }
}