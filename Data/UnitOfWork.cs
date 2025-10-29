using Microsoft.Data.SqlClient;
using MinimalApiSandbox.Data.Repositories;
using System.Data;

namespace MinimalApiSandbox.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly string _connectionString;
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _transaction;

    public UnitOfWork(IConfiguration configuration)
    {
        _connectionString = configuration["DbConnectionString"] ?? throw new NullReferenceException();
        _connection = new SqlConnection(_connectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();
    }

    private CarparkRepository? _carparks;
    public ICarparkRepository Carparks => _carparks ??= new CarparkRepository(_transaction); // Learning: Null-coalescing operators

    private SchoolRepository? _schools;
    public ISchoolRepository Schools => _schools ??= new SchoolRepository(_transaction);

    public void Dispose()
    {
        _transaction.Dispose();

        _connection.Close();
        _connection.Dispose();
    }
}
