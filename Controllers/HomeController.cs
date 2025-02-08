using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Numerics;

namespace HospitalManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmailService _emailService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(EmailService emailService, ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _emailService = emailService;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var doctorList = _context.MastDoctors.Where(d => d.TagDelete == 0).ToList();
            ViewBag.DoctorList = doctorList;

            var serviceList = _context.MastHosServices.Where(d => d.TagDelete == 0).ToList();
            ViewBag.ServiceList = serviceList;

            var testimonialList = _context.MastHosTestimonials.Where(d => d.TagDelete == 0).ToList();
            ViewBag.TestimonialList = testimonialList;

            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Service()
        {
            var serviceList = _context.MastHosServices.Where(d => d.TagDelete == 0).ToList();
            return View(serviceList);
        }
        public IActionResult Doctor()
        {
            var doctorList = _context.MastDoctors.Where(d => d.TagDelete == 0).ToList();
            return View(doctorList);
        }

        public IActionResult ContactUs()
        {
            ContactModel contactModel = new ContactModel();
            var headSettings = _context.HeadSetting.FirstOrDefault();
            if (headSettings != null)
            {
                contactModel.SiteEmail = headSettings.EmailId;
                contactModel.SiteAddress = headSettings.SiteAddress;
                contactModel.SitePhoneNo = headSettings.PhoneNo;
                contactModel.SiteEmergencyNo = headSettings.EmgncyPhoneNo;
            }
            return View(contactModel);
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactModel contactModel)
        {
            if (string.IsNullOrWhiteSpace(contactModel.Email) || string.IsNullOrWhiteSpace(contactModel.Message))
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View();
            }

            string emailBody = $"<h3>New Contact Us Form Submission</h3>" +
                               $"<p><strong>Name:</strong> {contactModel.Name}</p>" +
                               $"<p><strong>Email:</strong> {contactModel.Email}</p>" +
                               $"<p><strong>Phone:</strong> {contactModel.PhoneNumber}</p>" +
                               $"<p><strong>Subject:</strong> {contactModel.Subject}</p>" +
                               $"<p><strong>Message:</strong> {contactModel.Message}</p>";

            bool emailSent = await _emailService.SendEmailAsync(contactModel.Email, contactModel.Subject, emailBody);

            if (emailSent)
            {
                ViewBag.Success = "Your message has been sent successfully!";
            }
            else
            {
                ViewBag.Error = "Failed to send email. Please try again later.";
            }

            return View(contactModel);
        }

        public IActionResult BookingAppointment()
        {
            var doctors = _context.MastDoctors.Where(x => x.TagDelete == 0).ToList();
            ViewBag.Doctor = doctors;

            var servies = _context.MastHosServices.Where(x => x.TagDelete == 0).ToList();
            ViewBag.Service = servies;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookingAppointmentSubmit([FromBody] AppointmentModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.FullName))
            {
                return Json(new { success = false, message = "Invalid data." });
            }
            AppointmentHistory appointmentHistory = new AppointmentHistory();

            string emailBody = $@"
                <h3>New Appointment Request</h3>
                <p><strong>Service:</strong> {model.ServiceName}</p>
                <p><strong>Doctor:</strong> {model.DoctorName}</p>
                <p><strong>Patient Status:</strong> {model.PatientStatus}</p>
                <p><strong>Name:</strong> {model.FullName}</p>
                <p><strong>Gender:</strong> {model.Gender}</p>
                <p><strong>Age:</strong> {model.Age}</p>
                <p><strong>Email:</strong> {model.Email}</p>
                <p><strong>Phone:</strong> {model.PhoneNumber}</p>
                <p><strong>Appointment Date:</strong> {model.AppointmentDate}</p>
                <p><strong>Message:</strong> {model.Message}</p>
            ";

            bool emailSent = await _emailService.SendEmailAsync(model.Email, "New Appointment Request", emailBody);

            if (emailSent)
            {
                appointmentHistory.ServiceName = model.ServiceName;
                appointmentHistory.DoctorName = model.DoctorName;
                appointmentHistory.PatientStatus = model.PatientStatus;
                appointmentHistory.FullName = model.FullName;
                appointmentHistory.Gender = model.Gender;
                appointmentHistory.Age = model.Age;
                appointmentHistory.Email = model.Email;
                appointmentHistory.PhoneNumber = model.PhoneNumber;
                appointmentHistory.AppointmentDate = Convert.ToDateTime(model.AppointmentDate);
                appointmentHistory.Message = model.Message;
                appointmentHistory.EntDate = DateTime.Now;
                appointmentHistory.EntTime = DateTime.Now.ToLocalTime();
                appointmentHistory.TagActive = 1;
                appointmentHistory.TagDelete = 0;
                _context.AppointmentHistories.Add(appointmentHistory);
                try
                {
                    await _context.SaveChangesAsync();
                }catch(Exception ex)
                {
                    return Json(new { message=ex.Message.ToString()});
                }
                

                return Json(new { success = true });
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