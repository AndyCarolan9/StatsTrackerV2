namespace StatsTrackerV2.Pages;

public partial class SubstitutionPage : ContentPage
{
	public SubstitutionPage(SubstitutionPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}