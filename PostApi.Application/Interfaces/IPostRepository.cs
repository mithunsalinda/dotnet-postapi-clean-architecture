using PostApi.Domain.Entities;

namespace PostApi.Application.Interfaces;

public interface IPostRepository : IGenericRepository<Post>
{
    Task<IReadOnlyList<Post>> GetAllWithUserAsync();

    Task<IReadOnlyList<Post>> GetPostsByUserIdAsync(Guid userId);
}