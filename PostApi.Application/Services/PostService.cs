using PostApi.Application.DTOs;
using PostApi.Application.Interfaces;
using PostApi.Domain.Entities;

namespace PostApi.Application.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<PostResponse>> GetAllAsync()
    {
        var posts = await _unitOfWork.Posts.GetAllWithUserAsync();

        return posts.Select(post => new PostResponse(
            post.Id,
            post.Title,
            post.Content,
            post.UserId,
            post.User.UserName,
            post.CreatedAt,
            post.UpdatedAt
        )).ToList();
    }

    public async Task<PostResponse?> GetByIdAsync(Guid id)
    {
        var posts = await _unitOfWork.Posts.GetAllWithUserAsync();

        var post = posts.FirstOrDefault(x => x.Id == id);

        if (post is null)
        {
            return null;
        }

        return new PostResponse(
            post.Id,
            post.Title,
            post.Content,
            post.UserId,
            post.User.UserName,
            post.CreatedAt,
            post.UpdatedAt
        );
    }

    public async Task<PostResponse> CreateAsync(CreatePostRequest request, Guid userId)
    {
        var post = new Post
        {
            Title = request.Title,
            Content = request.Content,
            UserId = userId
        };

        await _unitOfWork.Posts.AddAsync(post);
        await _unitOfWork.SaveChangesAsync();

        var user = await _unitOfWork.Users.GetByIdAsync(userId);

        return new PostResponse(
            post.Id,
            post.Title,
            post.Content,
            post.UserId,
            user?.UserName ?? string.Empty,
            post.CreatedAt,
            post.UpdatedAt
        );
    }

    public async Task<bool> UpdateAsync(Guid id, UpdatePostRequest request, Guid userId)
    {
        var post = await _unitOfWork.Posts.GetByIdAsync(id);

        if (post is null)
        {
            return false;
        }

        if (post.UserId != userId)
        {
            return false;
        }

        post.Title = request.Title;
        post.Content = request.Content;
        post.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Posts.Update(post);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var post = await _unitOfWork.Posts.GetByIdAsync(id);

        if (post is null)
        {
            return false;
        }

        if (post.UserId != userId)
        {
            return false;
        }

        _unitOfWork.Posts.Delete(post);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}