namespace StatsTrackerV2.Pages;

public partial class OpenMatchPage : ContentPage
{
	public OpenMatchPage(OpenMatchPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}