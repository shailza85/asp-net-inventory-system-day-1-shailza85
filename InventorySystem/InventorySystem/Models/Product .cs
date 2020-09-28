using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace InventorySystem.Models
{
    /*
        Product must have unique product “ID”
        Product must have a product “Name”
        Product must have a “Quantity” that is  greater than or equal to zero
        Product must have an “IsDiscontinued” boolean
        Set to false by default

     */

    [Table("product")]
    public partial class Product
    {
        [Key]
        [Column("ID", TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [Column(TypeName = "int(10)")]
        public int Quantity { get; set; }

        [DefaultValue(false)]
        [Column(TypeName = "BOOLEAN")]
        public bool IsDiscontinued { get; set; }

       }
}
