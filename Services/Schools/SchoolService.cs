using MinimalApiSandbox.Data;
using MinimalApiSandbox.Data.Models;

namespace MinimalApiSandbox.Services;

public class SchoolService(IUnitOfWork unitOfWork) : ISchoolService // We don't need to implement IDisposable here because the service provider in ASP.NET will automatically call dispose on disposable services
{
    private readonly IUnitOfWork _dc = unitOfWork;

    public Task<IEnumerable<School>> GetSchoolsAsync()
        => _dc.Schools.GetAllAsync();

    public Task<School?> GetSchoolById(int id)
        => _dc.Schools.GetByIdAsync(id);
}