
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
       private List<Ingredient> mainlist;
        public MainPage(IDataBase dataBase)
        {
            _dataBase = dataBase;
            InitializeComponent();
            SetSizeElement();
            SetFirstList();
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                ObservableCollection<IngredientString> ingredients = (ObservableCollection<IngredientString>)ListIng.ItemsSource;
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
            HideKeyBoard();
            base.OnAppearing();
            if (Preferences.Default.Get("LoadList", false))
            { 
            SetFirstList();
                Preferences.Default.Set("LoadList",false);
             }
        }
         
        private void SetSizeElement()
        {
            Banner.AdsId = "ca-app-pub-3940256099942544/9214589741";
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            var width = displayInfo.Width / displayInfo.Density;
            var myNumberStyle =(Style) Resources["NumberStyle"];
            myNumberStyle.Setters.FirstOrDefault().Value = width / 8.0;
            var myMainEntryStyle = (Style)Resources["MainEntryStyle"];
            myMainEntryStyle.Setters.FirstOrDefault().Value = width / 4.0;
            NumberName.WidthRequest = width/8.0;
            NameName.WidthRequest = width/4.0;
            ProcentName.WidthRequest = width / 4.0;
            MassName.WidthRequest = width / 4.0;
            SaveName.WidthRequest = (width-40.0) / 3.0;
            ShowMoreName.WidthRequest = (width - 40) / 3;
            AddName.WidthRequest = (width - 40.0) / 3.0;
            TotalMess.WidthRequest = (width - 20.0) /3.0;
            TotalMessLable.WidthRequest = (width-20.0)*2.0/3.0;      

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
            ObservableCollection<IngredientString> ingredients = new ObservableCollection<IngredientString>();
            int id = Preferences.Default.Get("idlist", -1);
            if (id>=0)
            {
                 mainlist = await _dataBase.GetListItemsAsync(id);
            }
            else
            {
                mainlist = new List<Ingredient>();
                mainlist.Add(new Ingredient( 1, "A", 52, 26));
                mainlist.Add(new Ingredient( 2, "B",24.5, 12.25));
                mainlist.Add(new Ingredient( 3, "C", 15.5, 7.75));
                mainlist.Add(new Ingredient( 4, "D", 8, 4));
            }
            foreach(Ingredient ingredient in mainlist)
            {
                ingredients.Add(new IngredientString(ingredient));
            }
            CorrectPercent(ingredients);
            ListIng.ItemsSource = ingredients;
            UpdateTotalMass();
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
        private void UpdateTotalMass()
        {
            double totalMass = 0;
            foreach (Ingredient ingredient in mainlist)
            {
                totalMass += ingredient.Mass;
            }
            TotalMess.Text = Convert.ToString(Math.Round(totalMass,5));
            TotalMess.CursorPosition = 0;
        }
        private bool CorrectPercent(ObservableCollection<IngredientString> Ingredients)
        {

                double suma = 0;
                foreach (Ingredient ingredient in mainlist)
                {
                    suma += ingredient.Percent;
                }

                if (suma == 100.0)
                {
                    AppTheme currentTheme = Application.Current.RequestedTheme;
                    foreach (IngredientString ingredient in Ingredients)
                    {
                        if (currentTheme == AppTheme.Dark)
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
                    foreach (IngredientString ingredient in Ingredients)
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
                    int index = Convert.ToInt32(entry.ClassId) - 1;
                    mainlist[index].Mass = 0;
                    entry.IsReadOnly = true;
                    entry.Text = "0";
                }
                if (entry.Text[0] == ',' || entry.Text[0] == '.')
                {
                    entry.Text = "0" + entry.Text;
                }   
            }
        }
        private void FixValuesPercent(object sender, FocusEventArgs e)
        {
            if (sender != null)
            {
                Entry entry = sender as Entry;
                entry.CursorPosition = 0;
                double value;
                bool isValidmass = Double.TryParse(entry.Text, NumberStyles.Float, currentCulture, out value);
                if (!isValidmass)
                {

                    int index = Convert.ToInt32(entry.ClassId) - 1;
                    mainlist[index].Percent = 0;
                    entry.IsReadOnly = true;
                    entry.Text = "0";
                }
                if (value > 100)
                {
                    int index = Convert.ToInt32(entry.ClassId) - 1;
                    mainlist[index].Percent = 100;
                    entry.IsReadOnly = true;
                    entry.Text = "100";
                }
                else
                {
                    if (entry.Text[0] == ',' || entry.Text[0] == '.')
                    {
                        entry.Text = "0" + entry.Text;
                    }
                }
            }
        }
        private void ChangeMass(object sender, TextChangedEventArgs e)
        {
            if (sender != null)
            {
                int index;
                double mass, totalmass,percent;
                ObservableCollection<IngredientString> ingredients = (ObservableCollection<IngredientString>)ListIng.ItemsSource;
                Entry entry = sender as Entry;
                if (entry.IsFocused || entry.IsReadOnly==true) 
                {
                    if(entry.IsReadOnly == true)
                    {
                        entry.IsReadOnly = false;
                    }
                    bool isValidmass = Double.TryParse(e.NewTextValue, NumberStyles.Float, currentCulture, out mass);
                    if (isValidmass)
                    {
                        index = Convert.ToInt32(entry.ClassId) - 1;
                        percent = mainlist[index].Percent;
                        mainlist[index].Mass= mass;
                        ingredients[index].Mass = e.NewTextValue;
                        if (percent>0&&percent<=100) 
                        {

                            totalmass = Math.Round(mass * (100.0 / percent), 5);
                            for (int i = 0; i < ingredients.Count; i++)
                            {
                                if (i != index)
                                {
                                    percent = mainlist[i].Percent;
                                    if (percent != 0)
                                    {
                                        mainlist[i].Mass = Math.Round(totalmass * percent / 100.0, 5);
                                        ingredients[i].Mass = Convert.ToString(mainlist[i].Mass);
                                    }
                                    else
                                    {
                                        mainlist[i].Mass = 0;
                                        ingredients[i].Mass = "0";
                                    }
                                }
                            }
                            ListIng.ItemsSource = ingredients;
                            TotalMess.Text = Convert.ToString(totalmass);
                            TotalMess.CursorPosition = 0;
                        }
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
                int i = 0;
                if (entry.IsFocused || entry.IsReadOnly == true)
                {
                    if (entry.IsReadOnly == true)
                    {
                        entry.IsReadOnly = false;
                    }
                    ObservableCollection<IngredientString> ingredients = (ObservableCollection<IngredientString>)ListIng.ItemsSource;
                    double TotalMassValue, newvalue;
                    int index = Convert.ToInt32(entry.ClassId)-1;
                    bool isValidTotalMassValue = Double.TryParse(TotalMess.Text, NumberStyles.Float, currentCulture, out TotalMassValue);
                    bool isValidnewvalue = Double.TryParse(e.NewTextValue, NumberStyles.Float, currentCulture, out newvalue);
                    
                    if (isValidTotalMassValue && isValidnewvalue)
                    {
                         ingredients[index].Percent = e.NewTextValue;
                        mainlist[index].Percent = newvalue;
                        if (CorrectPercent(ingredients))
                        {
                            foreach (Ingredient ingredientlist in mainlist)
                            {
                                ingredientlist.Mass = Math.Round(TotalMassValue * (ingredientlist.Percent / 100.0),5);
                                ingredients[i].Mass=Convert.ToString(ingredientlist.Mass);
                                i++;
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
                int i = 0;
                bool isValidmass = Double.TryParse(e.NewTextValue, NumberStyles.Float, currentCulture, out TotalMassValue);
                if (!isValidmass)
                {
                    TotalMassValue = 0;
                }
                ObservableCollection<IngredientString> ingredients = (ObservableCollection<IngredientString>)ListIng.ItemsSource;
                if (ingredients!=null)
                { 
                foreach (Ingredient ingredientlist in mainlist)
                {
                        ingredientlist.Mass = Math.Round(TotalMassValue * (ingredientlist.Percent / 100.0), 5);
                        ingredients[i].Mass = Convert.ToString(ingredientlist.Mass);
                        i++;
                }
                }
            }
        }
        private void DelateIngredient(object sender, EventArgs args)
        {
            if (sender != null)
            {
                ImageButton button = sender as ImageButton;
                ObservableCollection<IngredientString> ingredients = (ObservableCollection<IngredientString>)ListIng.ItemsSource;
                if (ingredients != null)
                {
                    int index = Convert.ToInt32(button.ClassId) - 1;
                    if (index >= 0 && index < ingredients.Count)
                    {
                        if (ingredients.Remove(ingredients[index]) && mainlist.Remove(mainlist[index]))
                        {
                            HideKeyBoard();
                            for (int i = index; i < ingredients.Count; i++)
                            {
                                ingredients[i].Index--;
                                mainlist[i].Index--;
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
                ObservableCollection<IngredientString> ingredients = (ObservableCollection<IngredientString>)ListIng.ItemsSource;
                if (ingredients != null)
                {
                    if (ingredients.Count > 0)
                    {
                        mainlist.Add(new Ingredient(ingredients[ingredients.Count - 1].Index + 1));
                        ingredients.Add(new IngredientString(new Ingredient(ingredients[ingredients.Count - 1].Index + 1)));
                    }
                    else
                    {
                        mainlist.Add(new Ingredient(1));
                        ingredients.Add(new IngredientString(new Ingredient(1)));
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
                if (mainlist!=null) 
                {
                    await Navigation.PushModalAsync(new SavePage(_dataBase, mainlist));
                }

            }
        }
    }
}
