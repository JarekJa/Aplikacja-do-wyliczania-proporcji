using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacja_do_wyliczania_proporcji.Models
{
    [SQLite.Table("Ingredient")]
    public class Ingredient 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } = -1;
        public int IdListIngredients { get; set; }
        public string NameList { get; set; }
        public int Index { get; set; }
        public string Name { get; set; } = "";
        public double Percent { get; set; }
        public double Mass { get; set; } 
        public Ingredient(int index)
        {
            Index = index;
            Percent = 1;
            Mass = 1;
        }
        public Ingredient(int index,string name,double percent, double mass)
        {
            Index = index;
            Percent = percent;
            Mass = mass;
            Name= name;
        }
        public Ingredient()
        {
            
        }

    }
}
