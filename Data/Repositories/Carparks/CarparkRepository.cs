using Dapper;
using MinimalApiSandbox.Data.Models;
using System.Data;

namespace MinimalApiSandbox.Data.Repositories;

public class CarparkRepository(IDbTransaction transaction) : RepositoryBase(transaction), ICarparkRepository // Learning: Primary Constructors
{
    public Task<IEnumerable<Carpark>> GetAllAsync() 
        => Connection.QueryAsync<Carpark>("SELECT * FROM Carparks");

    public Task<Carpark?> GetByIdAsync(int id)
    {
        CommandDefinition command = new(
            commandText: "SELECT * FROM Carparks WHERE Id = @id",  // Dapper: https://www.learndapper.com/
            parameters: new { id },
            transaction: Transaction
            );

        return Connection.QueryFirstOrDefaultAsync<Carpark>(command);
    }
}
