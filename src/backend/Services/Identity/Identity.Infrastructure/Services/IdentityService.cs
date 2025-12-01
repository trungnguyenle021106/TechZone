using Identity.Application.Common.Interfaces;
using Identity.Application.Services;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public IdentityService(UserManager<ApplicationUser> userManager, IJwtTokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password, string firstName, string lastName)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            // UserManager tự động hash password + salt
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return new AuthenticationResult(false, string.Empty, errors);
            }

            return new AuthenticationResult(true, string.Empty, Enumerable.Empty<string>());
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthenticationResult(false, string.Empty, new[] { "User not found" });
            }

            // Kiểm tra pass
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                return new AuthenticationResult(false, string.Empty, new[] { "Invalid password" });
            }

            // Tạo token
            var token = _tokenGenerator.GenerateToken(user);
            return new AuthenticationResult(true, token, Enumerable.Empty<string>());
        }
    }
}