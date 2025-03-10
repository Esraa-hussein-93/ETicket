using E_Tickets.Models;
using New_Eticket.DataAccess;
using New_Eticket.Repositories.IRepositories;

namespace New_Eticket.Repositories
{
    public class MovieRepository :Repository<Movie>, IMovieRepository
    {

        private readonly ApplicationDbContext dbContext;

        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
