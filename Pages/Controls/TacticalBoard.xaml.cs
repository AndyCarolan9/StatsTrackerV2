using Microsoft.Maui.Layouts;
using StatsTrackerV2.Data.Constants;
using StatsTrackerV2.Data.DrawItems;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace StatsTrackerV2.Pages.Controls;

public partial class TacticalBoard : ContentView
{
	public static readonly BindableProperty TacticalPlayersProperty =
		BindableProperty.Create(
			nameof(TacticalPlayers),
			typeof(ObservableCollection<TacticalPlayerMarker>),
			typeof(TacticalBoard),
			default(ObservableCollection<TacticalPlayerMarker>),
			propertyChanged: OnItemsChanged);

	public ObservableCollection<TacticalPlayerMarker> TacticalPlayers
	{
		get => (ObservableCollection<TacticalPlayerMarker>)GetValue(TacticalPlayersProperty);
		set => SetValue(TacticalPlayersProperty, value);
	}

    public static readonly BindableProperty IsMoveActiveProperty =
        BindableProperty.Create(
            nameof(IsMoveActive),
            typeof(bool),
            typeof(TacticalBoard),
            false);

    public bool IsMoveActive
    {
        get => (bool)GetValue(IsMoveActiveProperty);
        set => SetValue(IsMoveActiveProperty, value);
    }

    public static readonly BindableProperty SelectedToolProperty =
        BindableProperty.Create(
            nameof(SelectedTool),
            typeof(string),
            typeof(TacticalBoard),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: OnSelectedToolChanged);

    public string SelectedTool
    {
        get => (string)GetValue(SelectedToolProperty);
        set => SetValue(SelectedToolProperty, value);
    }

    public static readonly BindableProperty SelectedDrawColorProperty =
        BindableProperty.Create(
            nameof(SelectedDrawColor),
            typeof(Color),
            typeof(TacticalBoard));

    public Color SelectedDrawColor
    {
        get => (Color)GetValue(SelectedDrawColorProperty);
        set => SetValue(SelectedDrawColorProperty, value);
    }

    public static readonly BindableProperty HomeColorProperty =
        BindableProperty.Create(
            nameof(HomeColor),
            typeof(Color),
            typeof(TacticalBoard),
            Colors.White,
            BindingMode.OneWay,
            propertyChanged: OnHomeColorChanged);

    public Color HomeColor
    {
        get => (Color)GetValue(HomeColorProperty);
        set => SetValue(HomeColorProperty, value);
    }

    public static readonly BindableProperty AwayColorProperty =
        BindableProperty.Create(
            nameof(AwayColor),
            typeof(Color),
            typeof(TacticalBoard),
            Colors.Black,
            BindingMode.OneWay,
            propertyChanged: OnAwayColorChanged);

    public Color AwayColor
    {
        get => (Color)GetValue(AwayColorProperty);
        set => SetValue(AwayColorProperty, value);
    }

    private readonly TacticalDrawable _drawable = new TacticalDrawable();

    private DrawItem? _drawItem = null;

	public TacticalBoard()
	{
		InitializeComponent();

        DrawingView.Drawable = _drawable;
    }

    private static void OnSelectedToolChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TacticalBoard? board = (TacticalBoard)bindable;
        if(board is null)
        {
            return;
        }

