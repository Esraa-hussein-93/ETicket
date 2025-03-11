using New_Eticket.DataAccess;
using Microsoft.AspNetCore.Mvc;
using E_Tickets.Models;
using Microsoft.EntityFrameworkCore;
using New_Eticket.Repositories;
using New_Eticket.Repositories.IRepositories;


namespace New_Eticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();

       // ICinemaRepository cinemaRepository;//= new CinemaRepository();
      
        private readonly ICinemaRepository cinemaRepository;

        public CinemaController(ICinemaRepository cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
        }

        public IActionResult Index()
        {
            var cinemas = cinemaRepository.Get();
            return View(cinemas.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Cinema());
        }

        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile? file)
        {
          
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    // Save img in wwwroot
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    // Save img name in db
                    cinema.CinemaLogo = fileName;
                }

                //dbContext.Cinemas.Add(cinema);
                //dbContext.SaveChanges();
                //
                cinemaRepository.Create(cinema);
                cinemaRepository.Commit();

                return RedirectToAction("Index");
            }
            return View(cinema);
        }



        [HttpGet]
        public IActionResult Edit(int Id)
        {
           // var cinemas = dbContext.Cinemas.FirstOrDefault(e => e.Id == Id);
            var cinemas = cinemaRepository.GetOne(e => e.Id == Id);
            if (cinemas != null)
            {
                return View(cinemas);
            }
            return RedirectToAction("NotFoundPage", "Home");
        }


        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile? file)
        {
          //  var cinemaInDb = dbContext.Cinemas.AsNoTracking().FirstOrDefault(e => e.Id == cinema.Id);
            var cinemaInDb = cinemaRepository.GetOne(e => e.Id == cinema.Id);

            if (cinemaInDb != null && file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", cinemaInDb.CinemaLogo);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Save img name in db
                cinema.CinemaLogo = fileName;
            }
            else
                cinema.CinemaLogo = cinemaInDb.CinemaLogo;

            if (cinema != null)
            {
                //dbContext.Cinemas.Update(cinema);
                //dbContext.SaveChanges();         
                cinemaRepository.Edit(cinema);
                cinemaRepository.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult DeleteImg(int Id)
        {
           // var cinema = dbContext.Cinemas.FirstOrDefault(e => e.Id == Id);
            var cinema = cinemaRepository.GetOne(e => e.Id == Id);

            if (cinema != null)
            {
                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", cinema.CinemaLogo);


                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Delete img name in db
                cinema.CinemaLogo = null;
                //   dbContext.SaveChanges();
                cinemaRepository.Commit();

                return RedirectToAction("Edit", new { Id });
            }
            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int Id)
        {
            //var cinema = dbContext.Cinemas.FirstOrDefault(e => e.Id == Id);
            var cinema = cinemaRepository.GetOne(e => e.Id == Id);

            if (cinema != null)
            {
                // Delete old img from wwwroot
                if (cinema.CinemaLogo != null)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", cinema.CinemaLogo);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Delete img name in db
                cinemaRepository.Delete(cinema);
                cinemaRepository.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

    }
}
