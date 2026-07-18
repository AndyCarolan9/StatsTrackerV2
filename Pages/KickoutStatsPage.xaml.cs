namespace StatsTrackerV2.Pages;

public partial class KickoutStatsPage : ContentPage, IStatsPage
{
	public KickoutStatsPage(KickoutStatsPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
		model.KickoutEventsUpdated += OnDrawableUpdated;
    }

    private void OnDrawableUpdated(object? sender, EventArgs e)
	{
		pitchDisplay.InvalidateDrawing();
	}

    public async void ExportPageData()
    {
        KickoutStatsPageModel? model = BindingContext as KickoutStatsPageModel;
		if (model == null)
		{
            await AppShell.DisplayMessage("Failed to export Kickout Data.");
            return;
        }

        string selectedTeam = model.SelectedTeam.Replace(" ", "_");
        string opponent = model.Teams.First(x => !x.Equals(model.SelectedTeam)).Replace(" ", "_");

        string pitchFileName = selectedTeam + "_Kickout_Pitch_Display_V_" + opponent;
        pitchDisplay.ExportControl(pitchFileName);

        string chartfileName = selectedTeam + "_Kickout_Chart_V_" + opponent;
        KickoutChart.SaveAsImage(chartfileName);

        string dataGridFileName = selectedTeam + "_Scored_&_Conceded_From_Kickouts_V_" + opponent;
        kickoutDataGrid.ExportControl(dataGridFileName);

        string distanceTableFileName = selectedTeam + "_Kickout_Distance_Table_V_" + opponent;
        kickoutDistanceTable.ExportControl(distanceTableFileName);

        await AppShell.DisplayMessage("Kickout Data Exported");
    }
}