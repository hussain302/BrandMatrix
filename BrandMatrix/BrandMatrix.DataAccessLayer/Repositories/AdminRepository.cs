using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Domain.Interfaces;
using BrandMatrix.Models.DomainModels;
using System.Data.SqlClient;

namespace BrandMatrix.DataAccessLayer.Repositories
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        public AdminRepository(ISqlConnection sqlConnection) : base(sqlConnection) { }

        public async Task<Admin> LoginAdminAsync(string spName, List<SqlParameter> parameters)
        {
            Admin result = new Admin();
            try
            {
                var reader = await sqlConnection.ExecuteSPRequestAsync(spName, parameters);
                while (await reader.ReadAsync())
                {
                    Admin user = MapReaderToEntity(reader);
                    result = user;
                }

                await reader.CloseAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}