using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Incident
    {
		[DisplayName("Incident Id#")]
		public int IncidentID { get; set; }

		[Required]
		[DisplayName("Customer Id#")]
		public int CustomerID { get; set; }     // foreign key property
		public Customer Customer { get; set; }  // navigation property

		[Required]
		[DisplayName("Product Id#")]
		public int ProductID { get; set; }     // foreign key property
		public Product Product { get; set; }   // navigation property

		[DisplayName("Technician Id#")]
		public int? TechnicianID { get; set; }     // foreign key property - nullable
		public Technician Technician { get; set; }   // navigation property

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[DisplayName("Date Opened")]
		public DateTime DateOpened { get; set; } = DateTime.Now;

		[DisplayName("Date Closed")]
		public DateTime? DateClosed { get; set; } = null;
	}
}