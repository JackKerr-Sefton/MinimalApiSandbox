using MinimalApiSandbox.Data.Models;

namespace MinimalApiSandbox.Services;
public interface ISchoolService
{
    Task<School?> GetSchoolById(int id);
    Task<IEnumerable<School>> GetSchoolsAsync();
}