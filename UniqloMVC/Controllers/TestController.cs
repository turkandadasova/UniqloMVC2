using Microsoft.AspNetCore.Mvc;

namespace UniqloMVC.Controllers
{
    public class TestController : Controller
    {
        public IActionResult SetSession(string key,string value)
        {
            HttpContext.Session.SetString(key, value);
            return Ok();
        }

        public async Task<IActionResult> GetSession(string key)
        {
          //  HttpContext.Session.Remove(key);
           // HttpContext.Session.Clear();
            return Content(HttpContext.Session.GetString(key));
        }

        public async Task<IActionResult> SetCookie(string key, string value)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                // Expires=new DateTime(2024,12,31,23,59,59)
                MaxAge = TimeSpan.FromMinutes(5)
            });
            return Ok();
        }

        public async Task<IActionResult> GetCookie(string key)
        {
           // HttpContext.Response.Cookies.Delete(key);
            
            return Content(HttpContext.Request.Cookies[key]);
        }
    }
}
