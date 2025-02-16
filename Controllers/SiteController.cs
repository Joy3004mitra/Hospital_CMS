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
    public class SiteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor injection for ApplicationDbContext and IWebHostEnvironment
        public SiteController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Index - View all doctors
        public IActionResult Create()
        {
            var site = _context.HeadSetting.FirstOrDefault();
            TempData["SuccessMessage"] = null;
            return View(site);
        }

        // Create - Save new doctor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HeadSetting setting, IFormFile? logoImage, IFormFile? bannerImage1, IFormFile? bannerImage2, IFormFile? bannerImage3, IFormFile? bannerImage4)
        {
            // Handle file upload
            if (logoImage != null && logoImage.Length > 0)
            {
                // Get file name and define the path
                string fileName = Path.GetFileName(logoImage.FileName);

                // Define the path in wwwroot
                string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "LogoImages");

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
                    await logoImage.CopyToAsync(stream);
                }

                // Save the relative file path in the doctor's record
                setting.LogoImage = "/Content/LogoImages/" + fileName;
            }

            if (bannerImage1 != null && bannerImage1.Length > 0)
            {
                // Get file name and define the path
                string fileName = Path.GetFileName(bannerImage1.FileName);

                // Define the path in wwwroot
                string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "BannerImages");

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
                    await bannerImage1.CopyToAsync(stream);
                }

                // Save the relative file path in the doctor's record
                setting.BannerImage1 = "/Content/BannerImages/" + fileName;
            }

            if (bannerImage2 != null && bannerImage2.Length > 0)
            {
                // Get file name and define the path
                string fileName = Path.GetFileName(bannerImage2.FileName);

                // Define the path in wwwroot
                string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "BannerImages");

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
                    await bannerImage2.CopyToAsync(stream);
                }

                // Save the relative file path in the doctor's record
                setting.BannerImage2 = "/Content/BannerImages/" + fileName;
            }

            if (bannerImage3 != null && bannerImage3.Length > 0)
            {
                // Get file name and define the path
                string fileName = Path.GetFileName(bannerImage3.FileName);

                // Define the path in wwwroot
                string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "BannerImages");

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
                    await bannerImage3.CopyToAsync(stream);
                }

                // Save the relative file path in the doctor's record
                setting.BannerImage3 = "/Content/BannerImages/" + fileName;
            }

            if (bannerImage4 != null && bannerImage4.Length > 0)
            {
                // Get file name and define the path
                string fileName = Path.GetFileName(bannerImage4.FileName);

                // Define the path in wwwroot
                string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "BannerImages");

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
                    await bannerImage4.CopyToAsync(stream);
                }

                // Save the relative file path in the doctor's record
                setting.BannerImage4 = "/Content/BannerImages/" + fileName;
            }

            var existingSetting = _context.HeadSetting.FirstOrDefault();
            if (existingSetting != null)
            {
                existingSetting.EditDate = DateTime.Now;
                existingSetting.EditTime = DateTime.Now.ToLocalTime();
                existingSetting.EmailId = setting.EmailId;
                existingSetting.PhoneNo = setting.PhoneNo;
                existingSetting.EmgncyPhoneNo = setting.EmgncyPhoneNo;
                existingSetting.SiteAddress = setting.SiteAddress;
                existingSetting.BannerLink1 = setting.BannerLink1;
                existingSetting.BannerLink2 = setting.BannerLink2;
                existingSetting.BannerLink3 = setting.BannerLink3;
                existingSetting.BannerLink4 = setting.BannerLink4;
                if (logoImage != null && logoImage.Length > 0)
                {
                    existingSetting.LogoImage = setting.LogoImage;
                }
                if (bannerImage1 != null && bannerImage1.Length > 0)
                {
                    existingSetting.BannerImage1 = setting.BannerImage1;
                }
                if (bannerImage2 != null && bannerImage2.Length > 0)
                {
                    existingSetting.BannerImage2 = setting.BannerImage2;
                }
                if (bannerImage3 != null && bannerImage3.Length > 0)
                {
                    existingSetting.BannerImage3 = setting.BannerImage3;
                }
                if (bannerImage4 != null && bannerImage4.Length > 0)
                {
                    existingSetting.BannerImage4 = setting.BannerImage4;
                }
                _context.Update(existingSetting);
            }
            else
            {
                setting.EntDate = DateTime.Now;
                setting.EntTime = DateTime.Now.ToLocalTime();
                setting.TagActive = 0;
                setting.TagDelete = 0;
                // Save doctor info to the database
                _context.HeadSetting.Add(setting);
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Site settings saved successfully!";

            return View(setting);
        }
    }
}
