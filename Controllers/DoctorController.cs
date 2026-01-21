using HospitalManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor injection for ApplicationDbContext and IWebHostEnvironment
        public DoctorController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        string defaultImagePath = "/images/review-author-1.png";

        //Index - View all doctors
        [Route("/doctor/list/")]
        public IActionResult Index()
        {
            var doctors = _context.MastDoctors.Where(d => d.TagDelete == 0).ToList();
            foreach (var doctor in doctors)
            {
                if (string.IsNullOrEmpty(doctor.DoctorImage) ||
                    !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", doctor.DoctorImage.TrimStart('/'))))
                {
                    doctor.DoctorImage = defaultImagePath;
                }
            }
            return View(doctors);
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
        public async Task<IActionResult> Create(MastDoctor doctor, IFormFile doctorImage)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (doctorImage != null && doctorImage.Length > 0)
                {
                    // Get file name and define the path
                    string fileName = Path.GetFileName(doctorImage.FileName);

                    // Define the path in wwwroot
                    string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "DoctorImages");

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
                        await doctorImage.CopyToAsync(stream);
                    }

                    // Save the relative file path in the doctor's record
                    doctor.DoctorImage = "/Content/DoctorImages/" + fileName;
                }
                doctor.EntDate = DateTime.Now;
                doctor.EntTime = DateTime.Now.ToLocalTime();
                doctor.TagActive = 0;
                doctor.TagDelete = 0;
                // Save doctor info to the database
                _context.MastDoctors.Add(doctor);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Doctor saved successfully!";
                //return RedirectToAction("Index");
            }

            return View(doctor);
        }

        // Edit - Show the edit form for a doctor
        public IActionResult Edit(int id)
        {
            var doctor = _context.MastDoctors.FirstOrDefault(d => d.MastDoctorKey == id);
            if (doctor == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(doctor.DoctorImage) ||
                !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", doctor.DoctorImage.TrimStart('/'))))
            {
                doctor.DoctorImage = defaultImagePath;
            }

            TempData["SuccessMessage"] = null;
            return View(doctor);
        }

        // Edit - Save edited doctor
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MastDoctor doctor, IFormFile doctorImage)
        {
            var existingDoctor = _context.MastDoctors.FirstOrDefault(x => x.MastDoctorKey == id);
            if (existingDoctor != null)
            {
                existingDoctor.DoctorName = doctor.DoctorName;
                existingDoctor.DoctorDegree = doctor.DoctorDegree;
                existingDoctor.DoctorDesc = doctor.DoctorDesc;
                existingDoctor.FaceboolProfileLink = doctor.FaceboolProfileLink;
                existingDoctor.LinkedInLink = doctor.LinkedInLink;
                existingDoctor.XLink = doctor.XLink;
            }
            // Handle file upload if a new image is selected
            if (doctorImage != null && doctorImage.Length > 0)
            {
                // Get file name and define the path
                string fileName = Path.GetFileName(doctorImage.FileName);

                // Define the path in wwwroot
                string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "DoctorImages");

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
                    await doctorImage.CopyToAsync(stream);
                }

                // Update the doctor's image path
                existingDoctor.DoctorImage = "/Content/DoctorImages/" + fileName;
            }

            existingDoctor.EditDate = DateTime.Now;
            existingDoctor.EditTime = DateTime.Now.ToLocalTime();
            doctor.DoctorImage = existingDoctor.DoctorImage;

            _context.Update(existingDoctor);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Doctor updated successfully!";
            return View(doctor);
        }

        // Delete - Show delete confirmation page
        //public IActionResult Delete(int id)
        //{
        //    var doctor = _context.MastDoctors.FirstOrDefault(d => d.MastDoctorKey == id);
        //    if (doctor == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(doctor);
        //}

        // Delete - Perform the deletion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = _context.MastDoctors.FirstOrDefault(d => d.MastDoctorKey == id);
            if (doctor == null)
            {
                return NotFound();
            }

            // Mark the doctor as deleted by setting TagDelete to 1
            doctor.TagDelete = 1;
            _context.Update(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
