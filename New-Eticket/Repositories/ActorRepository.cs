using E_Tickets.Models;
using New_Eticket.DataAccess;
using New_Eticket.Repositories.IRepositories;

namespace New_Eticket.Repositories
{
    public class ActorRepository:Repository<Actor>, IActorRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ActorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
