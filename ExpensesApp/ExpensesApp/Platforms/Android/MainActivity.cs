using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ExpensesApp
{
    [Activity(
        Label = "Expenses", 
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Android.Util.Log.Debug("MAUI_DEBUG", "App starting...");
            base.OnCreate(savedInstanceState);
            if (OperatingSystem.IsAndroid())
            {
                Microsoft.Maui.ApplicationModel.Platform.Init(this, savedInstanceState);
            }

        }

    }
}
