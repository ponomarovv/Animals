using Animals.BLL.Abstract.Services;
using Animals.BLL.Impl.Mappers;
using Animals.BLL.Impl.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Animals.BLL.Impl;

public static class BllDependencyInstaller
{
    public static void InstallServices(this IServiceCollection services)
    {
        services.AddScoped<IDogService, DogService>();

    }

    public static void InstallMappers(this IServiceCollection services)
    {
        var config = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappersProfile());
        });

        IMapper mapper = config.CreateMapper();
        services.AddSingleton(mapper);
    }
}
