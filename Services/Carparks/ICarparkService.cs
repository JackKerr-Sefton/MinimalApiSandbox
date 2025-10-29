using MinimalApiSandbox.Data.Models;

namespace MinimalApiSandbox.Services;

public interface ICarparkService
{
    Task<Carpark?> GetCarparkById(int id);
    Task<IEnumerable<Carpark>> GetCarparksAsync();
}