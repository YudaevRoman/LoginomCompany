using Source;
using Cluster;
using Transaction;

using System.Collections.Generic;

namespace Clope;

public class Clope<K, V> : IClope<K, V>
{
    public ITransactionSource<K, V> TransactionSource { get; set; }
    public IClusterSource<K, V> ClusterSource { get; set; }

    public Clope(ITransactionSource<K, V> transactionSource, IClusterSource<K, V> clusterSource)
    {
        TransactionSource = transactionSource;
        ClusterSource = clusterSource;
    }

    public List<ICluster<K, V>> Execution(double r)
    {
        List<ICluster<K, V>> clusters = new();
        clusters.Add(new Cluster<K, V>());

        Execution_StepOne(clusters, r);
        Execution_StepTwo(clusters, r);

        return clusters;
    }

    private void Execution_StepOne(List<ICluster<K, V>> clusters, double r)
    {
        ITransaction<K, V> transaction;
        int profitIndex;

        ClusterSource.Restart();
        TransactionSource.Restart();
        while (TransactionSource.Move())
        {
            transaction = TransactionSource.GetTransaction();
            profitIndex = Profit(clusters, transaction, r);
            clusters[profitIndex].Add(transaction);
            if (profitIndex == clusters.Count - 1)
            {
                clusters.Add(new Cluster<K, V>());
            }
            ClusterSource.SetClusterIndex(profitIndex);
            ClusterSource.Move();
        }
    }
    private void Execution_StepTwo(List<ICluster<K, V>> clusters, double r)
    {
        ITransaction<K, V> transaction;
        int deltaRemoveIndex;
        int profitIndex;
        bool moved;

        do
        {
            moved = false;
            ClusterSource.Restart();
            TransactionSource.Restart();
            while (TransactionSource.Move() && ClusterSource.Move())
            {
                transaction = TransactionSource.GetTransaction();
                deltaRemoveIndex = ClusterSource.GetClusterIndex();
                profitIndex = Profit(clusters, transaction, r, deltaRemoveIndex);
                if (profitIndex != deltaRemoveIndex)
                {
                    clusters[profitIndex].Add(transaction);
                    clusters[deltaRemoveIndex].Remove(transaction);
                    if (profitIndex == clusters.Count - 1)
                    {
                        clusters.Add(new Cluster<K, V>());
                    }
                    ClusterSource.SetClusterIndex(profitIndex);
                    moved = true;
                }
            }
        } while (moved != false);
    }

    private int Profit(List<ICluster<K, V>> clusters, ITransaction<K, V> t, double r, int deltaRemoveIndex = -1)
    {
        double deltaAdd;
        double deltaMax;
        int deltaMaxIndex = 0;
        double deltaRemove = 0;

        if (deltaRemoveIndex >= 0)
        {
            deltaRemove = clusters[deltaRemoveIndex].DeltaRemove(t, r);
        }

        deltaMax = clusters[0].DeltaAdd(t, r);
        for (int i = 1; i < clusters.Count; i++)
        {
            deltaAdd = clusters[i].DeltaAdd(t, r) + deltaRemove;
            if (deltaAdd > deltaMax)
            {
                deltaMaxIndex = i;
                deltaMax = deltaAdd;
            }
        }

        return deltaMaxIndex;
    }
}
