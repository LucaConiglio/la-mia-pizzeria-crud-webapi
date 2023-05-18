using System.ComponentModel.DataAnnotations;

namespace Pizzeria.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Pizza>? Pizze { get; set; }
    }
}
