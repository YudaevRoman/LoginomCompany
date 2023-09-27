namespace Transaction;

public class Attribute<K, V> : IAttribute<K, V>
{
    public K Key { get; set; }
    public V Value { get; set; }

    Attribute(K key, V value)
    {
        Key = key;
        Value = value;
    }
}
