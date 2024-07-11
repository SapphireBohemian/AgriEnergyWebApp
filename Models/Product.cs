using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AgriEnergyWebApp.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please Enter Product Name")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Please Enter Product Price")]
        public int ProductPrice { get; set; }
        [Required(ErrorMessage = "Please Enter Product Quantity")]
        public int ProductQuantity { get; set; }
        [Required(ErrorMessage = "Please Enter Product Type")]
        public string ProductType { get; set; }
        [Required(ErrorMessage = "Please Enter Date")]
        [DataType(DataType.Date)]
        public DateTime ProductDate { get; set; }

        public string FarmerId { get; set; }

        public virtual Farmer Farmer { get; set; }
    }
}
