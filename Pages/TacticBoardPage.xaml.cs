namespace StatsTrackerV2.Pages;

public partial class TacticBoardPage : ContentPage
{
	public TacticBoardPage(TacticBoardPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}