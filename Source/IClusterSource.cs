using Cluster;
using Transaction;

namespace Source;

public interface IClusterSource<K, V> : ISource
{
    ICluster<K, V> GetCluster();
    void SetCluster(ICluster<K, V> cluster);
}