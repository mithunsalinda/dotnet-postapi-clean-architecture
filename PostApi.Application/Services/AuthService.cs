using PostApi.Application.DTOs;
using PostApi.Application.Interfaces;
using PostApi.Domain.Entities;
using BCrypt.Net;
namespace PostApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var emailExists = await _unitOfWork.Users.EmailExistsAsync(request.Email);

        if (emailExists)
        {
            throw new Exception("Email already exists.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = passwordHash
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);

        if (user is null)
        {
            return null;
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash
        );

        if (!isPasswordValid)
        {
            return null;
        }

        var token = _jwtTokenService.GenerateToken(user);

        return new LoginResponse(
            token,
            user.UserName,
            user.Email
        );
    }
}