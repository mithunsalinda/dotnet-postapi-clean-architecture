using PostApi.Domain.Entities;

namespace PostApi.Application.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);

    Task<bool> EmailExistsAsync(string email);
}