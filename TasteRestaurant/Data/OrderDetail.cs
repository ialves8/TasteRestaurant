using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasteRestaurant.Data
{
    public class OrderDetail
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual OrderHeader OrderHeader { get; set; }

        [Required]
        public int MenuItemId { get; set; }

        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }

        [Required]
        [Display(Name = "Quantidade")]
        public int Count { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Preço")]
        public double Price { get; set; }
    }
}
