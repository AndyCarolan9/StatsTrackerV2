using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace StatsTrackerV2
{
    public partial class AppShell : Shell
    {
        private bool _canDataExport = false;
        public bool CanDataExport
        {
            get => _canDataExport;
            set
            {
                if(_canDataExport != value)
                {
                    _canDataExport = value;
                    OnPropertyChanged();
                }
            }
        }

        public AppShell()
        {
            InitializeComponent();
            var currentTheme = Application.Current!.RequestedTheme;
            ThemeSegmentedControl.SelectedIndex = currentTheme == AppTheme.Light ? 0 : 1;
            ExportButton.Pressed += ExportButton_Pressed;

            Application.Current.PageAppearing += OnPageChanged;
        }

        private void OnPageChanged(object? sender, Page e)
        {
            IStatsPage? currentPage = Shell.Current.CurrentPage as IStatsPage;
            CanDataExport = currentPage != null;
        }

        private void ExportButton_Pressed(object? sender, EventArgs e)
        {
            IStatsPage? statsPage = Shell.Current?.CurrentPage as IStatsPage;
            if (statsPage == null)
            {
                return;
            }

            statsPage.ExportPageData();
        }

        public static async Task DisplayMessage(string message)
        {
            if (OperatingSystem.IsWindows())
            {
                await DisplaySnackbarAsync(message);
            }
            else
            {
                await DisplayToastAsync(message);
            }
        }

        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#FF3300"),
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(0),
                Font = Font.SystemFontOfSize(18),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }

        public static async Task DisplayToastAsync(string message)
        {
            // Toast is currently not working in MCT on Windows
            if (OperatingSystem.IsWindows())
                return;

            var toast = Toast.Make(message, textSize: 18);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await toast.Show(cts.Token);
        }

        private void SfSegmentedControl_SelectionChanged(object? sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);
            pageTitle.Text = Current.CurrentPage.Title;
        }
    }
}
