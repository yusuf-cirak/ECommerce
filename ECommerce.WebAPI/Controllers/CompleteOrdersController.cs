using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers;

public class CompleteOrdersController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}