using Cluster;
using System.IO;
using Transaction;

namespace Source;

public class TransactionSourceFile : ITransactionSource<int, string>
{
    private string fileName;
    private string? currentLine;
    private StreamReader reader;

    public string FileName
    {
        get { return fileName; }
        set
        {
            fileName = value;
            reader = new(fileName);
        }
    }

    public string Separator { get; set; }
    public string NullValue { get; set; }

    public TransactionSourceFile(string _fileName, string separator, string nullValue)
    {
        currentLine = null;
        FileName = _fileName;
        Separator = separator;
        NullValue = nullValue;
    }

    public ITransaction<int, string> GetTransaction()
    {
        ITransaction<int, string> transaction = new Transaction<int, string>();
        if (currentLine != null)
        {
            string[] values = currentLine.Split(Separator);
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != NullValue)
                {
                    transaction.AttributeAdd(new Attribute<int, string>(i, values[i]));
                }
            }
        }

        return transaction;
    }

    public bool Move()
    {
        string? buffer = reader.ReadLine();
        if (buffer == null)
        {
            return false;
        }

        currentLine = buffer;

        return true;
    }

    public void Restart()
    {
        currentLine = null;
        reader.DiscardBufferedData();
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
    }
}
