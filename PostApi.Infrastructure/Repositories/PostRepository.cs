using Microsoft.EntityFrameworkCore;
using PostApi.Application.Interfaces;
using PostApi.Domain.Entities;
using PostApi.Infrastructure.Data;

namespace PostApi.Infrastructure.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyList<Post>> GetAllWithUserAsync()
    {
        return await Context.Posts
            .Include(x => x.User)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Post>> GetPostsByUserIdAsync(Guid userId)
    {
        return await Context.Posts
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}