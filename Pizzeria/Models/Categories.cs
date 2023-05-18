using System.ComponentModel.DataAnnotations;

namespace Pizzeria.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Pizza>? Pizze { get; set; }
    }
}
