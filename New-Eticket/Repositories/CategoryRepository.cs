using E_Tickets.Models;
using New_Eticket.DataAccess;
using New_Eticket.Repositories.IRepositories;

namespace New_Eticket.Repositories
{
    public class CategoryRepository: Repository<Category>, ICategoryRepository
    {

        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }

}
