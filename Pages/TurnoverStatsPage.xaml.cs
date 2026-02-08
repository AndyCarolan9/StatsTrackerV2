namespace StatsTrackerV2.Pages;

public partial class TurnoverStatsPage : ContentPage
{
	public TurnoverStatsPage(TurnoverStatsPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}