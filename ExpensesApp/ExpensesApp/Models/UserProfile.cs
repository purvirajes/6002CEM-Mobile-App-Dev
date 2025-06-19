using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("user_profiles")]
public class UserProfile : BaseModel
{
    [Column("user_id")]
    public string UserId { get; set; }

    [Column("budget_limit")]
    public decimal BudgetLimit { get; set; } = 1000; // Default value
}