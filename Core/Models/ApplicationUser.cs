using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
