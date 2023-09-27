namespace Transaction;

public interface IAttribute<K, V>
{
    K Key { get; set; }
    V Value { get; set; }
}
