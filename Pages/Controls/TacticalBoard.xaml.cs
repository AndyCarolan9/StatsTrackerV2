using Microsoft.Maui.Layouts;
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

	private readonly TacticalDrawable _drawable = new TacticalDrawable();

	private PointF? _startPoint = null;

	public TacticalBoard()
	{
		InitializeComponent();

        DrawingView.Drawable = _drawable;
    }

    private void BoardGrid_Tapped(object sender, TappedEventArgs e)
    {
		Point? position = e.GetPosition(BoardGrid);
		if(position == null)
		{
			return;
		}

		float x = (float)position.Value.X / (float)BoardGrid.Width;
		float y = (float)position.Value.Y / (float)BoardGrid.Height;

		if(_startPoint == null)
		{
			_startPoint = new PointF(x, y);
			return;
		}

		DrawLine newLine = new DrawLine((PointF)_startPoint, new PointF(x, y));
		_drawable.Lines.Add(newLine);
		_startPoint = null;
		DrawingView.Invalidate();
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
                MarkerColor = player.IsHomeMarker ? Colors.Green : Colors.Red
            };

            PlayerLayer.Children.Add(marker);

            double x = (player.X * imageRect.Width) + imageRect.Left;
            double y = (player.Y * imageRect.Height) + imageRect.Top;

            AbsoluteLayout.SetLayoutBounds(marker, new Rect(x - 20, y - 20, 40, 40));

            AbsoluteLayout.SetLayoutFlags(marker, AbsoluteLayoutFlags.None);
        }
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
}