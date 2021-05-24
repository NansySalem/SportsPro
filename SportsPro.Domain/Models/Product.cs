using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsPro.Models
{
    public class Product
    {
		[DisplayName("Product Id#")]
		public int ProductID { get; set; }

		[Required]
		[DisplayName("Product Code")]
		public string ProductCode { get; set; }

		[Required]
		public string Name { get; set; }

		[Range(0, 1000000)]
		[Column(TypeName = "decimal(8,2)")]
		[DisplayName("Yearly Price")]
		public decimal YearlyPrice { get; set; }

		[DisplayName("Release Date")]
		public DateTime ReleaseDate { get; set; } = DateTime.Now;
	}
}
