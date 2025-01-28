using Aplikacja_do_wyliczania_proporcji.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacja_do_wyliczania_proporcji.Sercices.Interfaces
{
    public  interface IDataBase
    {
        public void Init();
        public Task<int> AddItemAsync(ListIngredients list);
        public Task<ObservableCollection<Ingredient>> GetListItemsAsync(int id);
        public Task< ObservableCollection<ListIngString>> GetAllListStringAsync();
        public Task<bool> DelateLisatAsync(int id);
        public Task<bool> ModifyItemAsync(ListIngredients list);

    }
}
