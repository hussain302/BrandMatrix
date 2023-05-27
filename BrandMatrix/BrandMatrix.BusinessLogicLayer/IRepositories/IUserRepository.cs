
using BrandMatrix.Models.DomainModels;
using System.Data.SqlClient;

namespace BrandMatrix.BusinessLogicLayer.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {

        Task<User> LoginUserAsync(string spName, List<SqlParameter> parameters);

    }
}
