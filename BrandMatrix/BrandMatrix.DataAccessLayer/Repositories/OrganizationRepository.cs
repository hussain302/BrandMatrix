using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Domain.Interfaces;
using BrandMatrix.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandMatrix.DataAccessLayer.Repositories
{
    public class OrganizationRepository : BaseRepository<Organizations>, IOrganizationRepository
    {
        public OrganizationRepository(ISqlConnection sqlConnection) : base(sqlConnection)
        {
        }
    }
}
