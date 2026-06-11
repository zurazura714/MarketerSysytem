namespace MarketerSystem.Abstractions.Repository;

public interface IUnitOfWork
{
    Task CommitAsync();
    Task RollbackAsync();
}
