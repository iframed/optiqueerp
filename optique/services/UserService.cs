using Microsoft.AspNetCore.Http;
using optique.IServices;
using Microsoft.Extensions.Logging;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserService> _logger;

    public UserService(IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public string GetUserName()
    {
        var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        
        if (string.IsNullOrEmpty(userName))
        {
            _logger.LogWarning("User is not authenticated or username is empty.");
            return "Unknown";
        }

        _logger.LogInformation($"Retrieved username: {userName}");
        return userName;
    }
}
