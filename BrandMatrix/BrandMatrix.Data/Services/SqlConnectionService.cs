using System.Data;
using System.Data.SqlClient;
using BrandMatrix.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BrandMatrix.Domain.Services
{
    public class SqlConnectionService : ISqlConnection, IDisposable
    {
        private readonly string _connectionString;
        private readonly ILogger<SqlConnectionService> _logger;
        private readonly SqlConnection _connection;
        private readonly object _lockObject = new object();
        private bool _disposed;

        public SqlConnectionService(string connectionString, ILogger<SqlConnectionService> logger)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connection = new SqlConnection(_connectionString);
            _connection.Open();
        }

        public async Task<SqlDataReader> ExecuteSPRequestAsync(string spName, IEnumerable<SqlParameter> parameters)
        {
            if (string.IsNullOrWhiteSpace(spName))
            {
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(spName));
            }

            _logger.LogDebug($"Executing stored procedure '{spName}' with parameters: {string.Join(",", parameters)}");

            using var command = new SqlCommand(spName, _connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            try
            {
                return await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing stored procedure '{spName}' with parameters: {string.Join(",", parameters)}");
                throw;
            }
        }
        public async Task<SqlDataReader> ExecuteSPRequestAsync(string spName)
        {
            if (string.IsNullOrWhiteSpace(spName))
            {
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(spName));
            }

            _logger.LogDebug($"Executing stored procedure '{spName}'");

            using var command = new SqlCommand(spName, _connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            try
            {
                return await command.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing stored procedure '{spName}'");
                throw;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _connection?.Dispose();
                _disposed = true;
            }
        }
    }
}
