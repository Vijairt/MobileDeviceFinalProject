# 🏋️ FitTrack Pro

> A cross-platform fitness tracking application built with **.NET MAUI** for Android, iOS, macOS, and Windows — all from a single codebase.

FitTrack Pro helps busy professionals and students maintain their fitness regime by providing a single, streamlined platform to log meals, record workouts, and monitor vitamin/supplement intake — entirely offline, with no subscriptions required.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Screenshots / App Flow](#screenshots--app-flow)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Architecture](#architecture)
- [Database Schema](#database-schema)
- [Nutritionix API Integration](#nutritionix-api-integration)
- [Business Logic](#business-logic)
- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Configuration](#configuration)
- [Building & Running](#building--running)
- [Dependencies](#dependencies)
- [Project Timeline](#project-timeline)
- [Target Audience](#target-audience)
- [Contributing](#contributing)

---

## Overview

FitTrack Pro solves the problem of disjointed fitness apps by combining meal tracking, workout logging, and vitamin monitoring into one cohesive experience. All data is stored locally in a **SQLite database**, meaning the app works fully **offline**. When online, it optionally connects to the **Nutritionix API** to automatically retrieve accurate nutrition facts for food items.

---

## Features

### 🍽️ Meal & Calorie Logging
- Search for any food item (e.g., *"banana"*, *"chicken sandwich"*) using the **Nutritionix API**
- Automatically retrieves: calories, protein, carbohydrates, fat, and serving size
- Confirm and save the entry to the local database
- **Offline fallback**: manually enter calorie and macro values without internet access
- **Photo logging**: optionally capture a photo of your meal using the device camera
- Daily totals display: total calories consumed + full macro breakdown (protein, carbs, fat)
- Visual indicator showing calories remaining or over-budget vs. your daily goal (default: 2,000 kcal)

### 💪 Workout Tracking
- Log any exercise: Running, Walking, Cycling, Swimming, Lifting, HIIT, Yoga, or Other
- Record: exercise type, duration (minutes), distance (miles), sets, reps, and free-text notes
- Weekly summary: total sessions, total minutes, total miles
- **Progression tips**: automatically suggests a weekly mileage increase of ~10% based on last week's history (capped at 20%)
- Delete individual workout entries

### 💊 Vitamin & Supplement Tracking
- Add vitamins or supplements with: name, dosage, and health benefits
- Mark each vitamin as taken daily with a single tap (toggle check-in)
- **Streak tracking**: automatically calculates and displays your current consecutive-day streak per vitamin
- Motivational streak messages (e.g., *"🔥 6-day streak — keep it up!"*)
- Delete vitamins you no longer take

### 📊 Dashboard
- At-a-glance summary of today's nutrition (calories + macros)
- Calorie status vs. daily goal (*"Over goal by 100 calories"* / *"300 calories remaining"*)
- This week's workout count and total minutes
- Top vitamin streaks displayed in one place

---

## Screenshots / App Flow

```
┌─────────────────────────────────────────────────────┐
│                   Bottom Tab Bar                     │
│  [Dashboard]  [Meals]  [Workouts]  [Vitamins]        │
└─────────────────────────────────────────────────────┘

Dashboard Tab                    Meals Tab
┌─────────────────────┐          ┌─────────────────────┐
│ Monday, Jun 9       │          │ Meal Log             │
│                     │          │ ┌─ Today's Totals ─┐ │
│ 🍽️ Today's Nutrition │          │ │ 1800 kcal        │ │
│ ┌───────────────┐   │          │ │ P:90g C:200g F:50g│ │
│ │  1,800 kcal   │   │          │ └──────────────────┘ │
│ │  200 remaining│   │          │                     │
│ │ P:90 C:200 F:50│  │          │ • Chicken Breast    │
│ └───────────────┘   │          │   300 kcal          │
│                     │          │ • Banana            │
│ 💪 This Week        │          │   105 kcal          │
│ ┌───────────────┐   │          │                  [+]│
│ │ 3 sessions    │   │          └─────────────────────┘
│ │ 145 minutes   │   │
│ └───────────────┘   │          Add Meal Page
│                     │          ┌─────────────────────┐
│ 💊 Vitamin Streaks  │          │ Search Food         │
│ Vit D: 6-day 🔥    │          │ [chicken breast 200g]│
│                     │          │ [Search]            │
└─────────────────────┘          │                     │
                                 │ Results:            │
Workouts Tab                     │ ✓ Chicken Breast    │
┌─────────────────────┐          │   330 kcal          │
│ Workouts            │          │   P:62g C:0g F:7g   │
│ ┌── Weekly ───────┐ │          │            [Select] │
│ │ 3  145  4.5      │ │          │                     │
│ │ sess min miles   │ │          │ [Enter Manually]    │
│ └─────────────────┘ │          └─────────────────────┘
│ 💡 Try 13 miles     │
│   this week 💪      │          Vitamins Tab
│                     │          ┌─────────────────────┐
│ • Running 45 min    │          │ Vitamins            │
│   3.2 miles         │          │                     │
│ • Lifting 60 min    │          │ Vitamin D           │
│   3 sets × 10 reps  │          │ 1000 IU             │
│                  [+]│          │ Bone & immune health│
└─────────────────────┘          │ 🔥 6-day streak     │
                                 │ [✅ Taken]  [🗑️]    │
                                 │                  [+]│
                                 └─────────────────────┘
```

---

## Technology Stack

| Component | Technology |
|---|---|
| **Core Framework** | .NET MAUI (.NET 9) |
| **Architecture Pattern** | MVVM |
| **MVVM Toolkit** | CommunityToolkit.Mvvm 8.3.2 |
| **UI Extras** | CommunityToolkit.Maui 11.1.1 |
| **Charts / UI Components** | Syncfusion.Maui.Toolkit 1.0.6 |
| **Local Database** | SQLite via Microsoft.Data.Sqlite.Core 8.0.8 |
| **SQLite Native Bindings** | SQLitePCLRaw.bundle_green 2.1.10 |
| **Nutrition Data** | Nutritionix API (free tier) |
| **Photo Capture** | MAUI Essentials `MediaPicker` |
| **IDE** | Visual Studio 2026 |
| **Version Control** | Git / GitHub |
| **Target Platforms** | Android, iOS, macOS Catalyst, Windows |

---

## Project Structure

```
MobileDeviceFinalProject/
│
├── Models/                         # Data model classes
│   ├── MealEntry.cs                # A single logged meal with nutrition data
│   ├── WorkoutEntry.cs             # A single logged workout session
│   ├── VitaminEntry.cs             # A vitamin/supplement definition + VitaminLog
│   └── NutritionResult.cs          # Nutritionix API response DTO
│
├── Data/                           # SQLite repository layer
│   ├── Constants.cs                # Database file path helper
│   ├── MealRepository.cs           # CRUD for MealEntry table
│   ├── WorkoutRepository.cs        # CRUD for WorkoutEntry table
│   └── VitaminRepository.cs        # CRUD for VitaminEntry + VitaminLog + streak logic
│
├── Services/
│   ├── NutritionixService.cs       # Nutritionix API calls (offline-safe)
│   ├── ModalErrorHandler.cs        # Displays error alerts via Shell
│   └── IErrorHandler.cs            # Error handler interface
│
├── PageModels/                     # ViewModels (MVVM)
│   ├── DashboardPageModel.cs       # Dashboard summary logic
│   ├── MealLogPageModel.cs         # Daily meal list + totals
│   ├── AddMealPageModel.cs         # API search + manual entry + photo
│   ├── WorkoutPageModel.cs         # Weekly workout list + progression tip
│   ├── AddWorkoutPageModel.cs      # Workout log form logic
│   ├── VitaminPageModel.cs         # Vitamin checklist + streak display
│   └── AddVitaminPageModel.cs      # Add vitamin form logic
│
├── Pages/                          # XAML UI pages
│   ├── DashboardPage.xaml          # Home summary screen
│   ├── MealLogPage.xaml            # Daily meal list view
│   ├── AddMealPage.xaml            # Food search + manual entry form
│   ├── WorkoutPage.xaml            # Weekly workout list
│   ├── AddWorkoutPage.xaml         # Log a workout form
│   ├── VitaminPage.xaml            # Vitamin checklist
│   └── AddVitaminPage.xaml         # Add vitamin form
│
├── Resources/
│   ├── Styles/
│   │   ├── Colors.xaml             # App color palette
│   │   ├── Styles.xaml             # Base MAUI control styles
│   │   └── AppStyles.xaml          # Custom card, icon, and layout styles
│   ├── Fonts/
│   │   └── FluentUI.cs             # Fluent UI icon font constants
│   └── AppIcon / Splash            # App icon and splash screen assets
│
├── Utilities/
│   └── TaskUtilities.cs            # FireAndForgetSafeAsync extension
│
├── Platforms/                      # Platform-specific bootstrapping
│   └── Android / iOS / Windows ...
│
├── AppShell.xaml                   # Tab bar shell navigation (4 tabs)
├── AppShell.xaml.cs
├── MauiProgram.cs                  # DI container + app configuration
├── GlobalUsings.cs                 # Project-wide global using directives
└── App.xaml / App.xaml.cs          # Application entry point
```

---

## Architecture

FitTrack Pro follows the **MVVM (Model-View-ViewModel)** pattern throughout:

```
┌──────────┐     Data Binding      ┌────────────────┐
│  View    │ ◄────────────────────► │  ViewModel     │
│ (XAML)   │    Commands/Events    │ (PageModel)    │
└──────────┘                       └────────────────┘
                                          │
                                          │ calls
                                          ▼
                                   ┌────────────────┐
                                   │  Repository    │
                                   │  (Data Layer)  │
                                   └────────────────┘
                                          │
                                          │ SQL queries
                                          ▼
                                   ┌────────────────┐
                                   │  SQLite DB     │
                                   │ (Local File)   │
                                   └────────────────┘

External:
PageModel ──HTTP──► NutritionixService ──► Nutritionix REST API
```

- **Views** are XAML pages with compiled bindings (`x:DataType`) for performance
- **ViewModels** use `[ObservableProperty]` and `[RelayCommand]` from CommunityToolkit.Mvvm
- **Repositories** handle all raw SQL using `Microsoft.Data.Sqlite` with async patterns
- **Dependency Injection** is configured in `MauiProgram.cs` using the built-in MAUI DI container

---

## Database Schema

All tables are created automatically on first launch inside the device's private app data directory.

### `MealEntry`
| Column | Type | Description |
|---|---|---|
| `Id` | INTEGER PK | Auto-increment primary key |
| `FoodName` | TEXT | Name of the food item |
| `Calories` | REAL | Kilocalories per serving |
| `ProteinG` | REAL | Protein in grams |
| `CarbsG` | REAL | Carbohydrates in grams |
| `FatG` | REAL | Fat in grams |
| `ServingQty` | REAL | Serving quantity number |
| `ServingUnit` | TEXT | Serving unit (e.g., "cup", "oz") |
| `LoggedAt` | TEXT | ISO-8601 timestamp |
| `PhotoPath` | TEXT | Optional local path to meal photo |

### `WorkoutEntry`
| Column | Type | Description |
|---|---|---|
| `Id` | INTEGER PK | Auto-increment primary key |
| `ExerciseType` | TEXT | Running, Lifting, HIIT, etc. |
| `DurationMinutes` | INTEGER | Total session duration |
| `DistanceMiles` | REAL | Distance (nullable, for cardio) |
| `Sets` | INTEGER | Number of sets (nullable, for lifting) |
| `Reps` | INTEGER | Reps per set (nullable) |
| `Notes` | TEXT | Optional free-text notes |
| `LoggedAt` | TEXT | ISO-8601 timestamp |

### `VitaminEntry`
| Column | Type | Description |
|---|---|---|
| `Id` | INTEGER PK | Auto-increment primary key |
| `Name` | TEXT | Vitamin/supplement name |
| `Dosage` | TEXT | Dosage label (e.g., "1000 IU") |
| `Benefits` | TEXT | Health benefit description |
| `IsDaily` | INTEGER | 1 = daily, 0 = as needed |

### `VitaminLog`
| Column | Type | Description |
|---|---|---|
| `Id` | INTEGER PK | Auto-increment primary key |
| `VitaminEntryId` | INTEGER | Foreign key → VitaminEntry |
| `LoggedDate` | TEXT | ISO-8601 date the vitamin was taken |

---

## Nutritionix API Integration

FitTrack Pro uses the [Nutritionix Natural Language API](https://developer.nutritionix.com/) to look up nutrition data from plain-text food queries.

### Endpoint
```
POST https://trackapi.nutritionix.com/v2/natural/nutrients
```

### Request Headers
```
x-app-id:         YOUR_APP_ID
x-app-key:        YOUR_APP_KEY
x-remote-user-id: 0
Content-Type:     application/json
```

### Request Body
```json
{ "query": "chicken breast 200g" }
```

### Response Data Used
```json
{
  "foods": [
    {
      "food_name":               "chicken breast",
      "nf_calories":             330,
      "nf_protein":              62,
      "nf_total_carbohydrate":   0,
      "nf_total_fat":            7,
      "serving_qty":             200,
      "serving_unit":            "g"
    }
  ]
}
```

### Offline Behavior
If the device is offline or the API call fails, the app gracefully shows the **manual entry form** automatically, allowing users to type calorie and macro values themselves.

---

## Business Logic

### 1. Calorie Budget
```
User searches "chicken breast 200g"
  → API returns: 330 kcal, 62g protein, 0g carbs, 7g fat
  → User confirms → saved to DB

Dashboard query:
  SELECT SUM(Calories) FROM MealEntry WHERE date(LoggedAt) = date('now')
  → Today total: 1,800 kcal

Daily goal: 2,000 kcal
  → Status: "200 calories remaining"

Macro display:
  Protein: 90g  |  Carbs: 200g  |  Fat: 50g
```

### 2. Workout Progression
```
Last week's running miles:
  SELECT SUM(DistanceMiles) FROM WorkoutEntry
  WHERE date(LoggedAt) BETWEEN date('last Sunday - 7') AND date('last Saturday')
  → 12.0 miles

Progression calculation:
  Goal = lastWeekMiles × 1.10 (capped at ×1.20)
  Goal = 12.0 × 1.10 = 13.2 → rounded to 13.2 miles

Displayed tip:
  "Great job! Try 13.2 miles this week (last week: 12.0 mi) 💪"
```

### 3. Vitamin Streak
```
SELECT date(LoggedDate) FROM VitaminLog
WHERE VitaminEntryId = @id
ORDER BY LoggedDate DESC

Algorithm:
  Start from today (or yesterday if today not yet logged)
  Walk backwards day-by-day counting consecutive entries

Example result: streak = 6
Display: "🔥 6-day streak — keep it up!"
```

---

## Getting Started

### Prerequisites

| Requirement | Minimum Version |
|---|---|
| Visual Studio | 2022 / 2026 with .NET MAUI workload |
| .NET SDK | 9.0 |
| Android SDK | API 21+ (Android 5.0 Lollipop) |
| iOS SDK | iOS 15.0+ (requires macOS + Xcode) |
| Windows | Windows 10 build 17763+ |

### Prerequisites Installation

1. Install **Visual Studio 2022 or 2026** with the `.NET Multi-platform App UI development` workload checked.
2. Verify MAUI installation:
   ```powershell
   dotnet workload install maui
   ```
3. Confirm .NET 9 SDK is installed:
   ```powershell
   dotnet --list-sdks
   ```

---

## Configuration

### Nutritionix API Keys

Register for a **free** API account at [https://developer.nutritionix.com/](https://developer.nutritionix.com/).

Once you have your credentials, open:
```
MobileDeviceFinalProject/Services/NutritionixService.cs
```

Replace the placeholder constants:
```csharp
// Line 14-15 in NutritionixService.cs
private const string AppId  = "YOUR_APP_ID";   // ← replace this
private const string AppKey = "YOUR_APP_KEY";  // ← replace this
```

> **Note:** The app works fully without API keys — users can always enter nutrition data manually using the offline form.

---

## Building & Running

### Clone the Repository
```powershell
git clone https://github.com/Vijairt/MobileDeviceFinalProject.git
cd MobileDeviceFinalProject
```

### Restore NuGet Packages
```powershell
dotnet restore
```

### Run on Android Emulator
```powershell
dotnet build -t:Run -f net9.0-android
```

### Run on Windows
```powershell
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

### Run on iOS Simulator (macOS only)
```powershell
dotnet build -t:Run -f net9.0-ios
```

### Run via Visual Studio
1. Open `MobileDeviceFinalProject.sln` in Visual Studio
2. Select your target device/emulator from the debug toolbar dropdown
3. Press **F5** or click **▶ Run**

---

## Dependencies

| Package | Version | Purpose |
|---|---|---|
| `Microsoft.Maui.Controls` | (MauiVersion) | Core MAUI framework |
| `CommunityToolkit.Mvvm` | 8.3.2 | MVVM source generators (`[ObservableProperty]`, `[RelayCommand]`) |
| `CommunityToolkit.Maui` | 11.1.1 | Converters, behaviors (`EventToCommandBehavior`, `IsNotNullConverter`) |
| `Syncfusion.Maui.Toolkit` | 1.0.6 | UI components (pull-to-refresh, segmented control) |
| `Microsoft.Data.Sqlite.Core` | 8.0.8 | SQLite database access |
| `SQLitePCLRaw.bundle_green` | 2.1.10 | Native SQLite bindings for all platforms |
| `Microsoft.Extensions.Logging.Debug` | 9.0.9 | Debug logging |

---

## Project Timeline

| Week(s) | Milestone |
|---|---|
| **1–2** | App screens planned, database schema finalized, Visual Studio project set up with .NET MAUI |
| **3–4** | Meal logging feature built, Nutritionix API integrated, photo logging implemented |
| **5–6** | Workout tracking and vitamin/supplement tracking built, all features connected to SQLite |
| **7** | Full feature testing, bug fixes, UI polish, data persistence verification |
| **8** | Final testing, presentation preparation, project submission |

---

## Target Audience

**Primary users:** Health-conscious adults aged **18–35** in urban environments (students and working professionals) who:
- Exercise 3–5 times per week
- Are too busy for complex fitness apps
- Want simple, fast logging with clear visual feedback
- Do not want to pay for gym memberships or app subscriptions

**User Benefits:**
- ✅ Single app for meals, workouts, and vitamins
- ✅ No account required — fully local and private
- ✅ Works completely offline
- ✅ Gradual goal progression to build healthy habits
- ✅ Streak system to reinforce daily routines

---

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "Add your feature"`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a Pull Request against `master`

---

## License

This project was developed as a final project for a Mobile Device Programming course. All rights reserved by the project authors.

---

*Built with ❤️ using .NET MAUI — one codebase, every platform.*
