using E_Tickets.Models;
using Microsoft.EntityFrameworkCore;

namespace New_Eticket.DataAccess;

public class ApplicationDbContext: DbContext
{
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<ActorMovie> ActorMovies { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
    {
    }

    public ApplicationDbContext()
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=New-ETicket_Task;Integrated Security=True;TrustServerCertificate=True");

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ActorMovie>().HasKey(e => new { e.MovieId, e.ActorId });
    }
}
