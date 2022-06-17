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
    public class BlogController : Controller
    {
        private AppDbContext _context;

        private IWebHostEnvironment _env;
        public BlogController(AppDbContext context,
        IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public IActionResult Index()
        {
            List<Blog> blogs = _context.Blogs.ToList();
            return View(blogs);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //jsda yazsaqbunusilmeliyik,yoxsaislemiir
        public async Task<IActionResult> Create(Blog blog)
        {
            //validationstate-requiredolanlar
            if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return View();

            }

            if (!blog.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Accept only image!");

                return View();
            }
            if (blog.Photo.ImageSize(10000))
            {
                ModelState.AddModelError("Photo", "1mq yuxari olabilmez!");

                return View();
            }
            //string path = @"C:\Users\TOSHIBA\Desktop\FiorelloAdminF\FiorelloTask\wwwroot\img\";

            string fileName = await blog.Photo.SaveImage(_env, "assets/img");
            Blog newBlog = new Blog();
            newBlog.Image = fileName;
            await _context.Blogs.AddAsync(newBlog);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Blog dbBlog = await _context.Blogs.FindAsync(id);
            if (dbBlog == null) return NotFound();
            return View(dbBlog);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Blog dbBlog = await _context.Blogs.FindAsync(id);
            if (dbBlog == null) return NotFound();
            Helper.DeleteFile(_env, "img", dbBlog.Image);
            _context.Blogs.Remove(dbBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            Blog dbBlog = await _context.Blogs.FindAsync(id);
            if (dbBlog == null) return NotFound();

            return View(dbBlog);



        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Blog dbBlog = await _context.Blogs.FindAsync(id);

            if (dbBlog == null) return NotFound();
            dbBlog.Author = blog.Author;
            dbBlog.WriteTime = blog.WriteTime;
            dbBlog.Image = blog.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
