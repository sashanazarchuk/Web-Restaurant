using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal Price { get; set; } 
        public decimal Discount { get; set; }

        [ForeignKey("Category")]
        public virtual int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductImages> Images { get; set; }
    }
}
