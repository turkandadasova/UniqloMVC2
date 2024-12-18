using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Text.Json;
using UniqloMVC.DataAccess;
using UniqloMVC.ViewModels.Basket;

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
            var basketItems = JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["basket"] ?? "[]");
            var item = basketItems!.FirstOrDefault(x=>x.Id==id);
            if (item==null)
            { 
                item = new BasketProductItemVM(id);
                basketItems!.Add(item);
            }
            item.Count++;
            Response.Cookies.Append("basket",JsonSerializer.Serialize(basketItems));
            return Ok();
           
        }


        public async Task<IActionResult> Delete(int id)
        {
            var basketItems = JsonSerializer.Deserialize<List<BasketProductItemVM>>(Request.Cookies["basket"] ?? "[]");
            var item = basketItems!.FirstOrDefault(x => x.Id == id);
            if (item!.Count > 1)
            {
                item.Count--;   
            }
            else
            {
                basketItems!.Remove(item);
            }
            Response.Cookies.Append("basket", JsonSerializer.Serialize(basketItems));
            return RedirectToAction("Index", "Home");
        }

    }
}
