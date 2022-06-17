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
    public class BannerController : Controller
    {
        private AppDbContext _context;

        private IWebHostEnvironment _env;
        public BannerController(AppDbContext context,
        IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public IActionResult Index()
        {
            List<Banner> banners = _context.Banners.ToList();
            return View(banners);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //jsda yazsaqbunusilmeliyik,yoxsaislemiir
        public async Task<IActionResult> Create(Banner banner)
        {
            //validationstate-requiredolanlar
            if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return View();

            }

            if (!banner.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Accept only image!");

                return View();
            }
            if (banner.Photo.ImageSize(10000))
            {
                ModelState.AddModelError("Photo", "1mq yuxari olabilmez!");

                return View();
            }


            string fileName = await banner.Photo.SaveImage(_env, "img");
            Banner newBanner = new Banner();
            newBanner.Image = fileName;
            await _context.Banners.AddAsync(newBanner);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Banner dbBanner = await _context.Banners.FindAsync(id);
            if (dbBanner == null) return NotFound();
            return View(dbBanner);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Banner dbBanner = await _context.Banners.FindAsync(id);
            if (dbBanner == null) return NotFound();
            Helper.DeleteFile(_env, "img", dbBanner.Image);
            _context.Banners.Remove(dbBanner);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            Banner dbBanner = await _context.Banners.FindAsync(id);
            if (dbBanner == null) return NotFound();

            return View(dbBanner);



        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Banner banner)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Banner dbBanner = await _context.Banners.FindAsync(id);

            if (dbBanner == null) return NotFound();
            dbBanner.Title = banner.Title;
            dbBanner.SubTitle = banner.SubTitle;
            dbBanner.Image = banner.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
