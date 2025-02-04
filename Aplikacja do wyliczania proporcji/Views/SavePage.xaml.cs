using Aplikacja_do_wyliczania_proporcji.Models;
using Aplikacja_do_wyliczania_proporcji.Sercices.Interfaces;
using CommunityToolkit.Maui.Core.Platform;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Aplikacja_do_wyliczania_proporcji.Views;

public partial class SavePage : ContentPage
{
    private readonly IDataBase _dataBase;
    private readonly List<Ingredient> _ingredients;
    public SavePage(IDataBase dataBase, List<Ingredient> ingredients)
	{
       _dataBase = dataBase;
       _ingredients = ingredients;
        InitializeComponent();
        SettingSiveElements();
        SetList();

    }
    private void SetList() 
    {
        string list = "";
        foreach(Ingredient ingredient in _ingredients)
        {
            if (ingredient.Name.Length<=6)
            {
                list += ingredient.Name + ":" + Convert.ToString(ingredient.Percent) + "%," + Convert.ToString(ingredient.Mass) + "; ";
            }
            else
            {

                list += ingredient.Name.Substring(0, 6) + ":" + Convert.ToString(ingredient.Percent) + "%," + Convert.ToString(ingredient.Mass) + "; ";
            }
        
        }
        ListName.Text = list;
        if (_ingredients[0].NameList!=null)
        {
            NameEntry.Text = _ingredients[0].NameList;
            ModB.IsVisible = true;
        }
    }
    private void SettingSiveElements()
    {
        var displayInfo = DeviceDisplay.MainDisplayInfo;
        var width = displayInfo.Width / displayInfo.Density;
        Name.WidthRequest = width/2 - width*0.1;
        NameEntry.WidthRequest = width/ 2 - width * 0.1;
        SaveB.WidthRequest =  width / 2 - width * 0.1;
        ReturnB.WidthRequest= width / 2 - width * 0.1;
        ModB.WidthRequest = width / 2 - width * 0.1;
    }
    private async void Return(object sender, EventArgs args)
    {
        if (NameEntry.IsSoftKeyboardShowing())
        {
           await NameEntry.HideKeyboardAsync();
        }
        await Navigation.PopModalAsync() ;
    }

    private async void Save(object sender, EventArgs args)
    {
        if (NameEntry.Text!=null)
        {
            if (NameEntry.Text!="")
            {
                if (NameEntry.IsSoftKeyboardShowing())
                {
                    await NameEntry.HideKeyboardAsync();
                }
                ListIngredients listIngredient = new ListIngredients(NameEntry.Text, _ingredients);
              int id=  await _dataBase.AddItemAsync(listIngredient);
                Preferences.Default.Set("idlist", id);
                await Navigation.PopModalAsync();
            }
        }
        NameEntry.BackgroundColor = Colors.Red;
    }
    private async void Modify(object sender, EventArgs args)
    {
        if (NameEntry.Text != null)
        {
            if (NameEntry.Text != "")
            {
                if (NameEntry.IsSoftKeyboardShowing())
                {
                    await NameEntry.HideKeyboardAsync();
                }
                ListIngredients listIngredient = new ListIngredients(NameEntry.Text, _ingredients);
                await _dataBase.ModifyItemAsync(listIngredient);
                await Navigation.PopModalAsync();
            }
        }
        NameEntry.BackgroundColor = Colors.Red;
    }
}