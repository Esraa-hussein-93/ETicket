using System.Diagnostics;
using E_Tickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using New_Eticket.DataAccess;
using New_Eticket.Models;
using New_Eticket.Models.ViewModels;



namespace New_Eticket.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var movies = dbContext.Movies .Include(e => e.Category).Include(e => e.Cinema);
         
            return View(movies.ToList());
        }


        public IActionResult Details(int Id)
        {
            var movie = dbContext.Movies.Include(e => e.Category).Include(e => e.Cinema).Include(e => e.Actors).FirstOrDefault(e => e.Id == Id);

            if (movie != null)
            {
                // Anno. Type => Product, ProductsWithCategories


                return View(movie);
            }
         


            return RedirectToAction(nameof(NotFoundPage));
        }
        //public IActionResult Details(int id) // ÚßÚßÉ ÇáÓäíä æÇáÓäæÇÊ åäÇ
        //{
        //    var movie = dbContext.Movies
        //                         .Include(e => e.Category)
        //                         .Include(e => e.Cinema)
        //                         .Include(e => e.Actors)
        //                         .FirstOrDefault(e => e.Id == id);

        //    if (movie != null)
        //    {
        //        var relatedMovies = dbContext.Movies
        //                                     .Where(e => e.CategoryId == movie.CategoryId && e.Id != movie.Id)
        //                                     .Take(4)
        //                                     .ToList();

        //        var movieDetailsViewModel = new MovieDetailsViewModel
        //        {
        //            Movie = movie,
        //            RelatedMovies = relatedMovies
        //        };

        //        return View(movieDetailsViewModel);
        //    }

        //    return RedirectToAction(nameof(NotFoundPage));
        //}



        public IActionResult Cinema()
        {
            var cinemas = dbContext.Cinemas;
                return View (cinemas.ToList());
        }
      
        public IActionResult Category()
        {
          
                // Get the category based on the provided Id
                var category = dbContext.Categories.ToList();

                if (category != null) // æÇááå åæ ÇáãÝÑæÖ Çäí ÇÖÈØ ÇáÍÊÉ ÈÊÇÚÊ áãÇ ÇáíÒÑæ íÖÛØ Úáì Çááíäß íÙåÑáí ßá ÇáÇÝáÇã Çááí Ýí ÇáßÇÊíÌæÑí äÝÓåÇ æáßä æááå 
                {

                //var categoryWithMovies = new 
                //{
                //    Category = category,
                //    Movies = dbContext.Movies.Where(m => m.CategoryId == Id).ToList()
                //};
                //return View(categoryWithMovies);
                return View(category);
                }
            

            // If no category found, redirect to the NotFoundPage
            return RedirectToAction(nameof(NotFoundPage));
        }



        public IActionResult NotFoundPage()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
