using E_Tickets.Models;
using New_Eticket.DataAccess;
using New_Eticket.Repositories.IRepositories;

namespace New_Eticket.Repositories
{
    public class CinemaRepository : Repository<Cinema>, ICinemaRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CinemaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
