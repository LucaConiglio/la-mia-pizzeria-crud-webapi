using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pizzeria.Models
{
    public class PizzaFormModel
    {
        public Pizza? Pizza { get; set; }
        public List<Categories>? Categories { get; set; }

        public List<SelectListItem>? Ingredienti {  get; set; } 
        public List<string>? SelectedIngredients { get; set;}
    }
}
