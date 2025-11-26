using Aplikacja_do_wyliczania_proporcji.Models;
using Aplikacja_do_wyliczania_proporcji.Sercices.Interfaces;
using Plugin.MauiMTAdmob;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Aplikacja_do_wyliczania_proporcji.Views;

public partial class ListsPage : ContentPage
{
    private readonly IDataBase _dataBase;
    public ListsPage(IDataBase dataBase)
    {
        _dataBase = dataBase;
        InitializeComponent();
        SetSizeElement();
        SetItemsSource();
        this.SizeChanged += OnPageSizeChanged;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Interstitial();
        SetItemsSource();
    }
    private void SetSizeElement()
    {
        var displayInfo = DeviceDisplay.MainDisplayInfo;
        var width = displayInfo.Width / displayInfo.Density;
        width = (width - 50) * 2 / 7;
        Banner.AdsId = "ca-app-pub-3940256099942544/9214589741";
        CrossMauiMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");
        CrossMauiMTAdmob.Current.OnInterstitialLoaded += (sender, args) =>
        {
            DateTime dataInterstitial = Preferences.Default.Get("DataInterstitial", new DateTime(0));
            Interstitial();
        };
        var myNameLableStyle = (Style)Resources["NameLableStyle"];
        myNameLableStyle.Setters.FirstOrDefault().Value = width;
        var myCountLableStyle = (Style)Resources["CountLableStyle"];
        myCountLableStyle.Setters.FirstOrDefault().Value = width;
        var myButtonStyle = (Style)Resources["ButtonStyle"];
        myButtonStyle.Setters.FirstOrDefault().Value = width;
    }
    private void Interstitial()
    {
        DateTime dateTime = DateTime.Now;
        DateTime date = new DateTime(0);
        DateTime dataInterstitial = Preferences.Default.Get("DataInterstitial", date);
        if (dataInterstitial== date)
        {
            CrossMauiMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");
            if (CrossMauiMTAdmob.Current.IsInterstitialLoaded())
            {
                CrossMauiMTAdmob.Current.ShowInterstitial();
                Preferences.Default.Set("DataInterstitial", dateTime);
            }
         }
        else
        {
            TimeSpan difference = dateTime - dataInterstitial;
            if (difference.Minutes>=10)
            {
                if (CrossMauiMTAdmob.Current.IsInterstitialLoaded())
                {
                    CrossMauiMTAdmob.Current.ShowInterstitial();
                }
                Preferences.Default.Set("DataInterstitial", dateTime);
            }
        }
    }
    private void OnPageSizeChanged(object sender, EventArgs e)
    {
        var height = this.Bounds.Height;
        ListLists.MaximumHeightRequest = height - 80 - Recipe.Height;
    }
        private async Task SetItemsSource()
    {
        ListLists.ItemsSource = await _dataBase.GetAllListStringAsync();
    }
    private async void ChooseList(object sender, EventArgs args)
    {
        if (sender != null)
        {
            Button button = sender as Button;
            int id = Convert.ToInt32(button.ClassId);
            Preferences.Default.Set("idlist", id);
            Preferences.Default.Set("LoadList", true);
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
        private void DelateList(object sender, EventArgs args)
    {
        if (sender != null)
        {
            ObservableCollection<ListIngString> lists = (ObservableCollection<ListIngString>)ListLists.ItemsSource;
            ImageButton button = sender as ImageButton;
            int id = Convert.ToInt32(button.ClassId);
            ListIngString listIngredients = lists.Where(sc => sc.IdList == id).FirstOrDefault();
            if (listIngredients != null)
            {
                _dataBase.DelateLisatAsync(id);
                lists.Remove(listIngredients);
                ListLists.ItemsSource = new ObservableCollection<ListIngString>();
                ListLists.ItemsSource = lists;
              if(  Preferences.Default.Get("idlist", -1)==id)
                {
                    Preferences.Default.Set("idlist",-1) ;
                }
            }

        }
    }
}