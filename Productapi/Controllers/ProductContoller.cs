using Microsoft.AspNetCore.Mvc;
using Productapi.Model;

namespace Productapi.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var product = new Product
            {
                Id = 1,
                Name = "TV",
                Price = 50000
            };

            return View(product);
        }
    }
}