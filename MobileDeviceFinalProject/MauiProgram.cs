using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using MobileDeviceFinalProject.Data;

namespace MobileDeviceFinalProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
#if IOS || MACCATALYST
                    handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            // Repositories
            builder.Services.AddSingleton<MealRepository>();
            builder.Services.AddSingleton<WorkoutRepository>();
            builder.Services.AddSingleton<VitaminRepository>();

            // Services
            builder.Services.AddSingleton<ModalErrorHandler>();
            builder.Services.AddSingleton<NutritionixService>();

            // Tab page models (Singleton — one instance per tab)
            builder.Services.AddSingleton<DashboardPageModel>();
            builder.Services.AddSingleton<MealLogPageModel>();
            builder.Services.AddSingleton<WorkoutPageModel>();
            builder.Services.AddSingleton<VitaminPageModel>();

            // Tab pages
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddSingleton<MealLogPage>();
            builder.Services.AddSingleton<WorkoutPage>();
            builder.Services.AddSingleton<VitaminPage>();

            // Modal / pushed pages (Transient — fresh instance each time)
            builder.Services.AddTransientWithShellRoute<AddMealPage, AddMealPageModel>("addmeal");
            builder.Services.AddTransientWithShellRoute<AddWorkoutPage, AddWorkoutPageModel>("addworkout");
            builder.Services.AddTransientWithShellRoute<AddVitaminPage, AddVitaminPageModel>("addvitamin");

            return builder.Build();
        }
    }
}