        board.IsMoveActive = board.SelectedTool == TacticBoardConstants.MoveToolName;
    }

    private static void OnHomeColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TacticalBoard? board = (TacticalBoard)bindable;
        if(board is null)
        {
            return;
        }

        Color? newColor = newValue as Color;
        if(newColor is null)
        {
            return;
        }

        IList<IView> playerMarkers = board.PlayerLayer.Children;

        foreach (IView playerMarker in playerMarkers)
        {
            PlayerMarker? marker = playerMarker as PlayerMarker;
            if(marker is null)
            {
                continue;
            }

            if(marker.IsHomeMarker)
            {
                marker.MarkerColor = newColor;
            }
        }
    }

    private static void OnAwayColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        TacticalBoard? board = (TacticalBoard)bindable;
        if (board is null)
        {
            return;
        }

        Color? newColor = newValue as Color;
        if (newColor is null)
        {
            return;
        }

        IList<IView> playerMarkers = board.PlayerLayer.Children;

        foreach (IView playerMarker in playerMarkers)
        {
            PlayerMarker? marker = playerMarker as PlayerMarker;
            if (marker is null)
            {
                continue;
            }

            if (!marker.IsHomeMarker)
            {
                marker.MarkerColor = newColor;
            }
        }
    }

    private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TacticalBoard)bindable;

        if (oldValue is ObservableCollection<TacticalPlayerMarker> oldCollection)
            oldCollection.CollectionChanged -= control.Items_CollectionChanged;

        if (newValue is ObservableCollection<TacticalPlayerMarker> newCollection)
            newCollection.CollectionChanged += control.Items_CollectionChanged;
    }

    private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action != NotifyCollectionChangedAction.Move)
        {
            UpdatePlayerMarkers();
        }
    }

    private void UpdatePlayerMarkers()
	{
		PlayerLayer.Children.Clear();

        Rect imageRect = GetDisplayedImageRect();

		foreach(var player in TacticalPlayers)
		{
            var marker = new PlayerMarker
            {
                PlayerNumber = player.Number.ToString(),
                MarkerColor = player.IsHomeMarker ? HomeColor : AwayColor,
                IsHomeMarker = player.IsHomeMarker,
            };

            marker.SetBinding(PlayerMarker.InputActiveProperty, new Binding(nameof(IsMoveActive), source: this));

            PlayerLayer.Children.Add(marker);

            double x = (player.X * imageRect.Width) + imageRect.Left;
            double y = (player.Y * imageRect.Height) + imageRect.Top;

            AbsoluteLayout.SetLayoutBounds(marker, new Rect(x - 20, y - 20, 40, 40));

            AbsoluteLayout.SetLayoutFlags(marker, AbsoluteLayoutFlags.None);
        }

        var ballMarker = new PlayerMarker
        {
            PlayerNumber = "0",
            MarkerColor = Colors.White,
            IsHomeMarker = false,
            IsBall = true
        };

        ballMarker.SetBinding(PlayerMarker.InputActiveProperty, new Binding(nameof(IsMoveActive), source: this));

        PlayerLayer.Children.Add(ballMarker);

        double ballX = (0.5f * imageRect.Width) + imageRect.Left;
        double ballY = (0.5f * imageRect.Height) + imageRect.Top;

        AbsoluteLayout.SetLayoutBounds(ballMarker, new Rect(ballX - 20, ballY - 20, 40, 40));

        AbsoluteLayout.SetLayoutFlags(ballMarker, AbsoluteLayoutFlags.None);
    }

    private Rect GetDisplayedImageRect()
    {
        double containerWidth = PitchImage.Width;
        double containerHeight = PitchImage.Height;

        if (containerWidth <= 0 || containerHeight <= 0)
            return Rect.Zero;

        // Replace these with the actual dimensions of your image
        double imageWidth = 1017;
        double imageHeight = 632;

        double imageAspect = imageWidth / imageHeight;
        double containerAspect = containerWidth / containerHeight;

        double displayedWidth;
        double displayedHeight;

        if (imageAspect > containerAspect)
        {
            // Image limited by width
            displayedWidth = containerWidth;
            displayedHeight = displayedWidth / imageAspect;
        }
        else
        {
            // Image limited by height
            displayedHeight = containerHeight;
            displayedWidth = displayedHeight * imageAspect;
        }

        double x = (containerWidth - displayedWidth) / 2;
        double y = (containerHeight - displayedHeight) / 2;

        return new Rect(x, y, displayedWidth, displayedHeight);
    }

    private void PointerGestureRecognizer_PointerPressed(object sender, PointerEventArgs e)
    {
        Point position = CalculateMousePositionAsPercent(e);

        switch(SelectedTool)
        {
            case TacticBoardConstants.DrawLineToolName:
                _drawItem = new DrawLine(SelectedDrawColor, position, position);
                break;
            case TacticBoardConstants.DrawArrowToolName:
                _drawItem = new DrawArrow(SelectedDrawColor, position, position);
                break;
        }
        
        if(_drawItem != null)
            _drawable.DrawItems.Add(_drawItem);
    }

    private void PointerGestureRecognizer_PointerMoved(object sender, PointerEventArgs e)
    {
        DrawLine? line = _drawItem as DrawLine;
        if (line != null)
        {
            line.End = CalculateMousePositionAsPercent(e);
            DrawingView.Invalidate();
        }
    }

    private void PointerGestureRecognizer_PointerReleased(object sender, PointerEventArgs e)
    {
        _drawItem = null;
    }

    private PointF CalculateMousePositionAsPercent(PointerEventArgs e)
    {
        Point? position = e.GetPosition(BoardGrid);
        if (position == null)
        {
            return new PointF();
        }

        float x = (float)position.Value.X / (float)BoardGrid.Width;
        float y = (float)position.Value.Y / (float)BoardGrid.Height;

        return new PointF(x, y);
    }
}