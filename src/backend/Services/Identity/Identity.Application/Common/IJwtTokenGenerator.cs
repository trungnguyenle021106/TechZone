using Identity.Domain.Models;

namespace Identity.Application.Common.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user);
    }
}