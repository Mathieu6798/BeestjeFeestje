using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDomain;
namespace MyDomain.Guest
{
    public class GuestVM : IGuestVM
    {
        [Required]
        public string Id { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "CustomerCard")]
        public string? CustomerCard { get; set; }
        private List<string> customerCardOptions = new List<string> { "Zilver", "Goud", "Platina" };
        public List<string> CustomerCardOptions
        {
            get { return customerCardOptions; }
            set { customerCardOptions = value; }
        }
    }

}
