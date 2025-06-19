using ExpensesApp.Models;
using Supabase;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Supabase.Postgrest;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace ExpensesApp.Services
{
    public class ExpenseService
    {
        private readonly Supabase.Client _supabase;

        public ExpenseService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<Expense>> GetUserExpenses(string userId)
        {
            try
            {
                var response = await _supabase
                    .From<Expense>()
                    .Filter("user_id", Constants.Operator.Equals, userId)
                    .Get();

                return response.Models;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting expenses: {ex}");
                return new List<Expense>();
            }
        }

        public async Task<bool> AddExpense(Expense expense)
        {
            try
            {
                if (expense == null || string.IsNullOrEmpty(expense.UserId))
                {
                    Debug.WriteLine("Invalid expense data");
                    return false;
                }

                var response = await _supabase
                    .From<Expense>()
                    .Insert(expense);

                return response.ResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in AddExpense: {ex}");
                return false;
            }
        }

        public async Task<bool> UpdateBudgetLimit(string userId, decimal newLimit)
        {
            try
            {
                // Use upsert (update or insert) pattern
                var newProfile = new UserProfile
                {
                    UserId = userId,
                    BudgetLimit = newLimit
                };

                // This will either insert or update the existing record
                var response = await _supabase
                    .From<UserProfile>()
                    .Upsert(newProfile,
                        new QueryOptions { Returning = QueryOptions.ReturnType.Representation });

                // Check if the operation was successful
                if (response.ResponseMessage.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Budget limit updated successfully");
                    return true;
                }
                else
                {
                    var errorContent = await response.ResponseMessage.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Failed to update budget limit: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in UpdateBudgetLimit: {ex}");
                return false;
            }
        }

        public async Task<decimal> GetBudgetLimit(string userId)
        {
            try
            {
                var response = await _supabase
                    .From<UserProfile>()
                    .Filter(x => x.UserId, Constants.Operator.Equals, userId)
                    .Single();

                return response?.BudgetLimit ?? 1000; // Fallback to 1000 if not found
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting budget limit: {ex}");
                return 1000;
            }
        }

        public async Task<bool> UpdateExpense(Expense expense)
        {
            try
            {
                var response = await _supabase
                    .From<Expense>()
                    .Where(x => x.Id == expense.Id)
                    .Set(x => x.Name, expense.Name)
                    .Set(x => x.Cost, expense.Cost)
                    .Set(x => x.Category, expense.Category)
                    .Update();

                return response.ResponseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating expense: {ex}");
                return false;
            }
        }

        // In ExpenseService.cs
        public async Task<bool> DeleteExpense(int expenseId)
        {
            try
            {
                await _supabase
                    .From<Expense>()
                    .Where(x => x.Id == expenseId)
                    .Delete();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting expense: {ex}");
                return false;
            }
        }

        public async Task<Expense> GetExpenseById(int id)
        {
            try
            {
                var response = await _supabase
                    .From<Expense>()
                    .Where(x => x.Id == id)
                    .Single();

                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting expense: {ex}");
                return null;
            }
        }

    }

}