namespace StatsTrackerV2.Pages;

public partial class ShotStatsPage : ContentPage, IStatsPage
{
	public ShotStatsPage(ShotStatsPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
		model.ShotEventsUpdated += OnDrawableUpdated;
	}

	private void OnDrawableUpdated(object? sender, EventArgs e)
	{
		pitchDisplay.InvalidateDrawing();
	}

    private void NumericalAxis_LabelCreated(object sender, Syncfusion.Maui.Toolkit.Charts.ChartAxisLabelEventArgs e)
    {
        if (e.Position is double seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);

			int minutes = (ts.Hours * 60) + ts.Minutes;

            e.Label = $"{minutes:D2}:{ts.Seconds:D2}";
        }
    }

    public async void ExportPageData()
    {
        ShotStatsPageModel? model = BindingContext as ShotStatsPageModel;
        if (model == null)
        {
            await AppShell.DisplayMessage("Failed to export Scoring Data.");
            return;
        }

        string selectedTeam = model.SelectedTeam.Replace(" ", "_");
        string opponent = model.Teams.First(x => !x.Equals(model.SelectedTeam)).Replace(" ", "_");
        string chartfileName = selectedTeam + "_Scoring_Chart_V_" + opponent;
        shotChart.SaveAsImage(chartfileName);

        string eventScoresFileName = selectedTeam + "_Scored_From_Events_V_" + opponent;
        eventScoresChart.ExportControl(eventScoresFileName);

        string lineChartFileName = selectedTeam + "_Score_Progress_Chart_V_" + opponent;
        scoreTimeline.SaveAsImage(lineChartFileName);

        string scorersListFileName = selectedTeam + "_Scorers_List_V_" + opponent;
        scorersList.ExportControl(scorersListFileName);

        string pitchFileName = selectedTeam + "_Shooting_Pitch_Display_V_" + opponent;
        pitchDisplay.ExportControl(pitchFileName);

        await AppShell.DisplayMessage("Shooting Data Exported");
    }
}