namespace StatsTrackerV2.Pages;

public partial class ShotStatsPage : ContentPage
{
	public ShotStatsPage(ShotStatsPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
		model.ShotEventsUpdated += OnDrawableUpdated;
	}

	private void OnDrawableUpdated(object? sender, EventArgs e)
	{
		pitchDisplay.InvalidateDrawing();
	}

    private void NumericalAxis_LabelCreated(object sender, Syncfusion.Maui.Toolkit.Charts.ChartAxisLabelEventArgs e)
    {
        if (e.Position is double seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);

			int minutes = (ts.Hours * 60) + ts.Minutes;

            e.Label = $"{minutes:D2}:{ts.Seconds:D2}";
        }
    }
}