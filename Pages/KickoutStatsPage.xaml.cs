namespace StatsTrackerV2.Pages;

public partial class KickoutStatsPage : ContentPage
{
	public KickoutStatsPage(KickoutStatsPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
		model.KickoutEventsUpdated += OnDrawableUpdated;

        MainImage.SizeChanged += (s, e) =>
        {
            DrawingView.WidthRequest = MainImage.Width;
            DrawingView.HeightRequest = MainImage.Height;
        };
    }

	private void OnDrawableUpdated(object? sender, EventArgs e)
	{
		DrawingView.Invalidate();
	}
}