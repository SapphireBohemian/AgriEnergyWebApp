using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AgriEnergyWebApp.Models
{
    public partial class Employee
    {
        [Required(ErrorMessage = "Please Enter Admin ID")]
        public string AdminId { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
    }
}
