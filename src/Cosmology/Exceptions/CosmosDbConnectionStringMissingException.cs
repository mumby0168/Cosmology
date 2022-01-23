using Microsoft.Azure.Cosmos;

namespace Cosmology.Exceptions;

public class CosmosDbConnectionStringMissingException : Exception
{
    public CosmosDbConnectionStringMissingException() : base(
        $"Please set the {Constants.ConnectionStringEnvironmentVariable} env var to a valid cosmos connection string.")
    {
    }
}