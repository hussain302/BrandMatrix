using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandMatrix.Models.DomainModels
{
    public class Subscriptions
    {
        public Subscriptions()
        {
            
        }

        public int SubscriptionId { get; set; }

        public int OrganizationId { get; set; }

        public string Description { get; set; } = string.Empty;

        public string OrganizationName { get; set; } = string.Empty;

        public string OwnerName { get; set; } = string.Empty;

        public string Plan { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int DurationInDays { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
