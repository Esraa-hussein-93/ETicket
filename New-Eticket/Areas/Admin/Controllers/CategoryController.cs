using E_Tickets.Models;
using Microsoft.AspNetCore.Mvc;
using New_Eticket.DataAccess;
using New_Eticket.Repositories;
using New_Eticket.Repositories.IRepositories;


namespace New_Eticket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //ApplicationDbContext dbContext = new ApplicationDbContext();
        //ICategoryRepository categoryRepository; //= new CategoryRepository();
        
        private readonly ICategoryRepository categoryRepository;
     


        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository= categoryRepository;
        }

        public IActionResult Index()
        {
            //var categories= dbContext.Categories;
            var categories = categoryRepository.Get();
            return View(categories.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            // Validation
            //ModelState.Remove("Products");

            if (ModelState.IsValid)
            {
                //dbContext.Categories.Add(new Category
                //{
                //    Name = category.Name
                //});
                //dbContext.SaveChanges(); 
                categoryRepository.Create(category);

                categoryRepository.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            //var category = dbContext.Categories.FirstOrDefault(e=>e.Id==Id);
            var category = categoryRepository.GetOne(e => e.Id == Id);
            if (category != null)
            {
                return View(category);
            }
            return RedirectToAction("NotFoundPage", "Home");
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            // Validation

            //dbContext.Categories.Update(new Category
            //{
            //    Id = category.Id,
            //    Name = category.Name
            //});
            //dbContext.SaveChanges();
            categoryRepository.Edit(category);
            categoryRepository.Commit();
            TempData["notifaction"] = "Update Category Successfuly";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int Id)
        {
            var category = categoryRepository.GetOne(e => e.Id == Id);

            if (category != null)
            {
                categoryRepository.Delete(category);
                categoryRepository.Commit();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }



    }
}
