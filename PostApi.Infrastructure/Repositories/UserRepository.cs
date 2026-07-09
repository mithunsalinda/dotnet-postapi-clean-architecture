using Microsoft.EntityFrameworkCore;
using PostApi.Application.Interfaces;
using PostApi.Domain.Entities;
using PostApi.Infrastructure.Data;

namespace PostApi.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await Context.Users
            .AnyAsync(x => x.Email == email);
    }
}