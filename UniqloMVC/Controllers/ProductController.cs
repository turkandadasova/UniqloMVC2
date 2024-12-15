using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UniqloMVC.DataAccess;
using UniqloMVC.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var data = await _context.Products.Where(x => x.Id == id.Value && !x.IsDeleted)
                .Include(x=>x.Ratings)
                .ThenInclude(x=>x.User)
                .FirstOrDefaultAsync();
            if (data is null) return NotFound();
            ViewBag.Rating = 5;
            if(User.Identity?.IsAuthenticated ?? false)
            {
                string userId = User.Claims.FirstOrDefault(x=> x.Type == ClaimTypes.NameIdentifier)?.Value;
              int rating=  await _context.ProductRating.Where(x => x.UserId == userId && x.ProductId == id)
                    .Select(x => x.Raiting).FirstOrDefaultAsync(); 
                ViewBag.Rating = rating==0?5:rating;
            }
            return View(data);
        }

        public async Task<IActionResult> Rating(int productId,int rating)
        {
            string userId=User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier)!.Value;
            var data =await _context.ProductRating.Where(x => x.UserId == userId && x.ProductId == productId)
                .FirstOrDefaultAsync();
            if (data is null)
            {
                await _context.ProductRating.AddAsync(new Models.ProductRating
                {
                    ProductId = productId,
                    UserId = userId,
                    Raiting = rating
                });
            }
            else
            {
                data.Raiting = rating;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details),new {Id=productId});
        }

        //public IActionResult Comment()
        //{
    


       [HttpPost]
       public async Task<IActionResult> CreateComment(int productId, string comment, string userId)
       {
         var product = await _context.ProductComment.Where(x => x.UserId == userId && x.ProductId == productId)
         .FirstOrDefaultAsync();
            //var product = products.FirstOrDefault(p => p.Id == productId);
          if (product == null)
            return NotFound();
            if (product is null)
            {
                await _context.ProductComment.AddAsync(new Models.ProductComment
                {
                    ProductId = productId,
                    UserId = userId,
                    Comment = comment,
                });
            }
            else
            {
               product.Comment = comment;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { Id = productId });


            //var comment = new Comment
            //{
            //    Id = new Random().Next(1000),
            //    ProductId = productId,
            //    Content = content,
            //    Author = author
            //};

            //product.Comments.Add(comment);

            //return RedirectToAction("Details", new { id = productId });
        }
        

        //    return View();
        //}


    }
}
