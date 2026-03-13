using Microsoft.AspNetCore.Mvc;

namespace TrainingCentreManagement.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}