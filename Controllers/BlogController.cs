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
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        string defaultImagePath = "/images/review-author-1.jpg";

        public BlogController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Index - List all blogs
        [Route("/blog/list/")]
        public IActionResult Index()
        {
            var blogs = _context.MastBlogs.Where(b => b.TagDelete == 0).ToList();

            foreach (var b in blogs)
            {
                if (string.IsNullOrEmpty(b.BlogImage) ||
                    !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", b.BlogImage.TrimStart('/'))))
                {
                    b.BlogImage = defaultImagePath;
                }
            }

            return View(blogs);
        }

        // Create - GET
        public IActionResult Create()
        {
            TempData["SuccessMessage"] = null;
            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MastBlog blog, IFormFile blogImage)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (blogImage != null && blogImage.Length > 0)
                {
                    string fileName = Path.GetFileName(blogImage.FileName);
                    string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "BlogImages");

                    if (!Directory.Exists(uploadsDirectory))
                    {
                        Directory.CreateDirectory(uploadsDirectory);
                    }

                    string filePath = Path.Combine(uploadsDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await blogImage.CopyToAsync(stream);
                    }

                    blog.BlogImage = "/Content/BlogImages/" + fileName;
                }

                blog.EntDate = DateTime.Now;
                blog.EntTime = DateTime.Now.ToLocalTime();
                blog.TagActive = 0;
                blog.TagDelete = 0;

                _context.MastBlogs.Add(blog);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Blog saved successfully!";
                // optionally redirect, but to keep same pattern return view with TempData handled by script
            }

            return View(blog);
        }

        // Edit - GET
        public IActionResult Edit(int id)
        {
            var blog = _context.MastBlogs.FirstOrDefault(b => b.MastBlogKey == id);
            if (blog == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(blog.BlogImage) ||
                !System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blog.BlogImage.TrimStart('/'))))
            {
                blog.BlogImage = defaultImagePath;
            }

            TempData["SuccessMessage"] = null;
            return View(blog);
        }

        // Edit - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MastBlog blog, IFormFile blogImage)
        {
            var existing = _context.MastBlogs.FirstOrDefault(x => x.MastBlogKey == id);
            if (existing == null) return NotFound();

            existing.BlogTitle = blog.BlogTitle;
            existing.BlogSummary = blog.BlogSummary;
            existing.BlogContent = blog.BlogContent;

            // SEO fields
            existing.MetaTitle = blog.MetaTitle;
            existing.MetaDescription = blog.MetaDescription;
            existing.MetaKeywords = blog.MetaKeywords;

            // Handle file upload if a new image is selected
            if (blogImage != null && blogImage.Length > 0)
            {
                string fileName = Path.GetFileName(blogImage.FileName);
                string uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Content", "BlogImages");

                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                string filePath = Path.Combine(uploadsDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await blogImage.CopyToAsync(stream);
                }

                existing.BlogImage = "/Content/BlogImages/" + fileName;
            }

            existing.EditDate = DateTime.Now;
            existing.EditTime = DateTime.Now.ToLocalTime();

            _context.Update(existing);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Blog updated successfully!";
            // keep same behavior as Doctor controller (return View so swal runs then redirect)
            blog.BlogImage = existing.BlogImage;
            return View(blog);
        }

        // Delete - POST (soft delete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = _context.MastBlogs.FirstOrDefault(b => b.MastBlogKey == id);
            if (blog == null)
            {
                return NotFound();
            }

            blog.TagDelete = 1;
            _context.Update(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
