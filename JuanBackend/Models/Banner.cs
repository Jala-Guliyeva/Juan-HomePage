using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanBackend.Models
{
    public class Banner
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }
}
