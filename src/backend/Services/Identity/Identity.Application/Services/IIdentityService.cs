namespace Identity.Application.Services
{
    public record AuthenticationResult(bool Success, string Token, IEnumerable<string> Errors);

    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password, string firstName, string lastName);
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}