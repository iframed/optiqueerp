using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using optique.Models;

public interface IAuthService
{
    string GenerateToken(string username, string role);
}

public class AuthService : IAuthService
{
    private readonly Parametres _parametres;

    public AuthService(IOptions<Parametres> options)
    {
        _parametres = options.Value ?? throw new ArgumentNullException(nameof(options), "JWT settings are not configured correctly.");
    }

    public string GenerateToken(string username, string role)
    {
        if (string.IsNullOrEmpty(_parametres.Key) || string.IsNullOrEmpty(_parametres.Issuer) || string.IsNullOrEmpty(_parametres.Audience))
        {
            throw new InvalidOperationException("JWT settings are not configured correctly.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_parametres.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddHours(3),
            Issuer = _parametres.Issuer,
            Audience = _parametres.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
