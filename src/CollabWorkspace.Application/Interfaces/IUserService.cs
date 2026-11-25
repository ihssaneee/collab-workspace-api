namespace CollabWorkspace.Application.Interfaces
{
    public interface IUserService
    {
        Task<string?> GetUserNameAsync(Guid userId, CancellationToken cancellationToken);
        Task<IReadOnlyDictionary<Guid, string?>> GetUserNamesAsync(
            IEnumerable<Guid> userIds,
            CancellationToken cancellationToken = default);
    }
}