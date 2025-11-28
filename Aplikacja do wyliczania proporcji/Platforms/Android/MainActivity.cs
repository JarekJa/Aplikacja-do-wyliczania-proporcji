using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;
using Plugin.MauiMTAdmob;



namespace Aplikacja_do_wyliczania_proporcji
{
    [Activity(Label = "@string/app_name", Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            EnableEdgeToEdge();
            CrossMauiMTAdmob.Current.Init(this, "");
            Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
        }
        private void EnableEdgeToEdge()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.VanillaIceCream)
            {
                WindowCompat.SetDecorFitsSystemWindows(Window, false);
                var insetsController = WindowCompat.GetInsetsController(Window, Window.DecorView);
                if (insetsController != null)
                {
                    insetsController.Hide(WindowInsetsCompat.Type.SystemBars());
                    insetsController.SystemBarsBehavior = WindowInsetsControllerCompat.BehaviorShowTransientBarsBySwipe;
                }

            }

        }
    }
}

