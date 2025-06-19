using ExpensesApp.Models;
using ExpensesApp.Services;
using System.Windows.Input;
using System.Diagnostics;

namespace ExpensesApp.ViewModels
{
    public class EditExpenseViewModel : BaseViewModel
    {
        private readonly Expense _originalExpense;
        private readonly ExpenseService _expenseService;

        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Category { get; set; }

        public ICommand UpdateExpenseCommand { get; }
        public ICommand DeleteExpenseCommand { get; }

        public EditExpenseViewModel(Expense expense, ExpenseService expenseService)
        {
            try
            {
                if (expense == null) throw new ArgumentNullException(nameof(expense));
                if (expenseService == null) throw new ArgumentNullException(nameof(expenseService));

                _originalExpense = expense;
                _expenseService = expenseService;

                Name = expense.Name;
                Cost = expense.Cost;
                Category = expense.Category;

                UpdateExpenseCommand = new Command(async () => await UpdateExpense());
                DeleteExpenseCommand = new Command(async () => await DeleteExpense());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ViewModel init error: {ex}");
                throw;
            }
        }

        private async Task UpdateExpense()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name) || Cost <= 0 || string.IsNullOrWhiteSpace(Category))
                {
                    await Shell.Current.DisplayAlert("Error", "Please fill all fields correctly", "OK");
                    return;
                }

                _originalExpense.Name = Name;
                _originalExpense.Cost = Cost;
                _originalExpense.Category = Category;

                var success = await _expenseService.UpdateExpense(_originalExpense);
                if (success)
                {
                    await Shell.Current.DisplayAlert("Success", "Expense updated", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to update expense", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Update error: {ex}");
                await Shell.Current.DisplayAlert("Error", "An error occurred", "OK");
            }
        }

        private async Task DeleteExpense()
        {
            try
            {
                var confirm = await Shell.Current.DisplayAlert(
                    "Confirm Delete",
                    "Are you sure you want to delete this expense?",
                    "Yes", "No");

                if (confirm)
                {
                    var success = await _expenseService.DeleteExpense(_originalExpense.Id);
                    if (success)
                    {
                        await Shell.Current.DisplayAlert("Success", "Expense edited successfully!", "OK");
                        await Shell.Current.GoToAsync("//MainAppPage");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Failed to delete expense", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Delete error: {ex}");
                await Shell.Current.DisplayAlert("Error", "Failed to delete expense", "OK");
            }
        }
    }
}