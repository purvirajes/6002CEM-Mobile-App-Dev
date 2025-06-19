# 6002CEM_PurviTulcidas


# Expenses App - Motivation & Purpose

The **Expenses App** was created to provide a simple, lightweight solution for tracking personal spending while helping users stay within a defined monthly budget. With many finance apps feeling bloated or too complex for everyday use, this app focuses on **clarity, ease of use, and essential budgeting features**. Whether you're just trying to be more mindful of your purchases or actively managing financial goals, the Expenses App keeps everything organized and accessible no spreadsheets required.

---

## Features & Progress - 6 pages in total

| Feature                             | Status  |
|-------------------------------------|---------|
| User login with Supabase as the DB  | ✅      |
| User registration with Supabase      | ✅      |
| View list of user expenses          | ✅      |
| Add a new expense                   | ✅      |
| Edit existing expenses              | ✅      |
| Delete an expense                   | ✅      |
| Set a monthly budget limit          | ✅      |
| Back button on all pages            | ✅      |
| Redirect to `MainPage` from login/register back buttons | ✅ |
| Splash screen customised (no .NET)  | ✅      |
| Display app title as "Expenses"     | ✅      |
| Clean responsive UI                 | ✅      |

---

Installation
Clone the repository
Open the solution in Visual Studio
Configure your Supabase credentials in MauiProgram.cs
Build and run the application
Configuration
Update the MauiProgram.cs file with your Supabase project URL and API key:

var supabaseUrl = "https://filler.supabase.co";
var supabaseKey = "filler";

Absolutely! Here's the **full, properly formatted Markdown** section for your `README.md` covering **Installation** and **Configuration**:

---

## Installation

To get the app up and running on your machine:

1. **Clone the repository**

2. **Open the solution in Visual Studio**

   Make sure you have the **.NET MAUI workload installed**.

3. Nuget Packages

4. **Configure your Supabase credentials** in `MauiProgram.cs`

5. **Build and run the application**

   Select your target platform (Android, Windows, or iOS) and start debugging.

---

## Configuration

You need to connect the app to your Supabase project. In `MauiProgram.cs`, update the following lines with your actual Supabase project URL and API key:

```csharp
var supabaseUrl = "https://filler.supabase.co";
var supabaseKey = "filler";
```

These credentials are used to initialize the Supabase client for authentication and database operations.


**Links: https://youtu.be/8kXfLJoz2Pw** 
