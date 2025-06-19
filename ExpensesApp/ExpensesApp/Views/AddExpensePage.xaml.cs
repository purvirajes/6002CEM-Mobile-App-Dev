using ExpensesApp.Models;
using ExpensesApp.Services;
using System.Diagnostics;

namespace ExpensesApp.Views
{
    public partial class AddExpensePage : ContentPage
    {
        private readonly AuthService _authService;
        private readonly ExpenseService _expenseService;

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }


        public AddExpensePage(AuthService authService, ExpenseService expenseService)
        {
            InitializeComponent();
            _authService = authService;
            _expenseService = expenseService;
        }

        private async void OnSaveExpenseClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
                    !decimal.TryParse(CostEntry.Text, out var cost) ||
                    CategoryPicker.SelectedItem == null)
                {
                    await DisplayAlert("Error", "Please fill all fields correctly", "OK");
                    return;
                }

                var userId = _authService.CurrentUser?.Id;
                if (string.IsNullOrEmpty(userId))
                {
                    await DisplayAlert("Error", "User not authenticated. Please sign in again.", "OK");
                    return;
                }

                var newExpense = new Expense
                {
                    Name = NameEntry.Text.Trim(),
                    Cost = cost,
                    Category = CategoryPicker.SelectedItem.ToString(),
                    UserId = userId,
                    Date = DateTime.UtcNow
                };

                // The Supabase client automatically uses the current session
                var success = await _expenseService.AddExpense(newExpense);

                if (success)
                {
                    await DisplayAlert("Success", "Expense added successfully!", "OK");
                    await Shell.Current.GoToAsync("//MainAppPage");
                }
                else
                {
                    await DisplayAlert("Error",
                        "Failed to save expense. Check debug output for details.",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in OnSaveExpenseClicked: {ex}");
                await DisplayAlert("Error",
                    $"An unexpected error occurred: {ex.Message}",
                    "OK");
            }
        }
    }
}