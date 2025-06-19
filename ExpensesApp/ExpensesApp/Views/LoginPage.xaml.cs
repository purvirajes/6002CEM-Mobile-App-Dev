using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Supabase.Gotrue;

namespace ExpensesApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly Supabase.Client _supabase;
        public LoginPage(Supabase.Client supabase)
        {
            InitializeComponent();
            _supabase = supabase;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("MainPage");
        }


        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both email and password", "OK");
                return;
            }

            try
            {
                var loadingToast = Toast.Make("Logging in...", ToastDuration.Short, 14);
                await loadingToast.Show();

                var session = await _supabase.Auth.SignIn(email, password);

                await loadingToast.Dismiss();
                await Toast.Make("Login successful!", ToastDuration.Short, 14).Show();

                // Navigate to main app page
                await Shell.Current.GoToAsync("//MainAppPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Login Failed", "Invalid Credentials", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }
    }
}