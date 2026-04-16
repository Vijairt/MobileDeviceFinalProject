using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace MobileDeviceFinalProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#512BD4"),
                TextColor = Colors.White,
                CornerRadius = new CornerRadius(8),
                Font = Font.SystemFontOfSize(16)
            };

            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);
            await snackbar.Show(cancellationTokenSource.Token);
        }

        public static async Task DisplayToastAsync(string message)
        {
            if (OperatingSystem.IsWindows())
                return;

            var toast = Toast.Make(message, textSize: 16);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
            await toast.Show(cts.Token);
        }
    }
}
