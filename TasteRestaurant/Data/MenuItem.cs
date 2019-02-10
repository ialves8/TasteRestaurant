using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasteRestaurant.Data
{
    public class MenuItem
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Imagem")]
        public string Image { get; set; }

        [Display(Name = "Picante")]
        public string Spicyness { get; set; }

         public enum Specify { Suave=0, Moderado=1, Alto=2 }

        [Display(Name = "Preço")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que {1,00} r$")]
        public double Price { get; set; }

        [Display(Name = "Categoria")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryType CategoryType { get; set; }

        [Display(Name = "Tipo de alimento")]
        public int FoodTypeId { get; set; }

        [ForeignKey("FoodTypeId")]
        public virtual FoodType FoodType { get; set; }
    }
}
