using G3MWL.Models;

namespace G3MWL.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(UserLogin login);

        Task<bool> RegisterAsync(UserRegister register);
    }
}


