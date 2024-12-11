using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqloMVC.DataAccess;
using UniqloMVC.Models;
using UniqloMVC.ViewModels.Category;
using UniqloMVC.ViewModels.Product;
using UniqloMVC.ViewModels.Slider;

namespace UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {
       
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            

            if (!ModelState.IsValid) return View();
            Category category = new Category
            {
                Name = vm.Name
            };
            

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null) return BadRequest();
            var data = await _context.Categories
                .Where(x => x.Id == id)
                .Select(x => new CategoryUptadeVM
                {
                   Name= x.Name,
                  //  OtherFilesUrls = x.Images.Select(y => y.ImageUrl)
                })
                .FirstOrDefaultAsync();
            if (data is null) return NotFound();
            //ViewBag.Categories = await _context.Brands.Where(x => !x.IsDeleted)
            //    .ToListAsync();
            return View(data);


        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, CategoryUptadeVM vm)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Categories.FindAsync(id);

            if (data is null) return NotFound();


            //if (vm.File != null)
            //{
            //    if (!vm.File.IsValidType("image"))
            //        ModelState.AddModelError("File", "File must be image!");
            //    if (!vm.File.IsValidSize(5 * 1024))
            //        ModelState.AddModelError("File", "File length must be less than 2mg");

            //    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "sliders", data.ImageUrl);

            //    if (System.IO.File.Exists(oldFilePath))
            //    {
            //        System.IO.File.Delete(oldFilePath);
            //    }

            //    string newFileName = await vm.File.UploadAsync(_env.WebRootPath, "imgs", "sliders");
            //    data.ImageUrl = newFileName;
            //}

            if (!ModelState.IsValid) return View(vm);

            data.Name = vm.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Categories.FindAsync(id);

            if (data is null) return NotFound();
            

            //string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "sliders", data.ImageUrl);

            //if (System.IO.File.Exists(oldFilePath))
            //{
            //    System.IO.File.Delete(oldFilePath);
            //}

            _context.Categories.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
