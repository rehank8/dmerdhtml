using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Dmnerdhtml.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.MessageSent = TempData["MessageSent"];
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		[HttpGet]
		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
		[HttpPost]
		public ActionResult Contact(FormCollection form)
		{
			ViewBag.Message = "Your contact page.";


			MailAddress to = new MailAddress(ConfigurationManager.AppSettings["ToEmailContcat"]);
			MailMessage message = new MailMessage();
			message.To.Add(to);
			message.Subject = "New Message From " + form["name"];
			message.IsBodyHtml = true;
			message.BodyEncoding = System.Text.Encoding.ASCII;
			message.Body = "Hello Rehan,  New Messge: " + form["message"] + " from: " + form["email"] + "Phone: " + form["phone"];
			SmtpClient smtp = new SmtpClient();


			smtp.Send(message);

			return RedirectToAction("Contact");
		}

		[HttpPost]
		public ActionResult BookAppoinment(FormCollection form)
		{
			MailAddress to = new MailAddress(ConfigurationManager.AppSettings["ToEmailContcat"]);
			MailMessage message = new MailMessage();
			message.To.Add(to);
			message.Subject = "New Message From " + form["name"];
			message.IsBodyHtml = true;
			message.BodyEncoding = System.Text.Encoding.ASCII;
			message.Body = $"Hello Rehan,  New Messge: {form["message"]} from: {form["email"] } " +
							$"Phone: {form["phone"]} Booking Date: {form["bookDate"]} Time: {form["bookTime"]}";

			SmtpClient smtp = new SmtpClient();


			smtp.Send(message);
			TempData["MessageSent"] = "Message Sent!";


			ViewBag.MessageSent = "Message Sent!";
			return RedirectToAction("Index");
		}

		
		public ActionResult ResturantinDesplaines()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}