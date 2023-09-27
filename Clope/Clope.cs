using Source;
using Cluster;
using Transaction;

using System.Diagnostics;
using System.Collections.Generic;

namespace Clope;

public class Clope<K, V> : IClope<K, V>
{
    public int IterationCounter { get; private set; }
    public Stopwatch Timer { get; private set; }

    public ITransactionSource<K, V> TransactionSource { get; set; }
    public IClusterSource<K, V> ClusterSource { get; set; }

    public Clope(ITransactionSource<K, V> transactionSource, IClusterSource<K, V> clusterSource)
    {
        TransactionSource = transactionSource;
        ClusterSource = clusterSource;
        Timer = new Stopwatch();
    }

    public List<ICluster<K, V>> Execution(double r)
    {
        List<ICluster<K, V>> clusters = new();
        clusters.Add(new Cluster<K, V>());

        Timer.Reset();

        Timer.Start();
        Execution_StepOne(clusters, r);
        Execution_StepTwo(clusters, r);

        clusters.RemoveAll(cluster => cluster.N == 0);
        Timer.Stop();

        return clusters;
    }

    private void Execution_StepOne(List<ICluster<K, V>> clusters, double r)
    {
        ITransaction<K, V> transaction;
        int profitIndex;

        ClusterSource.Restart();
        TransactionSource.Restart();

        ClusterSource.Move();
        TransactionSource.Move();
        while (!TransactionSource.CheckEnd)
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
            TransactionSource.Move();
        }
    }
    private void Execution_StepTwo(List<ICluster<K, V>> clusters, double r)
    {
        ITransaction<K, V> transaction;
        int deltaRemoveIndex;
        int profitIndex;
        bool moved;

        IterationCounter = 0;
        do
        {

            IterationCounter++;
            moved = false;

            ClusterSource.Restart();
            TransactionSource.Restart();

            ClusterSource.Move();
            TransactionSource.Move();
            while (!TransactionSource.CheckEnd && !ClusterSource.CheckEnd)
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
                ClusterSource.Move();
                TransactionSource.Move();
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
