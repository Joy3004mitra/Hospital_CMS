using HospitalManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TestimonialController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        string defaultImagePath = "/images/review-author-1.jpg";

        // Index - View all Host testimonial
        public IActionResult Index()
        {
            var testimonialList = _context.MastHosTestimonials.Where(d => d.TagDelete == 0).ToList();
            foreach (var testimonial in testimonialList)
            {
                if (string.IsNullOrEmpty(testimonial.PatientImage) ||
                    !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", testimonial.PatientImage.TrimStart('/'))))
                {
                    testimonial.PatientImage = defaultImagePath;
                }
            }
            return View(testimonialList);
        }

        // Create - Display form for new doctor
        public IActionResult Create()
        {
            TempData["SuccessMessage"] = null;
            return View();
        }

        // Create - Save new doctor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MastHosTestimonials testimonial, IFormFile patientImage)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (patientImage != null && patientImage.Length > 0)
                {
                    // Get file name and define the path
                    string fileName = Path.GetFileName(patientImage.FileName);

                    // Define the path in wwwroot
                    string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "PatientImages");

                    // Ensure directory exists
                    if (!Directory.Exists(uploadsDirectory))
                    {
                        Directory.CreateDirectory(uploadsDirectory);
                    }

                    // Define the file path
                    string filePath = Path.Combine(uploadsDirectory, fileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await patientImage.CopyToAsync(stream);
                    }

                    // Save the relative file path in the testimonial's record
                    testimonial.PatientImage = "/Content/PatientImages/" + fileName;
                }
                testimonial.EntDate = DateTime.Now;
                testimonial.EntTime = DateTime.Now.ToLocalTime();
                testimonial.TagActive = 0;
                testimonial.TagDelete = 0;
                // Save testimonial info to the database
                _context.MastHosTestimonials.Add(testimonial);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Testimonial updated successfully!";

                //return RedirectToAction("Index");
            }

            return View(testimonial);
        }

        // Edit - Show the edit form for a doctor
        public IActionResult Edit(int id)
        {
            var testimonial = _context.MastHosTestimonials.FirstOrDefault(d => d.MastHosTestimonialsKey == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(testimonial.PatientImage) ||
                !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", testimonial.PatientImage.TrimStart('/'))))
            {
                testimonial.PatientImage = defaultImagePath;
            }
            TempData["SuccessMessage"] = null;
            return View(testimonial);
        }

        // Edit - Save edited doctor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MastHosTestimonials testimonial, IFormFile patientImage)
        {
            var existingtestimonial = _context.MastHosTestimonials.FirstOrDefault(x => x.MastHosTestimonialsKey == id);
            if (existingtestimonial != null)
            {
                existingtestimonial.PatientName = testimonial.PatientName;
                existingtestimonial.PatientComments = testimonial.PatientComments;
            }
            // Handle file upload if a new image is selected
            if (patientImage != null && patientImage.Length > 0)
            {
                // Get file name and define the path
                string fileName = Path.GetFileName(patientImage.FileName);

                // Define the path in wwwroot
                string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "PatientImages");

                // Ensure directory exists
                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                // Define the file path
                string filePath = Path.Combine(uploadsDirectory, fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await patientImage.CopyToAsync(stream);
                }

                // Update the testimonial's image path
                existingtestimonial.PatientImage = "/Content/PatientImages/" + fileName;
            }

            existingtestimonial.EditDate = DateTime.Now;
            existingtestimonial.EditTime = DateTime.Now.ToLocalTime();
            testimonial.PatientImage = existingtestimonial.PatientImage;

            _context.Update(existingtestimonial);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Testimonial updated successfully!";
            return View(testimonial);
        }

        // Delete - Perform the deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var testimonial = _context.MastHosTestimonials.FirstOrDefault(d => d.MastHosTestimonialsKey == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            // Mark the doctor as deleted by setting TagDelete to 1
            testimonial.TagDelete = 1;
            _context.Update(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
