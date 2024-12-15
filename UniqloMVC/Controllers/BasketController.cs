using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UniqloMVC.DataAccess;

namespace UniqloMVC.Controllers
{
    public class BasketController(UniqloDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Add(int id)
        {
            //if(!await _context.Products.AnyAsync(x=>x.Id==id))
            //    return NotFound();
           var basketItems = JsonSerializer.Deserialize<List<int>>(Request.Cookies["basket"] ?? "[]");
            basketItems.Add(id);
            Response.Cookies.Append("basket",JsonSerializer.Serialize(basketItems));
            return Ok();
           
        }

    }
}
