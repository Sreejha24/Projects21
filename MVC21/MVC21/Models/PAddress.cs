using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC21.Models
{
    [Table("PAddress")]
    public class PAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public int Pin { get; set; }

    }
}
