using PostApi.Domain.Entities;

namespace PostApi.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}