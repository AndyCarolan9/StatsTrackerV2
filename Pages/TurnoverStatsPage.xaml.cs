namespace StatsTrackerV2.Pages;

public partial class TurnoverStatsPage : ContentPage
{
	public TurnoverStatsPage(TurnoverStatsPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
		model.TurnoverEventsUpdated += OnDrawableUpdated;
	}

	private void OnDrawableUpdated(object? sender, EventArgs e)
	{
		pitchDisplay.InvalidateDrawing();
	}
}