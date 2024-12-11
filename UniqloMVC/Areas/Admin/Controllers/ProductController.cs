using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqloMVC.DataAccess;
using UniqloMVC.Extensions;
using UniqloMVC.Helpers;
using UniqloMVC.Models;
using UniqloMVC.ViewModels.Product;
using UniqloMVC.ViewModels.Slider;

namespace UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Product)]
    public class ProductController(IWebHostEnvironment _env, UniqloDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(x => x.Category).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (vm.CoverFile != null)
            {
                if (!vm.CoverFile.ContentType.StartsWith("image"))
                    ModelState.AddModelError("File", "File type must be an image");
                if (vm.CoverFile.Length > 5 * 1024 * 1024)
                    ModelState.AddModelError("File", "File must be less than 5mb");

            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
                return View();
            }
            Product product = vm;
            product.CoverImage = await vm.CoverFile!.UploadAsync(_env.WebRootPath, "imgs", "products");

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {

            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            if (!id.HasValue) return BadRequest();
            var data = await _context.Products
                .Where(p => p.Id == id.Value)
                .Select(x => new ProductUpdateVM
                {
                    Name = x.Name,
                    Description = x.Description,
                    CostPrice = x.CostPrice,
                    SellPrice = x.SellPrice,
                    Discount = x.Discount,
                    Quantity = x.Quantity,
                    CategoryId = x.CategoryId,
                    CoverFileUrl = x.CoverImage,                    
                }).FirstOrDefaultAsync();
            if (data is null) return NotFound();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        {
            if (!id.HasValue) return BadRequest();
            if (vm.CoverFile != null)
            {
                if (!vm.CoverFile.ContentType.StartsWith("image"))
                    ModelState.AddModelError("File", "File type must be an image");
                if (vm.CoverFile.Length > 5 * 1024 * 1024)
                    ModelState.AddModelError("File", "File must be less than 5mb");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
                return View(vm);
            }
            var products = await _context.Products
                .Where(p => p.Id == id.Value)
                .FirstOrDefaultAsync();
            if (products is null) return NotFound();
            if (vm.CoverFile != null)
            {
                string path = Path.Combine(_env.WebRootPath, "img", "products", products.CoverImage);
                using (Stream sr = System.IO.File.Create(path))
                {
                    await vm.CoverFile!.CopyToAsync(sr);
                }
            }

            products.Name = vm.Name;
            products.Description = vm.Description;
            products.CostPrice = vm.CostPrice;
            products.SellPrice = vm.SellPrice;
            products.Quantity = vm.Quantity;
            products.Discount = vm.Discount;
            products.CategoryId = vm.CategoryId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Products.FindAsync(id);

            if (data is null) return NotFound();

            string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "products", data.CoverImage);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            _context.Products.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
