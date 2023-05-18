using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Hosting;
using Pizzeria.Models;
using System.Data;

namespace Pizzeria.Controllers
{
    public class PizzaController : Controller
    {
        // GET: PizzaController1
        //******************************************************************************** GET INDEX
        public ActionResult Index()
        {
            using PizzaContext db = new PizzaContext();
                
                
            List<Pizza> pizze = db.Pizze.ToList<Pizza>();
            
            
            return View("Index",pizze);
        }
        //******************************************************************************** GET DETAILS
        // GET: PizzaController1/Details/5
        public ActionResult Details(int id)
        {
            using PizzaContext db = new PizzaContext();
            if (id == null)
                return NotFound();

            var pizza = db.Pizze.FirstOrDefault(p => p.Id == id);
            if (pizza == null)
                return NotFound();

            return View("ShowPizza", pizza);
        }
        //******************************************************************************** GET CREATE
        // GET: PizzaController1/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            //per utilizzare il db
            using PizzaContext db = new PizzaContext();
            //creo una lista dove schiaffo tutte le categorie
            List<Categories> categories = db.Categories.ToList();
            //creo una lista dove metto gli ingredienti
            List<Ingredient> ingredienti = db.ingredients.ToList();
            //creo una nuova istanza di pizzamodel
            PizzaFormModel model = new PizzaFormModel();
            
            //creo la lista da utilizzare di tipo SelectlistItem
            List<SelectListItem> listIngredienti = new List<SelectListItem>();

