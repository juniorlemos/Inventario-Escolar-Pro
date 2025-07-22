using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Communication.Request;

namespace InventarioEscolar.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
        Task ForgotPasswordAsync(ForgotPasswordRequest request);
        Task ResetPasswordAsync(string email, string token, string newPassword);
    }
}
