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
            Colors.Red,
            BindingMode.OneWay,
            propertyChanged: OnColorChanged);

    public Color MarkerColor
    {
        get => (Color)GetValue(MarkerColorProperty);
        set => SetValue(MarkerColorProperty, value);
    }

    public static readonly BindableProperty IsBallProperty =
        BindableProperty.Create(
            nameof(IsBall),
            typeof(bool),
            typeof(PlayerMarker),
            false);

    public bool IsBall
    {
        get => (bool)GetValue(IsBallProperty);
        set => SetValue(IsBallProperty, value);
    }

    public static readonly BindableProperty InputActiveProperty =
        BindableProperty.Create(
            nameof(InputActive),
            typeof(bool),
            typeof(PlayerMarker),
            false);

    public bool InputActive
    {
        get => (bool)GetValue(InputActiveProperty);
        set => SetValue(InputActiveProperty, value);
    }

    public Color LabelColor { get; set; } = Colors.White;

    public bool IsHomeMarker { get; set; } = true;

    private double _startTranslationX;
    private double _startTranslationY;

    public PlayerMarker()
	{
		InitializeComponent();
	}

    private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        PlayerMarker? playerMarker = bindable as PlayerMarker;
        if (playerMarker == null)
        {
            return;
        }

        Color detailColor = ColorsHelper.IsColorDark(playerMarker.MarkerColor) ? Colors.White : Colors.Black;
        playerMarker.Label.TextColor = detailColor;
        playerMarker.BorderStroke.Stroke = detailColor;
    }

    private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (!InputActive)
            return;

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