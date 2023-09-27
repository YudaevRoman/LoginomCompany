namespace Transaction;

public class Attribute<K, V> : IAttribute<K, V>
{
    public K Key { get; set; }
    public V Value { get; set; }

    public Attribute(K key, V value)
    {
        Key = key;
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Attribute<K, V> val)
        {
            return Key.Equals(val.Key) && Value.Equals(val.Value);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }
}
