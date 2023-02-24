using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recrutement.Models;
using Recrutement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Recrutement.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly DBContext _context;

        public ApplicationsController(DBContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Applications.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> ApplicationDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FirstOrDefaultAsync(m => m.id == id);
            
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Apply(NewApplication newApplication)
        {
            Application application = newApplication.application;

            if (newApplication.application == null || newApplication.ResumeFile == null)
            {
                ModelState.AddModelError("Application", "Complete your infos!");
            }

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

            string folderPath = projectDirectory + "/Applications" + "/" + application.firstName + application.lastName;
            string filePath = folderPath + "/resume.pdf";
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    newApplication.ResumeFile.CopyTo(fs);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
                    

            using (var memoryStream = new MemoryStream())
            {
                await newApplication.ResumeFile.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    application.resume = memoryStream.ToArray();
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
            }

            try
            {
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.id == id);
        }
    }
}
