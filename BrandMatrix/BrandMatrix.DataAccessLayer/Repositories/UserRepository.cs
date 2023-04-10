using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Domain.Interfaces;
using BrandMatrix.Models.DomainModels;

namespace BrandMatrix.DataAccessLayer.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ISqlConnection sqlConnection) : base(sqlConnection)
        {
        }
    }
}