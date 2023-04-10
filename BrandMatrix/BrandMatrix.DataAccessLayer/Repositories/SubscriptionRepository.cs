using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.Domain.Interfaces;
using BrandMatrix.Models.DomainModels;

namespace BrandMatrix.DataAccessLayer.Repositories
{
    public class SubscriptionRepository : BaseRepository<Subscriptions>, ISubscriptionRepository
    {
        public SubscriptionRepository(ISqlConnection sqlConnection) : base(sqlConnection)
        {
        }
    }
}
