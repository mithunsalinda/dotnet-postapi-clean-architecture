namespace PostApi.Application.DTOs;

public record PostResponse(
    Guid Id,
    string Title,
    string Content,
    Guid UserId,
    string UserName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);