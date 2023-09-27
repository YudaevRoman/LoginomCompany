﻿namespace Transaction;

public class Transaction<K, V> : ITransaction<K, V>
{
    public List<IAttribute<K, V>> Attributes { get; }
    public int AttributeCount
    {
        get { return Attributes.Count; }
    }

    Transaction(int capacity)
    {
        Attributes = new(capacity);
    }
    Transaction()
    {
        Attributes = new();
    }

    public void AttributeAdd(IAttribute<K, V> attribute)
    {
        Attributes.Add(attribute);
    }
    public void AttributeRemove(IAttribute<K, V> attribute)
    {
        Attributes.Remove(attribute);
    }
}

