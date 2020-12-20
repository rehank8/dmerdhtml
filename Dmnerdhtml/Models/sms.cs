using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.Http;
using System.Net;
using System.Configuration;

namespace TeacherApplication.Models
{
	public class sms
	{
		private readonly ITwilioRestClient _client;
		private readonly string _accountSid = "";//"AC845d34902af6ce50ebc8a1fb8d5941f9";
		private readonly string _authToken = "";// "cb259b7261353bb818deda10fd2f46f7";

		//private readonly string _accountSid = 
		//private readonly string _authToken = ";
		private readonly string _twilioNumber = "";//"(219)-249-4775";
		public string Tophonenumber { get; set; }
		public string bodymessage { get; set; }


		public sms()
		{
			//UserProfile userProfile = DbHelper.GetUserProfile(Helper.UserId);
			_accountSid = ConfigurationManager.AppSettings["TwilioAccountSId"];// "AC845d34902af6ce50ebc8a1fb8d5941f9";
			_authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];// "cb259b7261353bb818deda10fd2f46f7";
			_twilioNumber = ConfigurationManager.AppSettings["TwilioPhone"];// "(219)-249-4775";

			System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			_client = new TwilioRestClient(_accountSid, _authToken);
		}
		 

		public sms(ITwilioRestClient client)
		{

			_client = client;
		}

		public void SendSmsMessage(string phoneNumber, string message)
		{
			if (!string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(message))
			{
				string[] toPhoneNumbers = phoneNumber.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

				foreach (var toPhoneNumber in toPhoneNumbers)
				{
					var to = new Twilio.Types.PhoneNumber(toPhoneNumber);
					MessageResource.Create(
						to,
						from: new Twilio.Types.PhoneNumber(_twilioNumber),
						body: message,
						client: _client);
				}



			}
		}

	}

}