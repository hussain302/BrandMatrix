using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandMatrix.BusinessLogicLayer.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string spName, List<SqlParameter> parameters);
        Task<T> GetByIdAsync(string spName, List<SqlParameter> parameters);
        Task<T> GetByNameAsync(string spName, List<SqlParameter> parameters);
        Task<bool> CreateAsync(string spName, List<SqlParameter> parameters);
        Task<bool> UpdateAsync(string spName, List<SqlParameter> parameters);
        Task<bool> DeleteAsync(string spName, List<SqlParameter> parameters);
    }
}