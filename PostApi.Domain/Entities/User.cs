namespace PostApi.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Post> Posts { get; set; } = new List<Post>();
}