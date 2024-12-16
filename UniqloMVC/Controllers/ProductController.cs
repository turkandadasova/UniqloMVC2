using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UniqloMVC.DataAccess;
using UniqloMVC.Models;
using UniqloMVC.ViewModels.Comment;
using UniqloMVC.ViewModels.Product;
namespace UniqloMVC.Controllers
{
    public class ProductController(UniqloDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {

            if (!id.HasValue) return BadRequest();
            var data = await _context.Products
               .Where(x => x.Id == id) 
               .Include(x => x.Ratings)
               .Include(x => x.Comments)
               .ThenInclude(c => c.User)
               .FirstOrDefaultAsync();
            if (data == null) return NotFound();
            ProductDetailsVM vm = new ProductDetailsVM
            {
                Name = data.Name,
                Description = data.Description,
                SellPrice = data.SellPrice,
                Discount = data.Discount,
                CoverFile = data.CoverImage,
            };
            ViewBag.Rating = 5;
            if (User.Identity?.IsAuthenticated ?? false)
            {
                string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

                int rating = await _context.ProductRating.Where(x => x.UserId == userId && x.ProductId == id).Select(x => x.Raiting).FirstOrDefaultAsync();
                ViewBag.Rating = rating == 0 ? 5 : rating;
            }
            return View(data);
        }

        public async Task<IActionResult> Rating(int productId, int rating)
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var data = await _context.ProductRating.Where(x => x.UserId == userId && x.ProductId == productId).FirstOrDefaultAsync();

            if (data is null)
            {
                await _context.ProductRating.AddAsync(new Models.ProductRating
                {
                    UserId = userId,
                    ProductId = productId,
                    Raiting = rating
                });
            }
            else
            {
                data.Raiting = rating;
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { Id = productId });
        }

        public async Task<IActionResult> Comment(int productId, CommentCreateVM vm)
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

            var data = await _context.Comments.Where(x => x.UserId == userId && x.ProductId == productId).FirstOrDefaultAsync();


            Comment comment = new Comment
            {
                Comments = vm.Content,
                ProductId = productId,
                UserId = userId,
            };

            await _context.Comments.AddAsync(comment);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { Id = productId });
        }

    }
}
