using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Models
{
    // Kế thừa IdentityUser để tận dụng toàn bộ sức mạnh có sẵn
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}