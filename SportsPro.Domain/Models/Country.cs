using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Country
    {
		[Required]
		[DisplayName("Country Id#")]
		public string CountryID { get; set; }

		[Required]
		public string Name { get; set; }
	}
}
