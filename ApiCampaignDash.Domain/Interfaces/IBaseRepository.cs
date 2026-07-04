namespace ApiCampaignDash.Domain.Interfaces
{
    public interface IBaseRepository<T>where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?>GetByIdAsync(int id);
    }
}
