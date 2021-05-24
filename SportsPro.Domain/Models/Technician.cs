using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Technician
    {
		[DisplayName("Technician Id#")]
		public int TechnicianID { get; set; }	   

		[Required]
		[DisplayName("Technician Name")]
		public string Name { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string Phone { get; set; }
	}
}
