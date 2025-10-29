using MinimalApiSandbox.Data.Models;

namespace MinimalApiSandbox.Data.Repositories;

public interface ISchoolRepository
{
    Task<IEnumerable<School>> GetAllAsync();
    Task<School?> GetByIdAsync(int id);
}
