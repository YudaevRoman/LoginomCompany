using Cluster;
using Transaction;

using System.IO;
using System.Reflection.Metadata;

namespace Source;

public class TransactionSourceFile : ITransactionSource<int, string>
{
    private string fileName;
    private string? currentLine;
    private StreamReader reader;

    public bool CheckEnd { get; private set; }

    public string FileName
    {
        get { return fileName; }
        set
        {
            CheckEnd = false;
            fileName = value;
            reader = new(fileName);
        }
    }

    public string Separator { get; set; }
    public string NullValue { get; set; }

    public TransactionSourceFile(string _fileName, string separator, string nullValue)
    {
        currentLine = null;

        CheckEnd = false;
        FileName = _fileName;
        Separator = separator;
        NullValue = nullValue;
    }

    public ITransaction<int, string> GetTransaction()
    {
        if (CheckEnd || (currentLine == null))
        {
            throw new Exception("Reading from file end");
        }

        ITransaction<int, string> transaction = new Transaction<int, string>();
        string[] values = currentLine.Split(Separator);
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] != NullValue)
            {
                transaction.AttributeAdd(new Attribute<int, string>(i, values[i]));
            }
        }
        return transaction;
    }

    public void Move()
    {
        string? buffer = reader.ReadLine();
        if (buffer == null)
        {
            CheckEnd = true;
        }

        currentLine = buffer;
    }

    public void Restart()
    {
        CheckEnd = false;
        currentLine = null;
        reader.DiscardBufferedData();
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
    }
}
