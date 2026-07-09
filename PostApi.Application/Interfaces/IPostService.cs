using PostApi.Application.DTOs;

namespace PostApi.Application.Interfaces;

public interface IPostService
{
    Task<IReadOnlyList<PostResponse>> GetAllAsync();

    Task<PostResponse?> GetByIdAsync(Guid id);

    Task<PostResponse> CreateAsync(CreatePostRequest request, Guid userId);

    Task<bool> UpdateAsync(Guid id, UpdatePostRequest request, Guid userId);

    Task<bool> DeleteAsync(Guid id, Guid userId);
}