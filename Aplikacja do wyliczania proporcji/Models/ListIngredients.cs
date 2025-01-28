using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacja_do_wyliczania_proporcji.Models
{
    public class ListIngredients:ListIng
    {
        public ObservableCollection<Ingredient> Ingredients { get; set; } = new ObservableCollection<Ingredient>();

            public ListIngredients(string name, ObservableCollection<Ingredient> ingredients)
        {
                Name = name;
            Ingredients = ingredients;
            Count = ingredients.Count;
        }
    }
}
