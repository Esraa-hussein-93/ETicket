
using New_Eticket.DataAccess;
using New_Eticket.Models;
using New_Eticket.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using E_Tickets.Models;
using Microsoft.CodeAnalysis;
using New_Eticket.Repositories.IRepositories;


namespace New_Eticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        //  ApplicationDbContext dbContext = new ApplicationDbContext();
        IMovieRepository movieRepository ;// new MovieRepository();
        ICategoryRepository categoryRepository;//= new CategoryRepository();
        ICinemaRepository cinemaRepository;// = new CinemaRepository();


        public MovieController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }
        public IActionResult Index()
        {
            //var movies = dbContext.Movies.Include(e => e.Category).Include(e => e.Cinema);
            var movies = movieRepository.Get(includes: [e => e.Category, c=>c.Cinema]);
               
            return View(movies.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var categories = categoryRepository.Get();
            var cinemas = cinemaRepository.Get();
            ViewData["Categories"]= categories.ToList();
            ViewData["Cinemas"]= cinemas.ToList();
            ViewData["MovieStatuses"] = Enum.GetValues(typeof(MovieStatus)).Cast<MovieStatus>().ToList();
            return View( new Movie());
        }
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile? imgFile, IFormFile? trailerFile)
        {
            // Validation
            if (ModelState.IsValid)
            {
                // Handling Image Upload
                if (imgFile != null && imgFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        imgFile.CopyTo(stream);
                    }
                    movie.ImgUrl = fileName;
                }

                // Handling Trailer Upload (if needed)
                if (trailerFile != null && trailerFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(trailerFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\trailers", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        trailerFile.CopyTo(stream);
                    }
                    movie.TrailerUrl = fileName;  // Make sure your movie model has this property
                }

                //dbContext.Movies.Add(movie);
                //dbContext.SaveChanges();              
              
                movieRepository.Create(movie);
                movieRepository.Commit();

                return RedirectToAction("Index");
            }

            // Return to the form with validation errors if necessary
            //var categories = dbContext.Categories;
            //var cinemas = dbContext.Cinemas;

            var categories = categoryRepository.Get();
            var cinemas = cinemaRepository.Get();

            ViewData["Categories"] = categories.ToList();
            ViewData["Cinemas"] = cinemas.ToList();
            ViewData["MovieStatuses"] = Enum.GetValues(typeof(MovieStatus)).Cast<MovieStatus>().ToList();
            return View(movie);
        }

        //[HttpPost]
        //public IActionResult Create(Movie movie, IFormFile? file)
        //{
        //    // Validation

        //    if (ModelState.IsValid)
        //    {
        //        if (file != null && file.Length > 0)
        //        {
        //            // Save img in wwwroot
        //            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

        //            using (var stream = System.IO.File.Create(filePath))
        //            {
        //                file.CopyTo(stream);
        //            }

        //            // Save img name in db
        //            movie.ImgUrl = fileName;
        //        }

        //        dbContext.Movies.Add(movie);
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Index");
        //    }

        //    var categories = dbContext.Categories;
        //    var cinemas = dbContext.Cinemas;
        //    ViewData["Categories"] = categories.ToList();
        //    ViewData["Cinemas"] = cinemas.ToList();
        //    ViewData["MovieStatuses"] = Enum.GetValues(typeof(MovieStatus)).Cast<MovieStatus>().ToList();
        //    //ViewBag.Categories = categories;
        //    return View(movie);
        //}

        [HttpGet]
        public IActionResult Edit(int Id)
        {
           // var movie = dbContext.Movies.FirstOrDefault(e => e.Id == Id);
            var movie = movieRepository.GetOne(e => e.Id == Id);

            var categories = categoryRepository.Get();
            var cinemas =cinemaRepository.Get();
            //ViewBag.Categories = categories;
            ViewData["Categories"] = categories.ToList(); 
            ViewData["Cinemas"] = cinemas.ToList();
            ViewData["MovieStatuses"] = Enum.GetValues(typeof(MovieStatus)).Cast<MovieStatus>().ToList();
            if (movie != null)
            {
                return View(movie);
            }
            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile? imgFile, IFormFile? trailerFile)
        {

            var movieInDB = movieRepository.GetOne(e => e.Id == movie.Id);

            if (movieInDB != null && imgFile != null && imgFile.Length > 0)
            //if (movieInDB != null && imgFile != null && imgFile.Length > 0&& trailerFile!=null && trailerFile.Length>0)
            //
            //النية موجودة والله اني اضبط الايديت للفيديو بس الوقت مش موجود وحساه معقد وطويل كدا 
            //والوقت دا عايزة اصلي فيه التروايح :(
            // مش هيبقى تكديرة من التاسكات والعيال 
            //  في الايديت مشكلة اصلا انه مش بيرضى يعدل لي اي صورة قبل كدا 
            // ودا عايز يتعمل بعد رمضان علشان مخسرش صيامي 

            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    imgFile.CopyTo(stream);
                }

                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", movieInDB.ImgUrl);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Save img name in db
                movie.ImgUrl = fileName;
            }
            else
                movie.ImgUrl = movieInDB.ImgUrl;

            if (movie != null)
            {
                //dbContext.Movies.Update(movie);
                //dbContext.SaveChanges();             

                movieRepository.Edit(movie);
                movieRepository.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        //    // Validation
        // جزء فيه هرتلة كدا :D بمساعدة شات جي بي تي علشان بس 
        //    if (ModelState.IsValid)
        //    {C
        //        // Handling Image Upload
        //        if (imgFile != null && imgFile.Length > 0)
        //        {
        //            // Delete old image if exists
        //            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", movie.ImgUrl);
        //            if (System.IO.File.Exists(oldImagePath))
        //            {
        //                System.IO.File.Delete(oldImagePath);
        //            }

        //            // Save new image
        //            var imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName);
        //            var imgFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", imgFileName);
        //            using (var stream = System.IO.File.Create(imgFilePath))
        //            {
        //                imgFile.CopyTo(stream);
        //            }
        //            movie.ImgUrl = imgFileName;
        //        }

        //        // Handling Trailer Upload (if needed)
        //        if (trailerFile != null && trailerFile.Length > 0)
        //        {
        //            var trailerFileName = Guid.NewGuid().ToString() + Path.GetExtension(trailerFile.FileName);
        //            var trailerFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\trailers", trailerFileName);
        //            using (var stream = System.IO.File.Create(trailerFilePath))
        //            {
        //                trailerFile.CopyTo(stream);
        //            }
        //            movie.TrailerUrl = trailerFileName;
        //        }

        //        // Update movie details in the database
        //        dbContext.Movies.Update(movie);
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Index");
        //    }

        //    // Return to the form with validation errors if necessary
        //    var categories = dbContext.Categories;
        //    var cinemas = dbContext.Cinemas;
        //    ViewData["Categories"] = categories.ToList();
        //    ViewData["Cinemas"] = cinemas.ToList();
        //    ViewData["MovieStatuses"] = Enum.GetValues(typeof(MovieStatus)).Cast<MovieStatus>().ToList();
        //    return View(movie);
        //}

        //public IActionResult DeleteImg(int Id)
        //{
        //    var movie = dbContext.Movies.FirstOrDefault(e => e.Id == Id);

        //    if (movie != null)
        //    {
        //        // Delete old img from wwwroot
        //        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", movie.ImgUrl);


        //        if (System.IO.File.Exists(oldPath))
        //        {
        //            System.IO.File.Delete(oldPath);
        //        }

        //        // Delete img name in db
        //        movie.ImgUrl = null;
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Edit", new { Id });
        //    }

        //    return RedirectToAction("NotFoundPage", "Home");
        //}

        //public IActionResult Delete(int Id)
        //{
        //    var movie = dbContext.Movies.FirstOrDefault(e => e.Id == Id);

        //    if (movie != null)
        //    {
        //        // Delete old img from wwwroot
        //        if (movie.ImgUrl != null)
        //        {
        //            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", movie.ImgUrl);
        //            if (System.IO.File.Exists(oldPath))
        //            {
        //                System.IO.File.Delete(oldPath);
        //            }
        //        }

        //        // Delete img name in db
        //        dbContext.Movies.Remove(movie);
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("NotFoundPage", "Home"); // 

    }



    }
