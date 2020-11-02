using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping
{
    public class Customer
    {
        public string CustomerId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Key]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        //[RegularExpression(pattern: @"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$", ErrorMessage = "Email not valid")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string phone { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
