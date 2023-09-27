using Source;
using Cluster;
using Transaction;

using System.Collections.Generic;

namespace Clope;

public interface IClope<K, V>
{
    ITransactionSource<K, V> TransactionSource { get; set; }
    IClusterSource<K, V> ClusterSource { get; set; }
    List<ICluster<K, V>> Execution(double r);
}
