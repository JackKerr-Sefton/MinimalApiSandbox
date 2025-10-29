using MinimalApiSandbox.Data.Models;

namespace MinimalApiSandbox.Data.Repositories;

public interface ICarparkRepository
{
    Task<IEnumerable<Carpark>> GetAllAsync();
    Task<Carpark?> GetByIdAsync(int id);
}
