namespace BrandMatrix.Models.DomainModels
{
    public class Organizations
    {
        public Organizations()
        {
            
        }

        public int OrganizationId { get; set; }
        
        public string OrganizationName { get; set; } = string.Empty;
        
        public string OwnerName { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public string Phone { get; set; } = string.Empty;
        
        public string Website { get; set; } = string.Empty;        
        
        public string Address { get; set; } = string.Empty;        
        
        public string City { get; set; } = string.Empty;
        
        
        public string State { get; set; } = string.Empty;
        
        
        public string Country { get; set; } = string.Empty;
        
        
        public string ZipCode { get; set; } = string.Empty;
        
        
        public DateTime CreatedAt { get; set; }
        
        
        public DateTime UpdatedAt { get; set; }
    }
}
