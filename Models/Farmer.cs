using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AgriEnergyWebApp.Models
{
    public partial class Farmer
    {
        public Farmer()
        {
            Products = new HashSet<Product>();
        }
        [Required(ErrorMessage = "Please Enter Farmer ID")]
        public string FarmerId { get; set; }
        [Required(ErrorMessage = "Please Farmer Name")]
        public string FarmerName { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
