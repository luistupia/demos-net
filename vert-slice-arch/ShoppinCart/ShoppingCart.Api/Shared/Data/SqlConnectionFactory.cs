using System.Data;
using Microsoft.Data.Sqlite;

namespace ShoppingCart.Api.Shared.Data;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
{
    private readonly Lazy<IDbConnection> _connection;
    private bool _disposed;

    public SqlConnectionFactory(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        _connection = new Lazy<IDbConnection>(() =>
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        }, true);
    }

    public IDbConnection CreateConnection()
    {
        return _disposed ? throw new ObjectDisposedException(nameof(SqlConnectionFactory)) : _connection.Value;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            if (_connection.IsValueCreated)
            {
                _connection.Value.Dispose();
            }
            _disposed = true;
        }
    }
}
