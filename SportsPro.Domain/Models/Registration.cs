using SportsPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsPro.Models
{
    public class Registration
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }


        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
