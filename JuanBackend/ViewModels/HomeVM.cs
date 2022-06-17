using JuanBackend.Models;
using System.Collections;
using System.Collections.Generic;

namespace JuanBackend.ViewModels
{
    public class HomeVM
    {
        public IEnumerable <Slider> sliders  { get; set; }
        public IEnumerable <Service>services { get; set; }
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Banner> banners { get; set; }
        public IEnumerable<Seller>sellers { get; set; }
        public IEnumerable<Blog>blogs { get; set; }
        public IEnumerable<Brand> brands { get; set; }
    }
}
