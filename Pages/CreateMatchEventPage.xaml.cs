using StatsTrackerV2.Models;

namespace StatsTrackerV2.Pages;

public partial class CreateMatchEventPage : ContentPage
{
	public CreateMatchEventPage(CreateMatchEventPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}