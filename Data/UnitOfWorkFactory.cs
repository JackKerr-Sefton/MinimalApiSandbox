namespace MinimalApiSandbox.Data;

public class UnitOfWorkFactory(IConfiguration configuration) : IUnitOfWorkFactory
{
    private readonly IConfiguration _configuration = configuration;

    public IUnitOfWork Create() => new UnitOfWork(_configuration); // In practice we'd probably new up the connection here and pass it in but I've set it up this way for comparing UnitOfWork as a service approaches
}