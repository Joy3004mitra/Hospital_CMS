using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    public class HomeController : BaseController
    {
        private readonly EmailService _emailService;
        private readonly ILogger<HomeController> _logger;
        string defaultImagePath = "/images/review-author-1.jpg";
        string serviceImagePath = "/images/services-section.jpg";

        public HomeController(EmailService emailService, ApplicationDbContext context, ILogger<HomeController> logger)
            : base(context) // Call BaseController constructor
        {
            _emailService = emailService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                ViewBag.SiteDetails = _context.HeadSetting.FirstOrDefault();
                var doctorList = _context.MastDoctors.Where(d => d.TagDelete == 0).ToList();
                foreach (var doctor in doctorList)
                {
                    if (string.IsNullOrEmpty(doctor.DoctorImage) ||
                        !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", doctor.DoctorImage.TrimStart('/'))))
                    {
                        doctor.DoctorImage = defaultImagePath;
                    }
                }
                ViewBag.DoctorList = doctorList;
                var serviceList = _context.MastHosServices.Where(d => d.TagDelete == 0).OrderByDescending(d => d.MastHosServiceKey).Take(4).ToList();
                foreach (var service in serviceList)
                {
                    if (string.IsNullOrEmpty(service.ServiceImage) ||
                        !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", service.ServiceImage.TrimStart('/'))))
                    {
                        service.ServiceImage = serviceImagePath;
                    }
                }
                ViewBag.ServiceList = serviceList;

                var testimonialList = _context.MastHosTestimonials.Where(d => d.TagDelete == 0).ToList();
                foreach (var testimonial in testimonialList)
                {
                    if (string.IsNullOrEmpty(testimonial.PatientImage) ||
                        !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", testimonial.PatientImage.TrimStart('/'))))
                    {
                        testimonial.PatientImage = defaultImagePath;
                    }
                }
                ViewBag.TestimonialList = testimonialList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading homepage data");
                ViewBag.Error = "Failed to load data.";
            }
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Service()
        {
            var serviceList = _context.MastHosServices.Where(d => d.TagDelete == 0).ToList();
            foreach (var service in serviceList)
            {
                if (string.IsNullOrEmpty(service.ServiceImage) ||
                    !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", service.ServiceImage.TrimStart('/'))))
                {
                    service.ServiceImage = serviceImagePath;
                }
            }
            return View(serviceList);
        }

        public IActionResult Doctor()
        {
            var doctorList = _context.MastDoctors.Where(d => d.TagDelete == 0).ToList();
            foreach (var doctor in doctorList)
            {
                if (string.IsNullOrEmpty(doctor.DoctorImage) ||
                    !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", doctor.DoctorImage.TrimStart('/'))))
                {
                    doctor.DoctorImage = defaultImagePath;
                }
            }
            return View(doctorList);
        }

        public IActionResult ContactUs()
        {
            ContactModel contactModel = new ContactModel();
            if (ViewBag.SiteDetails != null)
            {
                contactModel.SiteEmail = ViewBag.SiteDetails.EmailId;
                contactModel.SiteAddress = ViewBag.SiteDetails.SiteAddress;
                contactModel.SitePhoneNo = ViewBag.SiteDetails.PhoneNo;
                contactModel.SiteEmergencyNo = ViewBag.SiteDetails.EmgncyPhoneNo;
            }
            TempData["SuccessMessage"] = null;
            return View(contactModel);
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactModel contactModel)
        {
            if (string.IsNullOrWhiteSpace(contactModel.Email) || string.IsNullOrWhiteSpace(contactModel.Message))
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View(contactModel);
            }

            bool emailSent = await _emailService.SendContactMailAsync(contactModel);
            if (emailSent)
            {
                TempData["SuccessMessage"] = "Message sent successfully!";
            }
            else
            {
                ViewBag.Error = "Failed to send email. Please try again later.";
            }

            return View(contactModel);
        }

        public IActionResult BookingAppointment()
        {
            ViewBag.Doctor = _context.MastDoctors.Where(x => x.TagDelete == 0).ToList();
            ViewBag.Service = _context.MastHosServices.Where(x => x.TagDelete == 0).ToList();

            TempData["SuccessMessage"] = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookingAppointmentSubmit([FromBody] AppointmentModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.FullName))
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            bool emailSent = await _emailService.SendBookingMailAsync(model);

            if (emailSent)
            {
                var appointmentHistory = new AppointmentHistory
                {
                    ServiceName = model.ServiceName,
                    DoctorName = model.DoctorName,
                    PatientStatus = model.PatientStatus,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    Age = model.Age,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    AppointmentDate = Convert.ToDateTime(model.AppointmentDate),
                    Message = model.Message,
                    EntDate = DateTime.Now,
                    EntTime = DateTime.Now.ToLocalTime(),
                    TagActive = 1,
                    TagDelete = 0
                };

                _context.AppointmentHistories.Add(appointmentHistory);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving appointment history");
                    return Json(new { success = false, message = "Failed to save booking details." });
                }

                TempData["SuccessMessage"] = "Booking appointment done successfully!";
                return Json(new { success = true, AppointmentDate = appointmentHistory.AppointmentDate });
            }
            else
            {
                return Json(new { success = false, message = "Failed to send email." });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
