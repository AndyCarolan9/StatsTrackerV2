namespace StatsTrackerV2.Pages;

public partial class KickoutStatsPage : ContentPage
{
	public KickoutStatsPage(KickoutStatsPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}