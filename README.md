# 🏋️ FitTrack Pro

A cross-platform fitness tracking app built with **.NET MAUI (.NET 9)** for Android, iOS, macOS, and Windows.

Track your meals, workouts, and vitamins all in one place — fully offline, no subscriptions needed.

---

## Features

| Tab | What it does |
|---|---|
| 📊 **Dashboard** | Daily calorie total vs. goal, weekly workout summary, vitamin streaks |
| 🍽️ **Meals** | Log food via Nutritionix API search or manual entry; optional meal photo |
| 💪 **Workouts** | Log sessions (type, duration, distance, sets/reps); weekly summary + auto progression tip |
| 💊 **Vitamins** | Add vitamins/supplements; daily check-in with streak counter |

---

## Tech Stack

- **Framework:** .NET MAUI (.NET 9)
- **Pattern:** MVVM with CommunityToolkit.Mvvm
- **Database:** SQLite (Microsoft.Data.Sqlite) — all data stored locally on device
- **Nutrition API:** Nutritionix (free tier) — falls back to manual entry when offline
- **UI:** XAML with Syncfusion.Maui.Toolkit & CommunityToolkit.Maui

---

## Getting Started

### Prerequisites
- Visual Studio 2022/2026 with the **.NET MAUI workload** installed
- .NET 9 SDK

### Clone & Run
```powershell
git clone https://github.com/Vijairt/MobileDeviceFinalProject.git
cd MobileDeviceFinalProject
dotnet restore
```
Open `MobileDeviceFinalProject.sln` in Visual Studio, select a target device, and press **F5**.

---

## Nutritionix API Setup

Register for a free key at [developer.nutritionix.com](https://developer.nutritionix.com/), then open `Services/NutritionixService.cs` and replace:

```csharp
private const string AppId  = "YOUR_APP_ID";
private const string AppKey = "YOUR_APP_KEY";
```

> The app works fully offline without API keys — users can enter nutrition data manually.

---

## Project Structure

```
Models/          → MealEntry, WorkoutEntry, VitaminEntry, NutritionResult
Data/            → MealRepository, WorkoutRepository, VitaminRepository (SQLite)
Services/        → NutritionixService, ModalErrorHandler
PageModels/      → ViewModels for each screen (MVVM)
Pages/           → XAML pages: Dashboard, MealLog, AddMeal, Workout, AddWorkout, Vitamin, AddVitamin
```

---

## License

Final project for Mobile Device Programming. All rights reserved by the project authors.
