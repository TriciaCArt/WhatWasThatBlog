using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using WhatWasThatBlog.Data;
using WhatWasThatBlog.Enums;
using WhatWasThatBlog.Models;
using X.PagedList;

namespace WhatWasThatBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _appSettings;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IEmailSender emailSender, IConfiguration appSettings)
        {
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
            _appSettings = appSettings;
        }

        //public IActionResult Index()
        //{
        //    var blogPosts = _context.BlogPosts.ToList();
        //    return View(blogPosts);
        //}
        public async Task<IActionResult> Index(int? pageNum) {
            pageNum ??= 1;
            var pageSize = 5;

            var posts = await _context.BlogPosts
                        .Where(b => b.BlogPostState == BlogPostState.ProductionReady && !b.IsDeleted)
                        .OrderByDescending(b=>b.Created)
                        .ToPagedListAsync(pageNum, pageSize);

            return View(posts);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult ContactMe()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactMe(string name, string email, string phone, string subject, string message)
        {
            var myEmail = _appSettings["SmtpSettings:UserName"];



            StringBuilder _builder = new(message);
            _builder.AppendLine("<br><br>");
            _builder.AppendLine($"Sender's Information <br>");
            _builder.AppendLine($"Name:{name}<br>");
            _builder.AppendLine($"Email:{email}<br>");
            _builder.AppendLine($"Phone:{phone}<br>");

            await _emailSender.SendEmailAsync(myEmail, subject, _builder.ToString());
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}