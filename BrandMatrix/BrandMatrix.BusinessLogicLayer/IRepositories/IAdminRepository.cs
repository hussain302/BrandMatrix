
using BrandMatrix.Models.DomainModels;
using System.Data.SqlClient;

namespace BrandMatrix.BusinessLogicLayer.IRepositories
{
    public interface IAdminRepository : IBaseRepository<Admin>
    {
        Task<Admin> LoginAdminAsync(string spName, List<SqlParameter> parameters);
    }
}
