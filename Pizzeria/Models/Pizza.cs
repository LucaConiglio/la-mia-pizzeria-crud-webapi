using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pizzeria.Models
{
    public class Pizza
    {


        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Il nome della pizza é obbligatorio!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La descrizione é obbligatoria!")]
        [MoreThenWordValidation(ErrorMessage = "La descrizione deve avere almeno 5 parole.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "L'immagine é obbligatoria.")]
        public string image { get; set; }
        [Required(ErrorMessage = "Il prezzo é obbligatorio. Es. 5,50")]
        [Range(0, 15, ErrorMessage = "Il prezzo deve essere maggiore di (0) e minore di (15)")]
        public double price { get; set; }
        
        
        public int? CategoryId { get; set; }
        public Categories? Category {get; set;}
        public List<Ingredient>? Ingredients { get; set;}
            
            
          
    }
}
