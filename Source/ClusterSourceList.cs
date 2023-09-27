using Cluster;
using Transaction;

using System.Collections.Generic;

namespace Source;

public class ClusterSourceList : IClusterSource<int, string>
{
    private int currentIndex;
    private List<int> clusters;

    public bool CheckEnd { get; private set; }

    public ClusterSourceList()
    {
        clusters = new();
        CheckEnd = false;
        currentIndex = -1;
    }

    public int GetClusterIndex()
    {
        return !CheckEnd ? clusters[currentIndex] : default;
    }

    public void SetClusterIndex(int clusterIndex)
    {
        if (CheckEnd)
        {
            clusters.Add(default);
            CheckEnd = false;
        }

        clusters[currentIndex] = clusterIndex;
    }

    public void Move()
    {
        currentIndex++;

        if (currentIndex >= clusters.Count)
        {
            currentIndex = clusters.Count;
            CheckEnd = true;
            return;
        }
    }

    public void Restart()
    {
        CheckEnd = false;
        currentIndex = -1;
    }
}
