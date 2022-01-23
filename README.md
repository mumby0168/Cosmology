# Cosmology
A CLI tool for working with Azure Cosmos DB.

To install the tool run the following command.

```shell
dotnet tool install --global Mumby0168.Cosmology    
```

You can then see what the tool offers by running.
```shell
cosmology --help
```

## Setting a connection string

There are two ways to give Cosmology a connection.

### Environment Variable (recommended)

The environment variable that cosmology uses is called `COSMOLOGY_COSMOS_ACCOUNT_CONNECTION_STRING`.

You can set this in MacOs with the following command.

```shell
export COSMOLOGY_COSMOS_ACCOUNT_CONNECTION_STRING="my-connection-string"
```

You can un-set this using

```shell
unset COSMOLOGY_COSMOS_ACCOUNT_CONNECTION_STRING
```

### Using the configuration file

If no environment variable can be found then the tool will look in a file named `appsettings.json` for the connection string. This is stored next to the `.dll` for the tools.
