using System.ComponentModel.DataAnnotations;

namespace Recrutement.Models
{
    public class Admin
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
