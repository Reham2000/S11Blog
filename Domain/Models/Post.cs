using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage ="maxlength is 100"),MinLength(2, ErrorMessage = "minlength is 2")]
        public string Title { get; set; }
        [MinLength(10, ErrorMessage = "minlength is 10")]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }
}
