using Blazored.LocalStorage;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

public class AuthService
{
    private readonly ILocalStorageService _localStorage;

    public AuthService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public string? UserId { get; private set; }

    public async Task LoadUserAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("jwt-token");

        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        }
        else
        {
            UserId = null;
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("jwt-token");
        UserId = null;
    }
}
