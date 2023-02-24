using Microsoft.AspNetCore.Http;
using Recrutement.Models;
using System.ComponentModel.DataAnnotations;

namespace Recrutement.ViewModels
{
    public class NewApplication
    {
        [Required]
        public Application application { get; set; }
        [Required]
        public IFormFile ResumeFile { get; set; }
    }
}
