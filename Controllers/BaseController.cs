using HospitalManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HospitalManagement.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _context;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public void LoadSiteSettings()
        {
            var siteDetails = _context.HeadSetting.FirstOrDefault();
            ViewBag.SiteDetails = siteDetails;
        }

        // Ensure site settings are loaded before each action
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            LoadSiteSettings();
            base.OnActionExecuting(context);
        }
    }
}
