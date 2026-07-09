namespace PostApi.Application.DTOs;

public record UpdatePostRequest(
    string Title,
    string Content
);