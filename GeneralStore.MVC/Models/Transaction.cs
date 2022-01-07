using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeneralStore.MVC.Models
{
    public class Transaction
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        [ForeignKey(nameof(Customer))]
        [Display(Name = "Customer Name")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        [Required]
        [ForeignKey(nameof(Product))]
        [Display(Name = "Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int ItemCount { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        [Display(Name = "Transaction Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfTransaction { get; set; }
    }
}