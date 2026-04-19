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

	public static readonly BindableProperty LocationPointProperty =
		BindableProperty.Create(
			nameof(LocationPoint),
			typeof(PointF?),
			typeof(PitchDisplay),
			null,
			BindingMode.TwoWay);

	public PointF? LocationPoint
	{
		get => (PointF?)GetValue(LocationPointProperty);
		set => SetValue(LocationPointProperty, value);
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

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		if(LocationPoint == null)
		{
			return;
		}

		BoxView? boxView = sender as BoxView;
		if (boxView == null)
		{
			return;
		}

		Point? point = e.GetPosition(boxView);
		if (point == null)
		{
			return;
		}

		float x = (float)point.Value.X / (float)boxView.Width;
		float y = (float)point.Value.Y / (float)boxView.Height;

		LocationPoint = new PointF(x, y);
		drawingView.Invalidate();
    }
}