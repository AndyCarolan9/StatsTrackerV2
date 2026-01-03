namespace StatsTrackerV2.Pages;

public partial class MatchPage : ContentPage
{
	public MatchPage(MatchPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}