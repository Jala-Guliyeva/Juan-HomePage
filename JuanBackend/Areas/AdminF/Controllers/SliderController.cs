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
    public class SliderController : Controller
    {
        private AppDbContext _context;

        private IWebHostEnvironment _env;
        public SliderController(AppDbContext context,
        IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //jsda yazsaqbunusilmeliyik,yoxsaislemiir
        public async Task<IActionResult> Create(Slider slider)
        {
            //validationstate-requiredolanlar
            if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return View();

            }

            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Accept only image!");

                return View();
            }
            if (slider.Photo.ImageSize(10000))
            {
                ModelState.AddModelError("Photo", "1mq yuxari olabilmez!");

                return View();
            }
            //string path = @"C:\Users\TOSHIBA\Desktop\FiorelloAdminF\FiorelloTask\wwwroot\img\";

            string fileName = await slider.Photo.SaveImage(_env, "img");
            Slider newSlider = new Slider();
            newSlider.Image = fileName;
            await _context.Sliders.AddAsync(newSlider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Slider dbSlider = await _context.Sliders.FindAsync(id);
            if (dbSlider == null) return NotFound();
            return View(dbSlider);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Slider dbSlider = await _context.Sliders.FindAsync(id);
            if (dbSlider == null) return NotFound();
            Helper.DeleteFile(_env, "img", dbSlider.Image);
            _context.Sliders.Remove(dbSlider);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            Slider dbSlider = await _context.Sliders.FindAsync(id);
            if (dbSlider == null) return NotFound();

            return View(dbSlider);



        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,  Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Slider dbSlider = await _context.Sliders.FindAsync(id);
           
            if (dbSlider == null) return NotFound();
            dbSlider.Title = slider.Title;
            dbSlider.Description = slider.Description;
            dbSlider.SubTitle = slider.SubTitle;
            dbSlider.Image = slider.Image;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
