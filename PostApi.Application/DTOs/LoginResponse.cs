namespace PostApi.Application.DTOs;

public record LoginResponse(
    string Token,
    string UserName,
    string Email
);