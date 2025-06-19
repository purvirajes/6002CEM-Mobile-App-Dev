using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ExpensesApp.Models;

[Table("expenses")]
public class Expense : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("cost")]
    public decimal Cost { get; set; }

    [Column("category")]
    public string Category { get; set; }

    [Column("date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Column("user_id")]
    public string UserId { get; set; }
}