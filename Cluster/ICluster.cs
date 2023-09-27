using Transaction;
using System.Collections.Generic;

namespace Cluster;

public interface ICluster<K, V>
{
    Dictionary<IAttribute<K, V>, int> Occ { get; }
    double N { get; }
    double W { get; }
    double S { get; }

    double DeltaAdd(ITransaction<K, V> t, double r);
    double DeltaRemove(ITransaction<K, V> t, double r);

    void Add(ITransaction<K, V> t);
    void Remove(ITransaction<K, V> t);
}
