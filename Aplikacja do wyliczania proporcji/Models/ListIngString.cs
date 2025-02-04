using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacja_do_wyliczania_proporcji.Models
{
    public class ListIngString : ListIng
    {
        public string IngredientsString { get; set; } = "";

            public ListIngString(Ingredient ingredient)
        {
            Name = ingredient.NameList;
            if (ingredient.Name.Length <= 6)
            {
                IngredientsString = ingredient.Name + ":" + Convert.ToString(ingredient.Percent) + "%," + Convert.ToString(ingredient.Mass) + ";  ";
            }
            else
            {
                IngredientsString = ingredient.Name.Substring(0, 6) + ":" + Convert.ToString(ingredient.Percent) + "%," + Convert.ToString(ingredient.Mass) + ";  ";
            }
            Count = 1;
            IdList = ingredient.IdListIngredients;
        }
        public void AddIngredient(Ingredient ingredient)
        {
            if (ingredient.Name.Length <= 6)
            {
                IngredientsString += ingredient.Name + ":" + Convert.ToString(ingredient.Percent) + "%," + Convert.ToString(ingredient.Mass) + ";  ";
            }
            else
            {
                IngredientsString += ingredient.Name.Substring(0, 6) + ":" + Convert.ToString(ingredient.Percent) + "%," + Convert.ToString(ingredient.Mass) + ";  ";
            }
            Count++;
        }
    }
}
