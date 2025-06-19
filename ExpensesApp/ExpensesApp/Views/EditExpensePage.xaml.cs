using ExpensesApp.Models;
using ExpensesApp.Services;
using ExpensesApp.ViewModels;
using System.Diagnostics;

namespace ExpensesApp.Views
{
    [QueryProperty(nameof(ExpenseId), "id")]
    public partial class EditExpensePage : ContentPage
    {
        private readonly ExpenseService _expenseService;
        private string _expenseId;

        public string ExpenseId
        {
            get => _expenseId;
            set
            {
                _expenseId = value;
                LoadExpenseData(value);
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        public EditExpensePage(ExpenseService expenseService)
        {
            InitializeComponent();
            _expenseService = expenseService;
        }

        private async void LoadExpenseData(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id) && int.TryParse(id, out int expenseId))
                {
                    Debug.WriteLine($"Loading expense ID: {expenseId}");
                    var expense = await _expenseService.GetExpenseById(expenseId);

                    if (expense != null)
                    {
                        Debug.WriteLine($"Successfully loaded expense ID: {expenseId}");
                        BindingContext = new EditExpenseViewModel(expense, _expenseService);
                    }
                    else
                    {
                        Debug.WriteLine($"Expense ID {expenseId} not found");
                        await Shell.Current.GoToAsync("..");
                    }
                }
                else
                {
                    Debug.WriteLine("Invalid expense ID");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CRITICAL ERROR in EditExpensePage: {ex}");
                await Shell.Current.DisplayAlert("Error", "Failed to load expense", "OK");
                await Shell.Current.GoToAsync("..");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // This is now handled by the property setter
        }
    }
}