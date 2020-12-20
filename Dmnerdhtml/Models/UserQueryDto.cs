using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dmnerdhtml.Models
{
	public partial class UserQueryDTO
	{
		public long TeacherID { get; set; }
		public string FirstName { get; set; }

		public string EMailID { get; set; }

		public string PhoneNo { get; set; }

		public string Query { get; set; }

		public string selelecteddate { get; set; }

		public string time { get; set; }

	}
}