using MinimalApiSandbox.Data.Models;

namespace MinimalApiSandbox.Services;
public interface ISchoolService
{
    Task<School?> GetSchoolByIdAsync(int id);
    Task<IEnumerable<School>> GetSchoolsAsync();
}