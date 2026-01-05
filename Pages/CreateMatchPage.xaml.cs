namespace StatsTrackerV2.Pages;

public partial class CreateMatchPage : ContentPage
{
	public CreateMatchPage(CreateMatchPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}