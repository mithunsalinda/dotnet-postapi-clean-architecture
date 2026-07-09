namespace PostApi.Application.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    IPostRepository Posts { get; }

    Task<int> SaveChangesAsync();
}