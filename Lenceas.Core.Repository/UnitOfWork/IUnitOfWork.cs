namespace Lenceas.Core.Repository
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}