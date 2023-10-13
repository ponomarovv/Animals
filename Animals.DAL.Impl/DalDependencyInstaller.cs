using Animals.DAL.Abstract.Repository;
using Animals.DAL.Abstract.Repository.Base;
using Animals.DAL.Impl.Repository;
using Microsoft.Extensions.DependencyInjection;
using StoreCS.DAL.Impl.Repository.Base;

namespace Animals.DAL.Impl;

public static class DalDependencyInstaller
{
    public static void InstallRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDogRepository, DogRepository>();
        
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    }
}
