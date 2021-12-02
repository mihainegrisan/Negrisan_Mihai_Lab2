using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Negrisan_Mihai_Lab2.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
