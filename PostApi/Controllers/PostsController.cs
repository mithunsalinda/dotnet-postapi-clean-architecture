using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostApi.Application.DTOs;
using PostApi.Application.Interfaces;
using System.Security.Claims;

namespace PostApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _postService.GetAllAsync();

        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        var post = await _postService.GetByIdAsync(id);

        if (post is null)
        {
            return NotFound(new
            {
                message = "Post not found."
            });
        }

        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(CreatePostRequest request)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new
            {
                message = "Invalid token."
            });
        }

        var post = await _postService.CreateAsync(request, userId.Value);

        return Ok(post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(Guid id, UpdatePostRequest request)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new
            {
                message = "Invalid token."
            });
        }

        var isUpdated = await _postService.UpdateAsync(id, request, userId.Value);

        if (!isUpdated)
        {
            return NotFound(new
            {
                message = "Post not found or you are not allowed to update this post."
            });
        }

        return Ok(new
        {
            message = "Post updated successfully."
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new
            {
                message = "Invalid token."
            });
        }

        var isDeleted = await _postService.DeleteAsync(id, userId.Value);

        if (!isDeleted)
        {
            return NotFound(new
            {
                message = "Post not found or you are not allowed to delete this post."
            });
        }

        return Ok(new
        {
            message = "Post deleted successfully."
        });
    }

    private Guid? GetCurrentUserId()
    {
        var userIdValue = User.FindFirst("userId")?.Value;

        if (string.IsNullOrWhiteSpace(userIdValue))
        {
            return null;
        }

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return null;
        }

        return userId;
    }
}