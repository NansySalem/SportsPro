﻿using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;

namespace SportsPro.Models
{
    public class Customer
    {
		public int CustomerID { get; set; }

		[Required]
		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[Required]
		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[Required]
		[DisplayName("Address")]
		public string Address { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string State { get; set; }

		[Required]
		[DisplayName("Postal Code")]
		public string PostalCode { get; set; }

		[Required]
		[DisplayName("Country")]
		public string CountryID { get; set; }
		public Country Country { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		[DisplayName("Name")]
		public string FullName => FirstName + " " + LastName;   // read-only property
	}
}