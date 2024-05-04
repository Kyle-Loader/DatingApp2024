using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppRole : IdentityRole<int>
    {
        //ICollection and list are basically the same and can be used interchangably
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}