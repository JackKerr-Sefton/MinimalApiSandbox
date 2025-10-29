using System.Data;

namespace MinimalApiSandbox.Data.Repositories;

// We mark this `abstract` as it is useless on its own and should not be instantiated as is. Basically it allows us to standardize instantiation for our "real" repositories.
public abstract class RepositoryBase(IDbTransaction transaction) // Learning: Primary Constructors
{
    protected readonly IDbTransaction Transaction = transaction;
    protected readonly IDbConnection Connection = transaction.Connection ?? throw new NullReferenceException(nameof(transaction.Connection));
}
