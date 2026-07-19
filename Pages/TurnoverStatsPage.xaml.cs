#if ANDROID
using Android.Content;
using Android.Provider;
#endif

namespace StatsTrackerV2.Pages;

public partial class TurnoverStatsPage : ContentPage, IStatsPage
{
	public TurnoverStatsPage(TurnoverStatsPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
		model.TurnoverEventsUpdated += OnDrawableUpdated;
	}

	private void OnDrawableUpdated(object? sender, EventArgs e)
	{
		pitchDisplay.InvalidateDrawing();
	}

    public async void ExportPageData()
    {
		TurnoverStatsPageModel? model = BindingContext as TurnoverStatsPageModel;
		if(model == null)
		{
			await AppShell.DisplayMessage("Failed to export Turnover Data.");
			return;
		}

		string selectedTeam = model.SelectedTeam.Replace(" ", "_");
		string opponent = model.Teams.First(x => !x.Equals(model.SelectedTeam)).Replace(" ", "_");
		string chartFileName = selectedTeam + "_Turnover_Chart_V_" + opponent;
		var image = await turnoverChart.CaptureAsync();
		if (image != null)
		{
			ExportHelper.ExportImage(chartFileName, image);
		}

		string scoredDGFileName = selectedTeam + "_Scored_From_Turnovers_V_" + opponent;
		scoredGrid.ExportControl(scoredDGFileName);

		string concededDGFileName = selectedTeam + "_Conceded_From_Turnovers_V_" + opponent;
		concededGrid.ExportControl(concededDGFileName);

		string pitchDisplayFileName = selectedTeam + "_Turnover_Pitch_Display_V_" + opponent;
		pitchDisplay.ExportControl(pitchDisplayFileName);

        await AppShell.DisplayMessage("Turnover Data Exported");
    }	
}