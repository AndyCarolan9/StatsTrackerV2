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

	}
}