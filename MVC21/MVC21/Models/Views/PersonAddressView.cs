using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC21.Models.Views
{
    public class PersonAddressView
    {
        [Key]
        public int PersonID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public long Mobile { get; set; }

        public int AddressID { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public int Pin { get; set; }

    }
}
