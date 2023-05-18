using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pizzeria.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaApi : ControllerBase
    {
        // GET: api/<PizzaApi>
        [HttpGet]
        public IActionResult GetPizze(string? search)
        {
            
            using var db = new PizzaContext();
            IQueryable<Pizza> listaPizze = db.Pizze;
            List<Pizza> listaPizzaSing;

            if(search!= null)
            {
                listaPizzaSing = db.Pizze.Where(p => p.Name.ToLower().Contains(search.ToLower())).ToList();
                //prendo le pizze dal db li trasformo in caratteri minuscoli e gli dico la stringa che mi viene passata, la trasformo anchessa e vedo se il db ha la stessa stringa!
                return Ok(listaPizzaSing);
            }    else
                return Ok(listaPizze.ToList());


        }

        // GET api/<PizzaApi>/get/5
        [HttpGet("{id}")]
        public IActionResult Get(int? id)
        {
            using var db = new PizzaContext();


            if (id != null)
            {
                
                var pizza = db.Pizze.Where(p => p.Id == id).FirstOrDefault();
                if (pizza != null)
                return Ok(pizza);
                else return NotFound();
            }
            return NotFound();
          
        }

        //  /api/pizzaapi/Create
        [HttpPost]
        public IActionResult Create(Pizza data)
        {

            using PizzaContext db = new PizzaContext();
            Pizza pizzaToCreate = new Pizza();
            pizzaToCreate.Ingredients = new List<Ingredient>();
            pizzaToCreate.Name = data.Name;
            pizzaToCreate.Description = data.Description;
            pizzaToCreate.image = data.image;
            pizzaToCreate.price = data.price;
            pizzaToCreate.CategoryId = data.CategoryId;

            if (data.Ingredients != null)
            {
                foreach (Ingredient ingrediente in data.Ingredients)
                {
                    int selectIntIngredientId = ingrediente.Id;
                    Ingredient ingredient = db.ingredients.FirstOrDefault(m => m.Id == selectIntIngredientId);
                    pizzaToCreate.Ingredients.Add(ingredient);
                }
            }
            db.Pizze.Add(pizzaToCreate);
            db.SaveChanges();

            return Ok();
        }
        // PUT api/<PizzaApi>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Pizza data)
        {
            using PizzaContext db = new PizzaContext();
            Pizza pizzaEdit = db.Pizze.Include(p => p.Ingredients).FirstOrDefault(m => m.Id == id);

            pizzaEdit.Ingredients.Clear();

            if (pizzaEdit != null)
            {

                pizzaEdit.Name = data.Name;
                pizzaEdit.Description = data.Description;
                pizzaEdit.image = data.image;
                pizzaEdit.price = data.price;
                pizzaEdit.CategoryId = data.CategoryId;

                if (data.Ingredients != null)
                {
                    foreach (Ingredient i in data.Ingredients)
                    {
                        int selectIntIngredientId = i.Id;
                        Ingredient ingredient = db.ingredients
                        .Where(m => m.Id == selectIntIngredientId)
                        .FirstOrDefault();
                        pizzaEdit.Ingredients.Add(ingredient);
                    }
                }

                db.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/<PizzaApi>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using PizzaContext db = new PizzaContext();
            Pizza pizzaDelete = db.Pizze.Where(m => m.Id == id).FirstOrDefault();

            if (pizzaDelete != null)
            {
                db.Pizze.Remove(pizzaDelete);
                db.SaveChanges();

                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
