using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDomain
{
    public class Animal
    {
        [Key]
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = "null";
        [Required(ErrorMessage = "Type is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Type { get; set; } = "null";
        [Required(ErrorMessage = "Price is required.")]
        [Range(1, int.MaxValue)]
        public int Price { get; set; }
        public string Image { get; set; } = "null";
        public ICollection<BookingAnimal> BookingAnimals { get; set; } = new List<BookingAnimal>();
        public Animal() { }

        public Animal(int id, string name, string type, int price, string image)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
            Image = image;
        }
    }
}
