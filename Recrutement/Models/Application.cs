using System.ComponentModel.DataAnnotations;

namespace Recrutement.Models
{
    public class Application
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [Phone]
        public string phone { get; set; }
        [Required]
        public string educationLevel { get; set; }
        [Required]
        [Range(0,25)]
        public int yearsOfExperience { get; set; }
        [Required]
        public string lastEmployer { get; set; }
        [Required]
        public byte[] resume { get; set; }
    }
}
