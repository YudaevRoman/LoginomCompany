using Cluster;
using Transaction;

namespace Source;

public interface IClusterSource<K, V> : ISource
{
    int GetClusterIndex();
    void SetClusterIndex(int clusterIndex);
}