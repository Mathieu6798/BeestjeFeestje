using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDomain.Guest
{
    public interface IGuestVM
    {
        string Id { get; set; }
        string? Email { get; set; }
        string Name { get; set; }
        string? PhoneNumber { get; set; }
        string Address { get; set; }
        public string? CustomerCard { get; set; }
        List<string> CustomerCardOptions { get; set; }
    }
}
