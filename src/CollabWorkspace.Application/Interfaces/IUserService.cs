namespace CollabWorkspace.Application.Interfaces
{
    public interface IUserService
    {
        Task<string?> GetUserNameAsync(Guid userId, CancellationToken cancellationToken);
    }
}