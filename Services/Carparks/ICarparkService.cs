using MinimalApiSandbox.Data.Models;

namespace MinimalApiSandbox.Services;

public interface ICarparkService
{
    Task<Carpark?> GetCarparkByIdAsync(int id);
    Task<IEnumerable<Carpark>> GetCarparksAsync();
}