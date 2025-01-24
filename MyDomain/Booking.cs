using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDomain
{
    public class Booking
    {
        [Key]
        [Required(ErrorMessage = "Id is required.")]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
        public string? UserId { get; set; }
        [Required(ErrorMessage = "BookedDate is required.")]
        public DateTime BookedDate { get; set; }
        public ContactInformation ContactInformation { get; set; }
        public ICollection<BookingAnimal> BookingAnimals { get; set; } = new List<BookingAnimal>();
        public ApplicationUser User { get; set; }
        public Booking() { }
    }
}
