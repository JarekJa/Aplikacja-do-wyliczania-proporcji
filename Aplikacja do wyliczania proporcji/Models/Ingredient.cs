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
    public class Ingredient : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } = -1;
        public int IdListIngredients { get; set; }
        public string NameList { get; set; } 
        private int _Index { get; set; }
        private string _Name { get; set; } = "";
        private string _Percent { get; set; } = "";
        private string _PercentColor { get; set; } = "White";
        private string _Mass { get; set; } = "";
        public Ingredient(int index)
        {
            _Index = index;
            _Percent = "1";
            _Mass = "1";
        }
        public Ingredient(int index,string name,string percent, string mass)
        {
            _Index = index;
            _Percent = percent;
            _Mass = mass;
            _Name= name;
        }
        public Ingredient()
        {
            
        }
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }
        public string Percent
        {
            get => _Percent;
            set
            {
                _Percent = value;
                OnPropertyChanged();
            }
        }
        public string PercentColor
        {
            get => _PercentColor;
            set
            {
                _PercentColor = value;
                OnPropertyChanged();
            }
        }
        public int Index
        {
            get => _Index;
            set
            {
                _Index = value;
                OnPropertyChanged();
            }
        }
        public string Mass
        {
            get => _Mass;
            set
            {
                _Mass = value;
                OnPropertyChanged();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
