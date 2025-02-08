using HospitalManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor injection for ApplicationDbContext and IWebHostEnvironment
        public ServiceController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Index - View all Host service
        public IActionResult Index()
        {
            var services = _context.MastHosServices.Where(d => d.TagDelete == 0).ToList();
            return View(services);
        }

        // Create - Display form for new service
        public IActionResult Create()
        {
            TempData["SuccessMessage"] = null;
            return View();
        }

        // Create - Save new host service
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MastHosService service, IFormFile serviceImage)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (serviceImage != null && serviceImage.Length > 0)
                {
                    // Get file name and define the path
                    string fileName = Path.GetFileName(serviceImage.FileName);

                    // Define the path in wwwroot
                    string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "ServiceImages");

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
                        await serviceImage.CopyToAsync(stream);
                    }

                    // Save the relative file path in the service's record
                    service.ServiceImage = "/Content/ServiceImages/" + fileName;
                }
                service.EntDate = DateTime.Now;
                service.EntTime = DateTime.Now.ToLocalTime();
                service.TagActive = 0;
                service.TagDelete = 0;
                // Save service info to the database
                _context.MastHosServices.Add(service);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Service saved successfully!";
                //return RedirectToAction("Index");
            }

            return View(service);
        }

        // Edit - Show the edit form for a service
        public IActionResult Edit(int id)
        {
            var service = _context.MastHosServices.FirstOrDefault(d => d.MastHosServiceKey == id);
            if (service == null)
            {
                return NotFound();
            }
            TempData["SuccessMessage"] = null;
            return View(service);
        }

        // Edit - Save edited service
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MastHosService service, IFormFile serviceImage)
        {

            if (ModelState.IsValid)
            {
                var existingservice = _context.MastHosServices.FirstOrDefault(x => x.MastHosServiceKey == id);
                if (existingservice != null)
                {
                    existingservice.ServiceName = service.ServiceName;
                    existingservice.ServiceShortDesc = service.ServiceShortDesc;
                }
                // Handle file upload if a new image is selected
                if (serviceImage != null && serviceImage.Length > 0)
                {
                    // Get file name and define the path
                    string fileName = Path.GetFileName(serviceImage.FileName);

                    // Define the path in wwwroot
                    string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "ServiceImages");

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
                        await serviceImage.CopyToAsync(stream);
                    }

                    // Update the service's image path
                    existingservice.ServiceImage = "/Content/ServiceImages/" + fileName;
                }

                existingservice.EditDate = DateTime.Now;
                existingservice.EditTime = DateTime.Now.ToLocalTime();
                service.ServiceImage = existingservice.ServiceImage;

                _context.Update(existingservice);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Service updated successfully!";
            }
            return View(service);
        }

        // Delete - Show delete confirmation page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = _context.MastHosServices.FirstOrDefault(d => d.MastHosServiceKey == id);
            if (service == null)
            {
                return NotFound();
            }

            // Mark the doctor as deleted by setting TagDelete to 1
            service.TagDelete = 1;
            _context.Update(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
