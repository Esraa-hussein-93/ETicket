using New_Eticket.DataAccess;
using New_Eticket.Models;
using Microsoft.AspNetCore.Mvc;
using E_Tickets.Models;
using Microsoft.EntityFrameworkCore;
using New_Eticket.Repositories;
using New_Eticket.Repositories.IRepositories;

namespace New_Eticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();
        IActorRepository actorRepository;//= new ActorRepository();
        public ActorController(IActorRepository actorRepository)
        {
            this.actorRepository = actorRepository;
        }
        public IActionResult Index()
        {
            var actors = actorRepository.Get();

            return View(actors.ToList());
        }


         
        [HttpGet]
        public IActionResult Create()
        {
           
            return View(new Actor());
        }

        [HttpPost]
        public IActionResult Create(Actor actor, IFormFile? file)
        {
            // Validation

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
                    actor.ProfilePicture = fileName;
                }

                //dbContext.Actors.Add(actor);
                //dbContext.SaveChanges();
                actorRepository.Create(actor);
                actorRepository.Commit();

                return RedirectToAction("Index");
            }

            return View(actor);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            //var actor = dbContext.Actors.FirstOrDefault(e => e.Id ==Id);
            var actor = actorRepository.GetOne(e => e.Id ==Id);

            if (actor != null)
            {
                return View(actor);
            }
            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Actor actor, IFormFile? file)
        {
            //var actorInDb = dbContext.Actors.AsNoTracking().FirstOrDefault(e => e.Id == actor.Id);
            var actorInDb = actorRepository.GetOne(e => e.Id == actor.Id);
            if (actorInDb != null && file != null && file.Length > 0)
            {
                // Save img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", actorInDb.ProfilePicture);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Save img name in db
                actor.ProfilePicture = fileName;
            }
            else
                actor.ProfilePicture = actorInDb.ProfilePicture;

            if (actor != null)
            {
                //dbContext.Actors.Update(actor);
                //dbContext.SaveChanges();
                actorRepository.Edit(actor);
                actorRepository.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult DeleteImg(int Id)
        {
            var actor = actorRepository.GetOne(e => e.Id == Id);

            if (actor != null)
            {
                // Delete old img from wwwroot
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", actor.ProfilePicture);


                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                // Delete img name in db
                actor.ProfilePicture = null;
                actorRepository.Commit();

                return RedirectToAction("Edit", new { Id });
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int Id)
        {
           // var actor = dbContext.Actors.FirstOrDefault(e => e.Id == Id);
            var actor = actorRepository.GetOne(e => e.Id == Id);

            if (actor != null)
            {
                // Delete old img from wwwroot
                if (actor.ProfilePicture != null)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", actor.ProfilePicture);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Delete img name in db
                //dbContext.Actors.Remove(actor);
                //dbContext.SaveChanges();             
                actorRepository.Delete(actor);
                actorRepository.Commit();

                return RedirectToAction("Index");
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

    }
}
