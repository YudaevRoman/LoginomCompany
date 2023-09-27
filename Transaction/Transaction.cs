namespace Transaction;

public class Transaction : ITransaction<int, string>
{
    public List<IAttribute<int, string>> Attributes { get; }
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

    public void AttributeAdd(IAttribute<int, string> attribute)
    {
        Attributes.Add(attribute);
    }
    public void AttributeRemove(IAttribute<int, string> attribute)
    {
        Attributes.Remove(attribute);
    }
}

