using ExpensesApp.Models;
using ExpensesApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace ExpensesApp.Views
{
    public partial class MainAppPage : ContentPage
    {
        private readonly AuthService _authService;
        private readonly ExpenseService _expenseService;
        public ObservableCollection<Expense> Expenses { get; } = new();
        public ICommand EditExpenseCommand { get; }


        public MainAppPage(AuthService authService, ExpenseService expenseService)
        {
            InitializeComponent();
            _authService = authService;
            _expenseService = expenseService;
            BindingContext = this;

            EditExpenseCommand = new Command<Expense>(async (expense) =>
            {
                try
                {
                    if (expense == null)
                    {
                        await DisplayAlert("Error", "Invalid expense selected", "OK");
                        return;
                    }

                    Debug.WriteLine($"Attempting to edit expense ID: {expense.Id}");

                    await Shell.Current.GoToAsync($"{nameof(EditExpensePage)}?id={expense.Id}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"CRITICAL ERROR in navigation: {ex}");
                    await DisplayAlert("Error", "Failed to open editor", "OK");
                }
            });

            LoadData();
        }


        private async Task EditExpense(Expense expense)
        {
            await Shell.Current.GoToAsync($"{nameof(EditExpensePage)}",
                new Dictionary<string, object>
                {
            { "Expense", expense }
                });
        }

        private async void LoadData()
        {
            try
            {
                var userId = _authService.CurrentUser?.Id;
                if (string.IsNullOrEmpty(userId)) return;

                var expenses = await _expenseService.GetUserExpenses(userId);
                Expenses.Clear();
                foreach (var expense in expenses)
                {
                    Expenses.Add(expense);
                }

                // Load budget limit from Supabase
                var budgetLimit = await _expenseService.GetBudgetLimit(userId);
                LimitEntry.Text = budgetLimit.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex}");
                await DisplayAlert("Error", "Failed to load data", "OK");
            }
        }

        private async void OnAddExpenseClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AddExpensePage");
        }

        private async void OnUpdateLimitClicked(object sender, EventArgs e)
        {
            try
            {
                if (decimal.TryParse(LimitEntry.Text, out var newLimit))
                {
                    var userId = _authService.CurrentUser?.Id;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var success = await _expenseService.UpdateBudgetLimit(userId, newLimit);
                        if (success)
                        {
                            await DisplayAlert("Success", "Budget limit updated", "OK");
                            // No need to reload data here since we're just updating the displayed value
                        }
                        else
                        {
                            await DisplayAlert("Error", "Failed to update budget limit", "OK");
                            // Reload the original value if update failed
                            var currentLimit = await _expenseService.GetBudgetLimit(userId);
                            LimitEntry.Text = currentLimit.ToString();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Please enter a valid amount", "OK");
                    // Reload the original value if input was invalid
                    var userId = _authService.CurrentUser?.Id;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var currentLimit = await _expenseService.GetBudgetLimit(userId);
                        LimitEntry.Text = currentLimit.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating limit: {ex}");
                await DisplayAlert("Error", "Failed to update budget limit", "OK");
            }
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            try
            {
                await _authService.SignOut();
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error signing out: {ex}");
                await DisplayAlert("Error", "Failed to sign out", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();
        }

    }
}