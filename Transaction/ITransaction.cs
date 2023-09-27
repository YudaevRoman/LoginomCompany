using System.Collections.Generic;

namespace Transaction;

public interface ITransaction<K, V>
{
    List<IAttribute<K, V>> Attributes { get; }
    int AttributeCount { get; }
    void AttributeAdd(IAttribute<K, V> attribute);
    void AttributeRemove(IAttribute<K, V> attribute);
}
