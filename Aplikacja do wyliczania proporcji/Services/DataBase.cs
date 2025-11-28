using Aplikacja_do_wyliczania_proporcji.Models;
using Aplikacja_do_wyliczania_proporcji.Sercices.Interfaces;
using SQLite;
using System.Collections.ObjectModel;

namespace Aplikacja_do_wyliczania_proporcji.Sercices
{

    public class DataBase : IDataBase
    {
        private SQLiteAsyncConnection _Database;
        private const string _DatabaseFilename = "TodoSQLite.db3";
        private const SQLite.SQLiteOpenFlags _Flags =
       SQLite.SQLiteOpenFlags.ReadWrite |
       SQLite.SQLiteOpenFlags.Create |
       SQLite.SQLiteOpenFlags.SharedCache;
        private static string _DbPath => Path.Combine(FileSystem.AppDataDirectory, _DatabaseFilename);

        public void Init()
        {
            if (_Database == null)
            {
                _Database = new SQLiteAsyncConnection(_DbPath, _Flags);
                _Database.CreateTableAsync<Ingredient>();
            }
        }
        public async Task<int> AddItemAsync(ListIngredients list)
        {
            Init();
            int idlist = await GetNewIdList();
            foreach (Ingredient ingredient in list.Ingredients)
            {
                ingredient.IdListIngredients = idlist;
                ingredient.NameList = list.Name;
                await _Database.InsertAsync(ingredient);
            }
            return idlist;
        }
        public async Task<bool> ModifyItemAsync(ListIngredients list)
        {
            Init();
            int idlist = list.Ingredients[0].IdListIngredients;
            foreach (Ingredient ingredient in list.Ingredients)
            {
                if (ingredient.Id==-1)
                {
                    ingredient.IdListIngredients = idlist;
                    ingredient.NameList = list.Name;
                    await _Database.InsertAsync(ingredient);
                }
                else
                {
                    await _Database.UpdateAsync(ingredient);
                }
            }
            return true;
        }
        private async Task<int> GetNewIdList()
        {
            Init();
            List<Ingredient> list = await _Database.Table<Ingredient>().ToListAsync();
            if (list != null)
            {
                if (list.Count != 0)
                {
                    int id = 0;
                    foreach (Ingredient ingredient in list)
                    {
                        if (ingredient.IdListIngredients > id)
                        {
                            id = ingredient.IdListIngredients;
                        }
                    }
                    return id + 1;
                }
            }
            return 0;
        }
        public async Task<List<Ingredient>> GetListItemsAsync(int id)
        {
            Init();
            List<Ingredient> ing = await _Database.Table<Ingredient>().Where(i => i.IdListIngredients == id).ToListAsync();
            return ing;
        }
        public async Task<ObservableCollection<ListIngString>> GetAllListStringAsync()
        {
            Init();
            ObservableCollection<ListIngString> lists = new ObservableCollection<ListIngString>();
            List<Ingredient> ing = await _Database.Table<Ingredient>().ToListAsync();
            foreach (Ingredient ingredient in ing)
            {
                ListIngString listIngString = lists.FirstOrDefault(lists=> lists.IdList==ingredient.IdListIngredients );
                if (listIngString==null)
                {
                    lists.Add(new ListIngString(ingredient));
                }
                else
                {
                    listIngString.AddIngredient(ingredient);
                }
            }
                return lists;
        }
            public async Task<bool> DelateLisatAsync(int id)
        {
            Init();
            List<Ingredient> ing = await _Database.Table<Ingredient>().Where(i => i.IdListIngredients == id).ToListAsync();
            bool delete = true;
            foreach (Ingredient ingredient in ing)
            {
               if( await _Database.DeleteAsync(ingredient)!=1)
                {
                    delete = false;
                }
            }
            return delete;
        }
    }
}
