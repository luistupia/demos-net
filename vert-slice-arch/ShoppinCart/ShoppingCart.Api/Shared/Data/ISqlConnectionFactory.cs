using System.Data;

namespace ShoppingCart.Api.Shared.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}