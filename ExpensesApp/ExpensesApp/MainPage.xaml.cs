using Microsoft.Extensions.DependencyInjection;
using Supabase;

namespace ExpensesApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var supabase = MauiProgram.CreateMauiApp().Services.GetRequiredService<Supabase.Client>();
            await Navigation.PushAsync(new Views.LoginPage(supabase));
            await Shell.Current.GoToAsync("LoginPage");

        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var supabase = MauiProgram.CreateMauiApp().Services.GetRequiredService<Supabase.Client>();
            await Navigation.PushAsync(new Views.RegisterPage(supabase));
        }
    }
}