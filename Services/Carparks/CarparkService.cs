using MinimalApiSandbox.Data;
using MinimalApiSandbox.Data.Models;

namespace MinimalApiSandbox.Services;

public class CarparkService(IUnitOfWorkFactory unitOfWorkFactory) : ICarparkService
{
    private readonly IUnitOfWorkFactory _unitOfWorkFactory = unitOfWorkFactory;

    public Task<IEnumerable<Carpark>> GetCarparksAsync()
    {
        using IUnitOfWork dc = _unitOfWorkFactory.Create();

        return dc.Carparks.GetAllAsync();
    }

    public Task<Carpark?> GetCarparkByIdAsync(int id)
    {
        using IUnitOfWork dc = _unitOfWorkFactory.Create();

        return dc.Carparks.GetByIdAsync(id);
    }
}
