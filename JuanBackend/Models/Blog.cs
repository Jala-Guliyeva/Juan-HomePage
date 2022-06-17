using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanBackend.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
        public string Desc { get; set; }
        public string Author { get; set; }
        public DateTime WriteTime { get; set; }
    }
}
