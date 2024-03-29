﻿using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Domain.Interfaces;
using BrandMatrix.Models.DomainModels;
using System.Data.SqlClient;

namespace BrandMatrix.DataAccessLayer.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ISqlConnection sqlConnection) : base(sqlConnection)
        {


        }

        public async Task<User> LoginUserAsync(string spName, List<SqlParameter> parameters)
        {
            User result = new User();
            try
            {
                var reader = await sqlConnection.ExecuteSPRequestAsync(spName, parameters);
                while (await reader.ReadAsync())
                {
                    User user = MapReaderToEntity(reader);
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