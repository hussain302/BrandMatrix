using BrandMatrix.Models.DomainModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace BrandMatrix.Utils
{
    public static class Mapper
    {

        public static Organizations ToDb(this Organizations personalInfo, Organizations AddressInfo)
        {
            return new Organizations
            {
                Address = AddressInfo.Address,
                City = AddressInfo.City,
                Country = AddressInfo.Country,
                State = AddressInfo.State,
                ZipCode = AddressInfo.ZipCode,
                CreatedAt = DateTime.Now,
                Email = personalInfo.Email,
                OrganizationName = personalInfo.OrganizationName,
                OwnerName = personalInfo.OwnerName,
                Phone = personalInfo.Phone,
                Website = personalInfo.Website,
            };      
        }




    }

    public static class SQLParameterMapper
    {

        public static SqlDbType GetSqlDbType(Type type)
        {
            var parameter = new SqlParameter();
            parameter.SqlDbType = type switch
            {
                Type t when t == typeof(int) => SqlDbType.Int,
                Type t when t == typeof(string) => SqlDbType.NVarChar,
                Type t when t == typeof(DateTime) => SqlDbType.DateTime2,
                // add more type mappings as needed
                _ => throw new NotSupportedException($"Type '{type}' is not supported.")
            };
            return parameter.SqlDbType;
        }

        public static List<SqlParameter> GetSqlParameters<T>(T model)
        {
            var properties = typeof(T).GetProperties();

            var parameters = new List<SqlParameter>();

            foreach (var property in properties)
            {

                var value = property.GetValue(model);
                var sqlDbType = GetSqlDbType(property.PropertyType);

                var parameter = new SqlParameter($"@{property.Name}", sqlDbType)
                {
                    Value = value ?? DBNull.Value
                };

                parameters.Add(parameter);
            }

            return parameters;
        }
    }
}
