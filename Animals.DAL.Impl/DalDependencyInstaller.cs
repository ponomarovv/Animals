using Animals.DAL.Abstract.Repository;
using Animals.DAL.Abstract.Repository.Base;
using Animals.DAL.Impl.Repository;
using Animals.DAL.Impl.Repository.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Animals.DAL.Impl;

public static class DalDependencyInstaller
{
    public static void InstallRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDogRepository, DogRepository>();
        
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    }
}
