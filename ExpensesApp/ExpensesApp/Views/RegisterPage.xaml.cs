using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Supabase.Gotrue;

namespace ExpensesApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        private readonly Supabase.Client _supabase;
        public RegisterPage(Supabase.Client supabase)
        {
            InitializeComponent();
            _supabase = supabase;
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("MainPage");
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text;
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;
            var confirmPassword = ConfirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "Please fill all required fields", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords don't match", "OK");
                return;
            }

            try
            {
                var loadingToast = Toast.Make("Creating account...", ToastDuration.Short, 14);
                await loadingToast.Show();

                var options = new SignUpOptions
                {
                    Data = new Dictionary<string, object>
                    {
                        { "username", username }
                    }
                };

                var session = await _supabase.Auth.SignUp(email, password, options);

                await loadingToast.Dismiss();

                if (session != null)
                {
                    await Toast.Make("Registration successful!", ToastDuration.Short, 14).Show();
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    await DisplayAlert("Error", "Registration failed", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Registration Failed", "Invalid Inputs", "OK");
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}