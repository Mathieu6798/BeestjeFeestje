using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDomain
{
    public class BookingAnimal
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int AnimalId { get; set; } 
        public Animal Animal { get; set; }
    }
}
