using MinimalApiSandbox.Data.Repositories;

namespace MinimalApiSandbox.Data;

public interface IUnitOfWork : IDisposable // Learning: IDisposable
{
    ICarparkRepository Carparks { get; }
    ISchoolRepository Schools { get; }
}
