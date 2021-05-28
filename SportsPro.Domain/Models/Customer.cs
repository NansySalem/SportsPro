using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System;


namespace SportsPro.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [Required]
        [StringLength(51, MinimumLength = 1, ErrorMessage = "The field must be a string with a minimum length of 1 and a maximum length of 51.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }



        [Required]
        [StringLength(51, MinimumLength = 1, ErrorMessage = "The field must be a string with a minimum length of 1 and a maximum length of 51.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }



        [Required]
        [StringLength(51, MinimumLength = 1, ErrorMessage = "The field must be a string with a minimum length of 1 and a maximum length of 51.")]
        [DisplayName("Address")]
        public string Address { get; set; }



        [Required]
        [StringLength(51, MinimumLength = 1, ErrorMessage = "The field must be a string with a minimum length of 1 and a maximum length of 51.")]
        public string City { get; set; }



        [Required]
        [StringLength(51, MinimumLength = 1, ErrorMessage = "The field must be a string with a minimum length of 1 and a maximum length of 51.")]
        public string State { get; set; }



        [Required]
        [StringLength(21, MinimumLength = 1, ErrorMessage = "The field must be a string with a minimum length of 1 and a maximum length of 21.")]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }



        [Required]
        [DisplayName("Country")]
        public string CountryID { get; set; }
        public Country Country { get; set; }



        //[RegularExpression(@"^\(\d{3}\)\s\d{3}-\d{4}", ErrorMessage = "Entered phone format is not valid. Please follow(999)999-9999")]
        public string Phone { get; set; }



        [StringLength(51, MinimumLength = 1, ErrorMessage = "The field must be a string with a minimum length of 1 and a maximum length of 51.")]
        [Required]
        [DataType(DataType.EmailAddress)]
       
        public string Email { get; set; }




        [DisplayName("Customer Name")]
        public string FullName => FirstName + " " + LastName;   // read-only property



        public ICollection<Registration> Registrations { get; set; }
    }
}