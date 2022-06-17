using FiorelloTask.Extentions;
using FiorelloTask.Helpers;
using JuanBackend.DAL;
using JuanBackend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackend.Areas.AdminF.Controllers
{
    [Area("AdminF")]
    public class ProductController : Controller
    {
        private AppDbContext _context;

        private IWebHostEnvironment _env;
        public ProductController(AppDbContext context,
        IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //jsda yazsaqbunusilmeliyik,yoxsaislemiir
        public async Task<IActionResult> Create(Product product)
        {
            //validationstate-requiredolanlar
            if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return View();

            }

            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Accept only image!");

                return View();
            }
            if (product.Photo.ImageSize(10000))
            {
                ModelState.AddModelError("Photo", "1mq yuxari olabilmez!");

                return View();
            }
            //string path = @"C:\Users\TOSHIBA\Desktop\FiorelloAdminF\FiorelloTask\wwwroot\img\";

            string fileName = await product.Photo.SaveImage(_env, "img");
            Product newProduct = new Product();
            newProduct.Image = fileName;
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();
            return View(dbProduct);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();
            Helper.DeleteFile(_env, "img", dbProduct.Image);
            _context.Products.Remove(dbProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();

            return View(dbProduct);



        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Product dbProduct = await _context.Products.FindAsync(id);

            if (dbProduct == null) return NotFound();
            dbProduct.Title = product.Title;
            dbProduct.Price = product.Price;
            dbProduct.DisCountPrice = product.DisCountPrice;
            dbProduct.Image = product.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
