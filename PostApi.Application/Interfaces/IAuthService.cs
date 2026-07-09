using PostApi.Application.DTOs;

namespace PostApi.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);

    Task<LoginResponse?> LoginAsync(LoginRequest request);
}