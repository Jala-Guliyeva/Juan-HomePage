using JuanBackend.DAL;
using JuanBackend.Models;
using JuanBackend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace JuanBackend.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            homeVM.sliders= _context.Sliders.ToList();
            homeVM.services= _context.Services.ToList();
            homeVM.products = _context.Products.ToList();
            homeVM.banners = _context.Banners.ToList();
            homeVM.sellers = _context.Sellers.ToList();
            homeVM.blogs = _context.Blogs.ToList();
            homeVM.brands = _context.Brands.ToList();

            return View(homeVM);
        }
    }
}
