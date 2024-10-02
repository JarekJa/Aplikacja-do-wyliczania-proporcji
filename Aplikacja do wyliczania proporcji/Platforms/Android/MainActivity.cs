using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.MauiMTAdmob;



namespace Aplikacja_do_wyliczania_proporcji
{
    [Activity(Label = "@string/app_name", Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

           CrossMauiMTAdmob.Current.Init(this, "");
        }
    }
}
