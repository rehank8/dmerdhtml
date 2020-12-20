using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dmnerdhtml.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using TeacherApplication.Models;

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
		public async Task<ActionResult> BookAppoinment(FormCollection form)
		{
			//MailAddress to = new MailAddress(ConfigurationManager.AppSettings["ToEmailContcat"]);
			//MailMessage message = new MailMessage();
			//message.To.Add(to);
			//message.Subject = "New Message From " + form["name"];
			//message.IsBodyHtml = true;
			//message.BodyEncoding = System.Text.Encoding.ASCII;
			//message.Body = $"Hello Rehan,  New Messge: {form["message"]} from: {form["email"] } " +
			//				$"Phone: {form["phone"]} Booking Date: {form["bookDate"]} Time: {form["bookTime"]}";

			//SmtpClient smtp = new SmtpClient();
			//smtp.Send(message);

			ViewBag.Message = "Your contact page.";

			var apiKey = System.Configuration.ConfigurationManager.AppSettings["sendgridkey"];
			var client = new SendGridClient(apiKey);

			//Send Email to owner
			var msg = new SendGridMessage()
			{
				From = new EmailAddress("rehabk8@outlook.com", "Dmnerd"),
				Subject = "New Client Appointment From " + form["name"],
				PlainTextContent = "Hello Rehan,  New Messge: " + form["message"] + " from: " + form["email"] + " Phone: " + form["phone"],
				HtmlContent = "Hello Rehan,  New Messge: " + form["message"] + " from: " + form["email"] + " Phone: " + form["phone"],

			};

			var toEmails = new List<EmailAddress>();
			toEmails.Add(new EmailAddress(ConfigurationManager.AppSettings["ToEmailContcat"]));
			msg.AddTos(toEmails);
			var response = await client.SendEmailAsync(msg);



			///Send email to customer
			msg = new SendGridMessage()
			{
				From = new EmailAddress("rehabk8@outlook.com", "Dmnerd"),
				Subject = "Your appoinment has beeen booked",
				PlainTextContent = "Hello " + form["name"] + ",  Your appoinmnet booked on " + form["bookDate"] + " " + form["bookTime"],
				HtmlContent = "Hello " + form["name"] + ",  Your appoinmnet booked on " + form["bookDate"] + " " + form["bookTime"],

			};
			toEmails = new List<EmailAddress>();
			toEmails.Add(new EmailAddress(form["email"]));

			msg.AddTos(toEmails);
			response = await client.SendEmailAsync(msg);


			sms objSMS = new sms();
			objSMS.SendSmsMessage(ConfigurationManager.AppSettings["VendorPhone"], $"Hi, you got a appoinment booked from {form["name"]}  at {form["bookDate"]} {form["bookTime"]} contact {form["phone"]} ");
			objSMS.SendSmsMessage(form["phone"], $"Hi {form["name"]}, Your appoinment is booked at {form["bookDate"]} {form["bookTime"]}, for any changes please call {ConfigurationManager.AppSettings["VendorPhone"]} ");
			//objSMS.SendSmsMessage(form["phone"], $"Hi {form["name"]}");

			TempData["MessageSent"] = "Message Sent!";

			var userQuery = new UserQueryDTO()
			{
				EMailID = form["email"],
				FirstName = form["name"],
				PhoneNo = form["phone"],
				Query = "From Statice Web Dmnerd",
				selelecteddate = form["bookDate"],
				time = form["bookDate"] + " " + form["bookTime"],
				TeacherID = int.Parse(ConfigurationManager.AppSettings["TeacherID"])
			};

			HttpClient httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			httpClient.BaseAddress = new System.Uri(ConfigurationManager.AppSettings["ApiBaseAddress"]);


			HttpResponseMessage httpResponse = await httpClient.PostAsJsonAsync(
			  "api/homeapi/staticbookappoinment", userQuery);


			return RedirectToAction("Index");
		}


		public ActionResult ResturantinDesplaines()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}