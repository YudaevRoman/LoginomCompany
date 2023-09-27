using Transaction;

namespace Source;

public interface ITransactionSource<K, V> : ISource
{
    ITransaction<K, V> GetTransaction();
}
