using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Cosmology.Registrars;

public sealed class ServiceProviderTypeResolver : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _provider;

    public ServiceProviderTypeResolver(IServiceProvider provider) => 
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));

    public object Resolve(Type? type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type), "The type cannot be null");
        }

        return _provider.GetRequiredService(type);
    }

    public void Dispose()
    {
        if (_provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
