using Friday.ERP.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Friday.ERP.Server.Utilities;

internal class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseOracle(configuration.GetConnectionString("SqlConnection")!,
                o =>
                {
                    o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    o.MigrationsAssembly("Friday.ERP.Server");
                    o.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19);
                });
        return new RepositoryContext(builder.Options);
    }
}