using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Domain.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace BrandMatrix.DataAccessLayer.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private readonly ISqlConnection sqlConnection;
        public BaseRepository(ISqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string spName, List<SqlParameter> parameters)
        {
            List<T> result = new List<T>();
            try
            {
                var reader = (parameters is not null) ? await sqlConnection.ExecuteSPRequestAsync(spName, parameters)
                                                      : await sqlConnection.ExecuteSPRequestAsync(spName);

                while (await reader.ReadAsync())
                {
                    var item = MapReaderToEntity(reader);
                    result.Add(item);
                }

                await reader.CloseAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<T> GetByIdAsync(string spName, List<SqlParameter> parameters)
        {
            T result = new T();            
            try
            {
                var reader = await sqlConnection.ExecuteSPRequestAsync(spName, parameters);

                while (await reader.ReadAsync())
                {
                    T item = MapReaderToEntity(reader);
                    result = item;
                }

                await reader.CloseAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<T> GetByNameAsync(string spName, List<SqlParameter> parameters)
        {
            T result = new T();
            try
            {
                var reader = await sqlConnection.ExecuteSPRequestAsync(spName, parameters);

                while (await reader.ReadAsync())
                {
                    T item = MapReaderToEntity(reader);
                    result = item;
                }

                await reader.CloseAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<bool> CreateAsync(string spName, List<SqlParameter> parameters)
        {
            try
            {
                var reader = await sqlConnection.ExecuteSPRequestAsync(spName, parameters);
                await reader.CloseAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<bool> UpdateAsync(string spName, List<SqlParameter> parameters)
        {
            try
            {
                var reader = await sqlConnection.ExecuteSPRequestAsync(spName, parameters);
                await reader.CloseAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(string spName, List<SqlParameter> parameters)
        {
            try
            {
                var reader = await sqlConnection.ExecuteSPRequestAsync(spName, parameters);
                await reader.CloseAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        private T MapReaderToEntity(IDataReader reader)
        {
            T entity = new T();
            var properties = typeof(T).GetProperties();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                var property = properties.FirstOrDefault(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
                if (property != null && reader[columnName] != DBNull.Value)
                {
                    var value = Convert.ChangeType(reader[columnName], property.PropertyType);
                    property.SetValue(entity, value);
                }
            }
            return entity;
        }

    }
}
