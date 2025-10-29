using Dapper;
using MinimalApiSandbox.Data.Models;
using System.Data;

namespace MinimalApiSandbox.Data.Repositories;

public class SchoolRepository(IDbTransaction transaction) : RepositoryBase(transaction), ISchoolRepository // Learning: Primary Constructors
{
    public Task<IEnumerable<School>> GetAllAsync()
        => Connection.QueryAsync<School>("SELECT * FROM Schools");

    public Task<School?> GetByIdAsync(int id)
    {
        CommandDefinition command = new(
            commandText: "SELECT * FROM Schools WHERE Id = @id",  // Dapper: https://www.learndapper.com/
            parameters: new { id },
            transaction: Transaction
            );

        return Connection.QueryFirstOrDefaultAsync<School>(command);
    }
}