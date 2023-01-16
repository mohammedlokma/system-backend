namespace system_backend.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminsRepository Admins { get; }
        IAgentRepository Agents { get; }
        ICompanyRepository Companies { get; }
        Task SaveAsync();

    }
}
