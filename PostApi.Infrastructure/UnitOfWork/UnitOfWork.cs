using PostApi.Application.Interfaces;
using PostApi.Infrastructure.Data;
using PostApi.Infrastructure.Repositories;

namespace PostApi.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Posts = new PostRepository(_context);
    }

    public IUserRepository Users { get; }

    public IPostRepository Posts { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}