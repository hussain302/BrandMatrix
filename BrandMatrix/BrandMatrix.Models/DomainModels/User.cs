using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandMatrix.Models.DomainModels
{
    public class User
    {

        public int UserId { get; set; }

        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; } = string.Empty;

        public string OwnerName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
