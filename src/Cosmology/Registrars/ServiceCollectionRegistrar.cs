using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Cosmology.Registrars;

public sealed class ServiceCollectionRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _builder;

    public ServiceCollectionRegistrar(IServiceCollection builder)
    {
        _builder = builder;
    }

    public ITypeResolver Build() => 
        new ServiceProviderTypeResolver(_builder.BuildServiceProvider());

    public void Register(Type service, Type implementation) => 
        _builder.AddSingleton(service, implementation);

    public void RegisterInstance(Type service, object implementation) => 
        _builder.AddSingleton(service, implementation);

    public void RegisterLazy(Type service, Func<object> func)
    {
        if (func is null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        _builder.AddSingleton(service, _ => func());
    }
}