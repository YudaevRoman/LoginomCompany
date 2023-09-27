namespace Transaction;

public class Attribute : IAttribute<int, string>
{
    public int Key { get; set; }
    public string Value { get; set; }

    Attribute(int key, string value)
    {
        Key = key;
        Value = value;
    }
}
