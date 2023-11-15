﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;

namespace DemoPL.ViewModels
{
	public class UserViewModel
	{
		public string Id { get; set; }

		public string FName { get; set; }
		public string LName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public IEnumerable<string>? Roles { get; set; }

		public UserViewModel()
		{
			Id = Guid.NewGuid().ToString();	
		}
	}
}
