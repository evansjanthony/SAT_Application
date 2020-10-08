using SAT_Application.UI.MVC.Models;
using System;
using System.Net.Mail;
using System.Web.Mvc;

namespace SimpleTemplateConvert.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }
            string message = $"{cvm.Name} has contacted you from {cvm.Email}. <br/>Subject: {cvm.Subject}<br/>Message: {cvm.Message}";

            MailMessage mail = new MailMessage("newadmin1@example.com", "emailaddress@site.com", $"{System.DateTime.Now.Date} - {cvm.Subject}", message);

            mail.Priority = MailPriority.High;
            mail.IsBodyHtml = true;
            mail.ReplyToList.Add(cvm.Email);
            SmtpClient client = new SmtpClient("emailaddress@site.com");
            client.Credentials = new System.Net.NetworkCredential("newadmin1@example.com", "Admin@123456");
            client.Port = 8889;

            try
            {
                client.Send(mail);
            }
            catch (Exception e)
            {
                ViewBag.Error = "Sorry, there was an error.";

                if (User.IsInRole("Admin"))
                {
                    ViewBag.Error = $"Error: {e.StackTrace}";
                }

                return View(cvm);
            }

            return View("EmailConfirmation", cvm);
        }
    }
}
