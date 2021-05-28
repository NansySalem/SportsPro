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
        [StringLength(51, MinimumLength = 5, ErrorMessage = "Please enter a value between 5-50 characters")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }



        [Required]
        [StringLength(51, MinimumLength = 1, ErrorMessage = "Please enter a value between 1-50 characters")]
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
        [RegularExpression(@"(^(?!.*[DFIOQUdfioqu])([A-VXYa-vxy][0-9][A-Za-z])[ -]?([0-9][a-zA-Z][0-9])$)|(^([0-9]{5})[ :-]?([0-9]{4})?$)", ErrorMessage = "{0} is not a valid postal code")]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }



        [Required]
        [DisplayName("Country")]
        public string CountryID { get; set; }
        public Country Country { get; set; }



       [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "{0} is not a valid postal code")]
        public string Phone { get; set; }



        [StringLength(51, ErrorMessage = "Invalid email")]
        [Required]
        [DataType(DataType.EmailAddress)]
       
        public string Email { get; set; }




        [DisplayName("Customer Name")]
        public string FullName => FirstName + " " + LastName;   // read-only property



        public ICollection<Registration> Registrations { get; set; }
    }
}