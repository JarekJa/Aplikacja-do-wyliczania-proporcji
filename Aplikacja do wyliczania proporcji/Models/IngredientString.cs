using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacja_do_wyliczania_proporcji.Models
{
     class IngredientString: INotifyPropertyChanged
    {
        private int _Index { get; set; }
        private string _Name { get; set; } = "";
        private string _Percent { get; set; } = "";
        private string _PercentColor { get; set; } = "White";
        private string _Mass { get; set; } = "";

        public IngredientString(Ingredient ingredient)
        {
            _Index = ingredient.Index;
            _Name = ingredient.Name;    
            _Percent = Convert.ToString( ingredient.Percent);
            _Mass= Convert.ToString( ingredient.Mass);
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
