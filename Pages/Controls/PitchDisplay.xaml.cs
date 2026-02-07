using CommunityToolkit.Maui.Views;

namespace StatsTrackerV2.Pages.Controls;

public partial class PitchDisplay : ContentView
{
	public static readonly BindableProperty StatisticDrawerProperty =
		BindableProperty.Create(
			nameof(StatisticDrawer),
			typeof(IDrawable),
			typeof(PitchDisplay),
			null);

	public IDrawable StatisticDrawer
	{
		get => (IDrawable)GetValue(StatisticDrawerProperty);
		set => SetValue(StatisticDrawerProperty, value);
	}

	public static readonly BindableProperty HomeTeamTextProperty =
		BindableProperty.Create(
			nameof(HomeTeamText),
			typeof(string),
			typeof(PitchDisplay));

	public string HomeTeamText
	{
		get => (string)GetValue(HomeTeamTextProperty);
		set => SetValue (HomeTeamTextProperty, value);
	}

    public static readonly BindableProperty AwayTeamTextProperty =
        BindableProperty.Create(
            nameof(AwayTeamText),
            typeof(string),
            typeof(PitchDisplay));

    public string AwayTeamText
    {
        get => (string)GetValue(AwayTeamTextProperty);
        set => SetValue(AwayTeamTextProperty, value);
    }

    public PitchDisplay()
	{
		InitializeComponent();

		drawingView.BindingContext = this;
		drawingView.SetBinding(GraphicsView.DrawableProperty, new Binding(nameof(StatisticDrawer), source: this));

		homeTeamLabel.BindingContext = this;
		homeTeamLabel.SetBinding(Label.TextProperty, new Binding(nameof(HomeTeamText), source: this));

        awayTeamLabel.BindingContext = this;
        awayTeamLabel.SetBinding(Label.TextProperty, new Binding(nameof(AwayTeamText), source: this));

        MainImage.SizeChanged += (s, e) =>
        {
            drawingView.WidthRequest = MainImage.Width;
            drawingView.HeightRequest = MainImage.Height;
        };
    }

	public void InvalidateDrawing()
	{
		drawingView.Invalidate(); 
	}
}