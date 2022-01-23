using Cosmology.Registrars;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Cosmology.Extensions;

public static class ServiceCollectionExtensions
{
    public static ITypeRegistrar BuildTypeRegistrar(this IServiceCollection services) => 
        new ServiceCollectionRegistrar(services);
}