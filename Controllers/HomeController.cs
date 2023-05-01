using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using kalbestore_fe.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using kalbestore_fe.Utils;

namespace kalbestore_fe.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RegisteredOrder(int id)
    {
        // Get product details from API
        var productDetail = APIHandler.GetProdukById(id).Result;

        // Pass product details to the view
        return View(productDetail);
    }

    public IActionResult UnregisteredOrder(int id)
    {
        // Get product details from API
        var productDetail = APIHandler.GetProdukById(id).Result;

        // Pass product details to the view
        return View(productDetail);
    }

    public IActionResult Produk()
    {
        return View();
    }

    public IActionResult Tes()
    {
        return View();
    }
}

