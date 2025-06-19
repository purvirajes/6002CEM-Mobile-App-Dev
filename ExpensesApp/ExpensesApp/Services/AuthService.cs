using Supabase;
using Supabase.Gotrue;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ExpensesApp.Services
{
    public class AuthService
    {
        private readonly Supabase.Client _supabase;
        private readonly ExpenseService _expenseService;

        public User? CurrentUser => _supabase.Auth.CurrentUser;
        public Session? CurrentSession => _supabase.Auth.CurrentSession;


        public AuthService(Supabase.Client supabase, ExpenseService expenseService)
        {
            _supabase = supabase;
            _expenseService = expenseService;
        }

        public async Task<bool> IsLoggedIn()
        {
            try
            {
                var session = await _supabase.Auth.RetrieveSessionAsync();
                return session != null && session.User != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking login status: {ex}");
                return false;
            }
        }

        public async Task<AuthResponse> SignUp(string email, string password)
        {
            try
            {
                var result = await _supabase.Auth.SignUp(email, password);

                if (result?.User?.Id != null)
                {
                    // Create user profile with default budget
                    await _expenseService.UpdateBudgetLimit(result.User.Id, 1000);
                    return new AuthResponse { Success = true, User = result.User };
                }

                return new AuthResponse { Success = false, Message = "User creation failed" };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SignUp error: {ex}");
                return new AuthResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<AuthResponse> SignIn(string email, string password)
        {
            try
            {
                var result = await _supabase.Auth.SignIn(email, password);
                return new AuthResponse { Success = true, User = result.User };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SignIn error: {ex}");
                return new AuthResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<AuthResponse> SignOut()
        {
            try
            {
                await _supabase.Auth.SignOut();
                return new AuthResponse { Success = true };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SignOut error: {ex}");
                return new AuthResponse { Success = false, Message = ex.Message };
            }
        }
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public User? User { get; set; }
    }
}