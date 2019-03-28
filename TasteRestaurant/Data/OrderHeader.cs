using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasteRestaurant.Data
{
    public class OrderHeader
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Data do pedido")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name = "Total de pedido")]
        public double OrderTotal { get; set; }

        [Required]
        [Display(Name = "Horário da retirada")]
        public DateTime PickUpTime { get; set; }

        [Display(Name = "Condição")]
        public string Status { get; set; }

        [Display(Name = "Comentários")]
        public string Comments { get; set; }

    }
}
