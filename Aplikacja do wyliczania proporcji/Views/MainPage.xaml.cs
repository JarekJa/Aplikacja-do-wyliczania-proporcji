
using Aplikacja_do_wyliczania_proporcji.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Platform;
using Aplikacja_do_wyliczania_proporcji.Sercices.Interfaces;
using Aplikacja_do_wyliczania_proporcji.Views;
using System.Globalization;




namespace Aplikacja_do_wyliczania_proporcji
{
    public partial class MainPage : ContentPage
    {
       private Entry _keyBoardEntry = null;
       private  readonly IDataBase _dataBase;
       private CultureInfo currentCulture = CultureInfo.CurrentCulture;
        public MainPage(IDataBase dataBase)
        {
            _dataBase = dataBase;
            InitializeComponent();
            SetSizeElement();
            SetFirstList();
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                ObservableCollection<Ingredient> ingredients = (ObservableCollection<Ingredient>)ListIng.ItemsSource;
                if(ingredients!=null)
                {
                    CorrectPercent(ingredients);
                    ListIng.ItemsSource = ingredients;
                }
            };
            this.SizeChanged += OnPageSizeChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Preferences.Default.Get("LoadList", false))
            { 
            SetFirstList();
                Preferences.Default.Set("LoadList",false);
             }
        }
         
        private void SetSizeElement()
        {
            Banner.AdsId = "";
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            var width = displayInfo.Width / displayInfo.Density;
            var myNumberStyle =(Style) Resources["NumberStyle"];
            myNumberStyle.Setters.FirstOrDefault().Value = width / 8;
            var myMainEntryStyle = (Style)Resources["MainEntryStyle"];
            myMainEntryStyle.Setters.FirstOrDefault().Value = width / 4;
            NumberName.WidthRequest = width/8;
            NameName.WidthRequest = width/4;
            ProcentName.WidthRequest = width / 4;
            MassName.WidthRequest = width / 4;
            SaveName.WidthRequest = (width-40) / 3;
            ShowMoreName.WidthRequest = (width - 40) / 3;
            AddName.WidthRequest = (width - 40) / 3;
            TotalMess.WidthRequest = (width - 20) /3;
            TotalMessLable.WidthRequest = (width-20)*2/3;      

        }
        private void OnPageSizeChanged(object sender, EventArgs e)
        {

            var height = this.Bounds.Height;
            var header = Header.Height;
            var footer = Footer.Height;
            if (_keyBoardEntry != null)
            {
                if (!_keyBoardEntry.IsSoftKeyboardShowing())
                {
                    _keyBoardEntry.Unfocus();
                    _keyBoardEntry = null;
                    ShowMoreName.IsVisible = false;
                    Banner.IsVisible = true;
                }
            }
            if (Banner.IsVisible)
            {
                ListIng.MaximumHeightRequest = height - header - footer- 55;
            }
            else
            {
                ListIng.MaximumHeightRequest = height - header - footer;
            }
        }


        private async Task SetFirstList()
        {
            ObservableCollection<Ingredient> ingredients;
            int id = Preferences.Default.Get("idlist", -1);
            if (id>=0)
            {
                 ingredients = await _dataBase.GetListItemsAsync(id);
            }
            else
            {
                ingredients = new ObservableCollection<Ingredient>();
                ingredients.Add(new Ingredient( 1, "A", "50", "50"));
                ingredients.Add(new Ingredient( 2, "B", "25", "25"));
                ingredients.Add(new Ingredient( 3, "C", "25", "25"));
            }
            CorrectPercent(ingredients);
            ListIng.ItemsSource = ingredients;
            UpdateTotalMass(ingredients);
        }
        private async Task HideKeyBoard()
        {
            if (_keyBoardEntry != null)
            {
                if (_keyBoardEntry.IsSoftKeyboardShowing())
                {
                   await  _keyBoardEntry.HideKeyboardAsync();
                }
                _keyBoardEntry.Unfocus();
                _keyBoardEntry = null; 
                ShowMoreName.IsVisible = false;
                Banner.IsVisible = true;
            }
        }
        private void UpdateTotalMass(ObservableCollection<Ingredient> Ingredients)
        {
            double totalMass = 0;
            foreach (Ingredient ingredient in Ingredients)
            {
                totalMass += Convert.ToDouble(ingredient.Mass);
            }
            TotalMess.Text = Convert.ToString(Math.Round(totalMass,5));
            TotalMess.CursorPosition = 0;
        }
        private bool CorrectPercent(ObservableCollection<Ingredient> Ingredients)
        {
            double suma = 0;
            foreach (Ingredient ingredient in Ingredients)
            {
                suma += Convert.ToDouble(ingredient.Percent);
            }
            if (suma == 100.0)
            {
                AppTheme currentTheme = Application.Current.RequestedTheme;
                foreach (Ingredient ingredient in Ingredients)
                {
                    if(currentTheme==AppTheme.Dark)
                    {
                        ingredient.PercentColor = "Black";
                    }
                    else
                    {
                        ingredient.PercentColor = "White";
                    }
                }
                if (currentTheme == AppTheme.Dark)
                {
                    ProcentName.BackgroundColor = Colors.Black;
                }
                else
                {
                    ProcentName.BackgroundColor = Colors.White;
                }
                return true;
            }
            else
            {
                foreach (Ingredient ingredient in Ingredients)
                {
                    ingredient.PercentColor = "Red";
                }
                ProcentName.BackgroundColor = Colors.Red;
                return false;
            }
        }
        private async void ShowSize(object sender, FocusEventArgs e)
        {
            if (sender != null)
            {
                Entry entry = sender as Entry;
                _keyBoardEntry = entry;
                ShowMoreName.IsVisible = true;
                Banner.IsVisible = false;
                if(!entry.IsSoftKeyboardShowing())
                {
                  await  entry.ShowKeyboardAsync();
                }
               
            }
        }
        private void FixValues(object sender, FocusEventArgs e)
        {
            if (sender != null)
            {
                Entry entry = sender as Entry;
                entry.CursorPosition = 0;
                double value;
                bool isValidmass = Double.TryParse(entry.Text, NumberStyles.Float, currentCulture, out value);
                if (!isValidmass)
                {
                    entry.Text = "0";
                }
                if (entry.Text[0] == ',' || entry.Text[0] == '.')
                {
                    entry.Text = "0" + entry.Text;
                }
                
            }
        }
        private void ChangeMass(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                double mass, totalmass,percent;
                ObservableCollection<Ingredient> ingredients = (ObservableCollection<Ingredient>)ListIng.ItemsSource;
                Entry entry = sender as Entry;
                if (entry.IsFocused) 
                {
                    bool isValidmass = Double.TryParse(e.NewTextValue, NumberStyles.Float, currentCulture, out mass);
                    if (isValidmass)
                    {
                        int index = Convert.ToInt32(entry.ClassId) - 1;
                        percent = Convert.ToDouble(ingredients[index].Percent);
                        ingredients[index].Mass = e.NewTextValue;
                        if (percent!=0)
                        {
                            totalmass = Math.Round( mass * (100.0 / percent),5);
                        }
                        else
                        {
                            totalmass = 0;
                        }
                        for (int i = 0; i < ingredients.Count; i++)
                        {
                            if (i!=index)
                            {
                                percent = Convert.ToDouble(ingredients[i].Percent);
                                if (percent!=0)
                                {
                                    ingredients[i].Mass = Convert.ToString(Math.Round(totalmass * percent / 100.0,5));
                                }
                                else
                                {
                                    ingredients[i].Mass = "0";
                                }
                            }
                        }
                        ListIng.ItemsSource = ingredients;
                        TotalMess.Text = Convert.ToString(totalmass);
                        TotalMess.CursorPosition = 0;
                    }
                }
                else
                {
                    entry.CursorPosition = 0;
                }
            }
        }
                private void ChangeMassPercent(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                Entry entry = sender as Entry;
                if (entry.IsFocused)
                {
                    ObservableCollection<Ingredient> ingredients = (ObservableCollection<Ingredient>)ListIng.ItemsSource;
                    double TotalMassValue, newvalue;
                    int index = Convert.ToInt32(entry.ClassId)-1;
                    bool isValidTotalMassValue = Double.TryParse(TotalMess.Text, NumberStyles.Float, currentCulture, out TotalMassValue);
                    bool isValidnewvalue = Double.TryParse(e.NewTextValue, NumberStyles.Float, currentCulture, out newvalue);
                    
                    if (isValidTotalMassValue && isValidnewvalue)
                    {
                        if (newvalue <= 100)
                        {
                            ingredients[index].Percent = e.NewTextValue;
                        }
                        else
                        {
                            ingredients[index].Percent = "100";
                        }
                        if (CorrectPercent(ingredients))
                        {
                            foreach (Ingredient ingredient in ingredients)
                            {
                                ingredient.Mass = Convert.ToString(Math.Round(TotalMassValue * (Convert.ToDouble(ingredient.Percent) / 100.0),5));
                            }
                        }
                    }
                }
                else
                {
                    entry.CursorPosition = 0;
                }
            }
        }

        private void ChangeMassTotalMass(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                double TotalMassValue;
                bool isValidmass = Double.TryParse(e.NewTextValue, NumberStyles.Float, currentCulture, out TotalMassValue);
                if (!isValidmass)
                {
                    TotalMassValue = 0;
                }
                ObservableCollection<Ingredient> ingredients = (ObservableCollection<Ingredient>)ListIng.ItemsSource;
                foreach (Ingredient ingredient in ingredients)
                {
                    ingredient.Mass = Convert.ToString(Math.Round(TotalMassValue * (Convert.ToDouble(ingredient.Percent) / 100.0),5));
                }
            }
        }
        private void DelateIngredient(object sender, EventArgs args)
        {
            if (sender != null)
            {
                ImageButton button = sender as ImageButton;
                ObservableCollection<Ingredient> ingredients = (ObservableCollection<Ingredient>)ListIng.ItemsSource;
                if (ingredients != null)
                {
                    int index = Convert.ToInt32(button.ClassId) - 1;
                    if (index >= 0 && index < ingredients.Count)
                    {
                        if (ingredients.Remove(ingredients[index]))
                        {
                            HideKeyBoard();
                            for (int i = index; i < ingredients.Count; i++)
                            {
                                ingredients[i].Index--;
                            }
                            CorrectPercent(ingredients);
                            ListIng.ItemsSource = null;
                            ListIng.ItemsSource = ingredients.ToList();
                            ListIng.ItemsSource = ingredients;

                        }
                    }
                }
            }
        }
        private void AddIngredient(object sender, EventArgs args)
        {
            if (sender != null)
            {
                HideKeyBoard();
                ObservableCollection<Ingredient> ingredients = (ObservableCollection<Ingredient>)ListIng.ItemsSource;
                if (ingredients != null)
                {
                    if (ingredients.Count > 0)
                    {
                        ingredients.Add(new Ingredient(ingredients[ingredients.Count - 1].Index + 1));
                    }
                    else
                    {
                        ingredients.Add(new Ingredient(1));
                    }
                    CorrectPercent(ingredients);
                    ListIng.ItemsSource = ingredients;
                }
            }
        }
        private void ShowMore(object sender, EventArgs args)
        {
            if (sender != null)
            {
                HideKeyBoard();
            }
        }
        private async void Save(object sender, EventArgs args)
        {
            if (sender != null)
            {
                HideKeyBoard();
                ObservableCollection<Ingredient> ingredients =( ObservableCollection<Ingredient>)ListIng.ItemsSource;
                if (ingredients.Count!=0) 
                {
                    await Navigation.PushModalAsync(new SavePage(_dataBase, ingredients));
                }

            }
        }
    }
}
