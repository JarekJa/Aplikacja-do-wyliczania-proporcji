using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacja_do_wyliczania_proporcji.Models
{
    [SQLite.Table("ListIngredients")]
    public class ListIngredients
    {
        [PrimaryKey, AutoIncrement]
        public int IdList { get; set; }
        public string Name { get; set; }
        public int Count { get; set; } = 0;
        public string IngredientsString { get; set; } = "";
        public ObservableCollection<Ingredient> Ingredients { get; set; } = new ObservableCollection<Ingredient>();
        public ListIngredients()
        {
                
        }
        public ListIngredients(string name, ObservableCollection<Ingredient> ingredients)
        {
                Name = name;
            Ingredients = ingredients;
            Count = ingredients.Count;
        }
        public ListIngredients(int id,string name, Ingredient ingredient)
        {
            Name = name;
            Ingredients = new ObservableCollection<Ingredient>();
            Ingredients.Add(ingredient);
            if (ingredient.Name.Length <= 6)
            {
                IngredientsString = ingredient.Name + ":" + ingredient.Percent + "%," + ingredient.Mass + ";  ";
            }
            else
            {
                IngredientsString = ingredient.Name.Substring(0,6) + ":" + ingredient.Percent + "%," + ingredient.Mass + ";  ";
            }
            Count = 1;
            IdList = id;    
        }
    }
}
