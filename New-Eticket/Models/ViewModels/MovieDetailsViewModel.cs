using E_Tickets.Models;

namespace New_Eticket.Models.ViewModels
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; }
        public List<Movie> RelatedMovies { get; set; }
    }
}
