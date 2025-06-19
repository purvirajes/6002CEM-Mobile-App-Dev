using ExpensesApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExpensesApp
{
    public partial class AppShell : Shell
    {
        public AppShell(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            // Register all your routes here
            RegisterRoutes();

            this.Loaded += (s, e) => CheckAuthState();
        }

        private readonly AuthService _authService;

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("AddExpensePage", typeof(Views.AddExpensePage));
            Routing.RegisterRoute("EditExpensePage", typeof(Views.EditExpensePage));
            Routing.RegisterRoute("LoginPage", typeof(Views.LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(Views.RegisterPage));
            Routing.RegisterRoute("MainAppPage", typeof(Views.MainAppPage)); // optional but good for consistency
            Routing.RegisterRoute("MainPage", typeof(MainPage)); // optional but good for consistency
        }


        private async void CheckAuthState()
        {
            var isLoggedIn = await _authService.IsLoggedIn();
            await Shell.Current.GoToAsync(isLoggedIn ? "//MainAppPage" : "//LoginPage");
        }
    }
}