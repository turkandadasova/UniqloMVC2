using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniqloMVC.DataAccess;
using UniqloMVC.Extensions;
using UniqloMVC.Helpers;
using UniqloMVC.Models;
using UniqloMVC.ViewModels.Slider;

namespace UniqloMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Slider)]
    public class SliderController(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (vm.File != null)
            {
                if (!vm.File.IsValidType("image"))
                    ModelState.AddModelError("File", "File must be image!");
                if (!vm.File.IsValidSize(5 * 1024))
                    ModelState.AddModelError("File", "File length must be less than 2mg");
            }
           
            if (!ModelState.IsValid) return View();

            string newFileName = await vm.File.UploadAsync(_env.WebRootPath, "imgs", "sliders");


            Slider slider = new Slider
            {
                ImageUrl = newFileName,
                Title = vm.Title,
                Subtitle = vm.Subtitle,
                Link = vm.Link,
            };

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {

            if (!id.HasValue) return BadRequest();

            var data = await _context.Sliders.FindAsync(id);

            if(data is null) return NotFound();

            SliderUpdateVM vm = new();

            vm.Title = data.Title;
            vm.Subtitle = data.Subtitle;
            vm.Link = data.Link;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id,SliderUpdateVM vm)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Sliders.FindAsync(id);

            if (data is null) return NotFound();


            if (vm.File != null)
            {
                if (!vm.File.IsValidType("image"))
                    ModelState.AddModelError("File", "File must be image!");
                if (!vm.File.IsValidSize(5 * 1024))
                    ModelState.AddModelError("File", "File length must be less than 2mg");

                string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "sliders", data.ImageUrl);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                string newFileName = await vm.File.UploadAsync(_env.WebRootPath, "imgs", "sliders");
                data.ImageUrl = newFileName;
            }

            if (!ModelState.IsValid) return View(vm);

            data.Title = vm.Title;
            data.Subtitle = vm.Subtitle;
            data.Link = vm.Link;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(!id.HasValue) return BadRequest();

            var data = await _context.Sliders.FindAsync(id);

            if (data is null) return NotFound();

            string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "sliders", data.ImageUrl);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            _context.Sliders.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Hide(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Sliders.FindAsync(id);

            if (data is null) return NotFound();

            data.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(int? id)
        {
            if (!id.HasValue) return BadRequest();

            var data = await _context.Sliders.FindAsync(id);

            if (data is null) return NotFound();

            data.IsDeleted = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Select()
        {
            return View(await _context.Sliders.ToListAsync());
        }

    }
}
