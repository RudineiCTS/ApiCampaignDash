using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly AppDbContext _context;

        public ClientsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Clients>> GetByCampaignIdAsync(int idCampaign)
        {
            return await _context.Clients
                .AsNoTracking()
                .Where(c => c.IdCampaign == idCampaign)
                .ToListAsync();
        }
    }
}