            //itero su ingredienti
            foreach(Ingredient ingrediente in ingredienti)
            {
                //ogni ingrediente lo aggiungo e creo una nuova istanza della classe SelectListItem dove specifico text e value
                listIngredienti.Add(new SelectListItem()
                { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
            }
            //passo al modello gli ingredienti
            model.Ingredienti = listIngredienti;
            //dico che model.categ é = alla lista di categ
            model.Categories = categories;
            //creo una nuova istanza di pizza
            model.Pizza = new Pizza();

            return View("CreatePizza", model);
        }

        // POST: PizzaController1/Create
        //************************************************************************************POST CREATE
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PizzaFormModel data)
        {
            using PizzaContext db = new PizzaContext();
            if (!ModelState.IsValid)
            {
                List<Ingredient> ingredients = db.ingredients.ToList();
                List<SelectListItem> listIngredienti = new List<SelectListItem>();
                foreach (Ingredient ingrediente in ingredients)
                {
                    listIngredienti.Add(
                        new SelectListItem()
                        { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
                }
                data.Ingredienti = listIngredienti;

                List<Categories> categories = db.Categories.ToList();
                data.Categories = categories;
                return View("CreatePizza", data);
            }
            Pizza createPizza = new Pizza();
            createPizza.Name = data.Pizza.Name;
            createPizza.Description = data.Pizza.Description;
            createPizza.CategoryId = data.Pizza.CategoryId;
            createPizza.image = data.Pizza.image;
            createPizza.price = data.Pizza.price;
            //sto inizializzando una lista di ingredienti se nò é nullo a questo punto!
            createPizza.Ingredients = new List<Ingredient>();

            
             
            if (data.SelectedIngredients  != null)
            {
                foreach (string selectedIngredients in data.SelectedIngredients)
                {
                    int selectedIngredientsId = int.Parse(selectedIngredients);
                    Ingredient ingredient = db.ingredients
                        .Where(m=> m.Id == selectedIngredientsId)
                        .FirstOrDefault();
                    createPizza.Ingredients.Add(ingredient);
                }
            }

            db.Pizze.Add(createPizza);
            db.SaveChanges();
            return RedirectToAction("Index", new {message="Pizza inserita correttamente" });
        }

        // GET: PizzaController1/Edit/5
        //********************************************************************************************************************* GET UPDATE
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {
            using PizzaContext db = new PizzaContext();

            PizzaFormModel model = new PizzaFormModel();
            List<Categories> categories = db.Categories.ToList();
            //creo una lista dove metto gli ingredienti
            List<Ingredient> ingredienti = db.ingredients.ToList();
            //creo la lista da utilizzare di tipo SelectlistItem
            List<SelectListItem> listIngredienti = new List<SelectListItem>();

            //itero su ingredienti
            foreach (Ingredient ingrediente in ingredienti)
            {
                //ogni ingrediente lo aggiungo e creo una nuova istanza della classe SelectListItem dove specifico text e value
                listIngredienti.Add(new SelectListItem()
                { Text = ingrediente.Name, Value = ingrediente.Id.ToString(),
                Selected = ingredienti.Any(m=> m.Id == ingrediente.Id )});
            }
            //passo al modello gli ingredienti
            model.Ingredienti = listIngredienti;
            //dico che model.categ é = alla lista di categ
            model.Categories = categories;

             
            model.Pizza = db.Pizze.FirstOrDefault(p => p.Id == id);

            return View("EditPizza", model);
        }

        // POST: PizzaController1/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, PizzaFormModel data)
        {
            using PizzaContext db = new PizzaContext();
            if (!ModelState.IsValid)
            {
                //assegno a ingredients la lista degli ingredienti che prendo dal db
                List<Ingredient> ingredients = db.ingredients.ToList();
                //creo una nuova lista degli ingredienti selezionati
                List<SelectListItem> IngredientSelected = new List<SelectListItem>();
                foreach (Ingredient ingrediente in ingredients)
                {
                    //aggiungo gli elementi selezionati alla lista e li converto in una stringa per manipolarla meglio
                    IngredientSelected.Add(
                        new SelectListItem()
                        { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
                }
                data.Ingredienti = IngredientSelected;

                List<Categories> categories = db.Categories.ToList();
                data.Categories = categories;
                return View("EditPizza", data);
            }

            //assegno a pizzamodificata una pizza presa dal db che abbia lo stesso id e che include gli ingredienti
            Pizza pizzaModificata = db.Pizze.Where(pizza => pizza.Id == id)
                .Include(pizza => pizza.Ingredients).FirstOrDefault();
            //quello che passo tramide data lo do a pizza modificata e così via!
            pizzaModificata.Name = data.Pizza.Name;
            pizzaModificata.Description = data.Pizza.Description;
            pizzaModificata.CategoryId = data.Pizza.CategoryId;
            pizzaModificata.image = data.Pizza.image;
            pizzaModificata.price = data.Pizza.price;
            //sto inizializzando una lista di ingredienti se nò é nullo a questo punto!
            pizzaModificata.Ingredients = new List<Ingredient>();


            //se quello che passo come parametro, cioé gli ingredienti selezionati non sono nulli allora
            if (data.SelectedIngredients != null)
            {
                //itero, ingredienteSelezionato in quello che mi hanno passato come parametro
                foreach (string ingredienteSelezionato in data.SelectedIngredients)
                {
                    //trasformo in ingredienteID cioé in intero ingredienteselezionato
                    int ingredienteSelezionatoId = int.Parse(ingredienteSelezionato);
                    //ingrediente é = a dentro al db id = allo stesso id di ingredienteselezionatoID e prendo il primo
                    Ingredient ingredient = db.ingredients
                        .Where(m => m.Id == ingredienteSelezionatoId)
                        .FirstOrDefault();
                    //passo a pizza da modificare alla lista degli ingredienti, gli ingredienti che hannoo lo stesso id
                    pizzaModificata.Ingredients.Add(ingredient);
                }
            }

            
            db.SaveChanges();
            return RedirectToAction("Index", new { message = "Pizza inserita correttamente" });
        }

        // GET: PizzaController1/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PizzaController1/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
           
            using PizzaContext db = new PizzaContext();

            if (id == null)
                return NotFound();

            var pizza = db.Pizze.FirstOrDefault(p => p.Id == id);
            if (pizza == null)
                return NotFound();
            db.Pizze.Remove(pizza);
            db.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
