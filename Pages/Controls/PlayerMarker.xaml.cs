namespace StatsTrackerV2.Pages.Controls;

public partial class PlayerMarker : ContentView
{
    public static readonly BindableProperty PlayerNumberProperty =
        BindableProperty.Create(
            nameof(PlayerNumber),
            typeof(string),
            typeof(PlayerMarker),
            "1");

    public string PlayerNumber
    {
        get => (string)GetValue(PlayerNumberProperty);
        set => SetValue(PlayerNumberProperty, value);
    }

    public static readonly BindableProperty MarkerColorProperty =
        BindableProperty.Create(
            nameof(MarkerColor),
            typeof(Color),
            typeof(PlayerMarker),
            Colors.Red);

    public Color MarkerColor
    {
        get => (Color)GetValue(MarkerColorProperty);
        set => SetValue(MarkerColorProperty, value);
    }

    private double _startTranslationX;
    private double _startTranslationY;

    public PlayerMarker()
	{
		InitializeComponent();
	}

    private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _startTranslationX = TranslationX;
                _startTranslationY = TranslationY;
                break;

            case GestureStatus.Running:
                TranslationX = _startTranslationX + e.TotalX;
                TranslationY = _startTranslationY + e.TotalY;
                break;

            case GestureStatus.Completed:
                // Add event call here for when player locations will be stored in float values
                break;
        }
    }
}