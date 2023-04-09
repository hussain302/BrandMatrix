using System.Data;
using System.Data.SqlClient;

namespace BrandMatrix.Domain.Interfaces
{
    public interface ISqlConnection
    {
        Task<SqlDataReader> ExecuteSPRequestAsync(string cmdText, IEnumerable<SqlParameter> parameters);
        Task<SqlDataReader> ExecuteSPRequestAsync(string cmdText);
    }
}
