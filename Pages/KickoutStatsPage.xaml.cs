namespace StatsTrackerV2.Pages;

public partial class KickoutStatsPage : ContentPage
{
	public KickoutStatsPage(KickoutStatsPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
		model.KickoutEventsUpdated += OnDrawableUpdated;
	}

	private void OnDrawableUpdated(object? sender, EventArgs e)
	{
		DrawingView.Invalidate();
	}
}