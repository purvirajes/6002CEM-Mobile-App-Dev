using Microsoft.Extensions.Logging;
using Supabase;
using CommunityToolkit.Maui;
using ExpensesApp.Services;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using System.Text.Json;

namespace ExpensesApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Supabase configuration
        var supabaseUrl = "https://ycdlipgbhdzbjzsbxlsd.supabase.co";
        var supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InljZGxpcGdiaGR6Ymp6c2J4bHNkIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQyODk1ODcsImV4cCI6MjA1OTg2NTU4N30.yYPYQMJ51YN21lSoAnWs45noNAuIWfT6n9rdgko1VEo";


        builder.Services.AddSingleton(provider =>
            new Supabase.Client(supabaseUrl, supabaseKey, new Supabase.SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true,
                SessionHandler = new SupabaseSessionHandler(),
            }));

        // Register services
        builder.Services.AddSingleton<AppShell>();

        // Register pages
        builder.Services.AddTransient<Views.LoginPage>();
        builder.Services.AddTransient<Views.RegisterPage>();
        builder.Services.AddTransient<Views.MainAppPage>();
        builder.Services.AddTransient<Views.AddExpensePage>();

        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<ExpenseService>();
        builder.Services.AddSingleton<Views.EditExpensePage>();
        builder.Services.AddTransient<ViewModels.EditExpenseViewModel>();




#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

public class SupabaseSessionHandler : IGotrueSessionPersistence<Session>
{
    public void DestroySession()
    {
        Preferences.Remove("supabase_session");
    }

    public Session? LoadSession()
    {
        var sessionStr = Preferences.Get("supabase_session", null);
        return sessionStr != null ? JsonSerializer.Deserialize<Session>(sessionStr) : null;
    }

    public void SaveSession(Session session)
    {
        var sessionStr = JsonSerializer.Serialize(session);
        Preferences.Set("supabase_session", sessionStr);
    }
}