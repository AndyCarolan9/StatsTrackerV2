namespace StatsTrackerV2.Pages;

public partial class TeamListPage : ContentPage
{
	public TeamListPage(TeamListPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}