using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StatsTrackerV2.Models;
using Syncfusion.Maui.Toolkit.Hosting;

namespace StatsTrackerV2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit(options =>
                {
                    options.SetShouldEnableSnackbarOnWindows(true);
                })
                .ConfigureSyncfusionToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
#if WINDOWS
    				Microsoft.Maui.Controls.Handlers.Items.CollectionViewHandler.Mapper.AppendToMapping("KeyboardAccessibleCollectionView", (handler, view) =>
    				{
    					handler.PlatformView.SingleSelectionFollowsFocus = false;
    				});
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

            builder.Services.AddSingleton<Match>();
            builder.Services.AddSingleton<MatchPageModel>();
            builder.Services.AddSingleton<KickoutStatsPageModel>();
            builder.Services.AddSingleton<TurnoverStatsPageModel>();
            builder.Services.AddSingleton<ShotStatsPageModel>();

            builder.Services.AddTransientWithShellRoute<OpenMatchPage, OpenMatchPageModel>("openMatch");
            builder.Services.AddTransientWithShellRoute<CreateMatchPage, CreateMatchPageModel>("createMatch");
            builder.Services.AddTransientWithShellRoute<TeamListPage, TeamListPageModel>("teamList");
            builder.Services.AddTransientWithShellRoute<CreateMatchEventPage, CreateMatchEventPageModel>("createMatchEvent");
            builder.Services.AddTransientWithShellRoute<SubstitutionPage, SubstitutionPageModel>("substitution");

            return builder.Build();
        }
    }
}
