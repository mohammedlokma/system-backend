namespace system_backend.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminsRepository Admins { get; }

        Task SaveAsync();

    }
}
