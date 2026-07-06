#if ANDROID
using Android.Content;
using Android.Provider;
#endif

using StatsTrackerV2.Models.ResultColors;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.Pages.Controls;

public partial class PitchDisplay : ContentView, IStatsControl
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

    public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(
                nameof(Items),
                typeof(ObservableCollection<EventResultColor>),
                typeof(PitchDisplay));

    public ObservableCollection<EventResultColor> Items
    {
        get => (ObservableCollection<EventResultColor>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

	public static readonly BindableProperty ShowLegendProperty =
		BindableProperty.Create(
			nameof(ShowLegend),
			typeof(bool),
			typeof(PitchDisplay),
			false);

	public bool ShowLegend
	{
		get => (bool)GetValue(ShowLegendProperty);
		set => SetValue(ShowLegendProperty, value);
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

		legend.BindingContext = this;
		legend.SetBinding(LocationMapLegend.ItemsProperty, new Binding(nameof(Items), source: this));
		legendBorder.SetBinding(Border.IsVisibleProperty, new Binding(nameof(ShowLegend), source: this));

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

    public async void ExportControl(string fileName)
    {
        var pitchImage = await baseGrid.CaptureAsync();
        if (pitchImage != null)
        {
            using var stream = await pitchImage.OpenReadAsync();
			fileName = string.Concat(fileName, ".png");

#if WINDOWS
		var path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
		var file = Path.Combine(path, fileName);

		await using var fileStream = File.Create(file);
		await stream.CopyToAsync(fileStream);

#elif ANDROID
		var resolver = Android.App.Application.Context.ContentResolver;

		var values = new ContentValues();
		values.Put(MediaStore.IMediaColumns.DisplayName, fileName);
		values.Put(MediaStore.IMediaColumns.MimeType, "image/png");
		values.Put(
		    MediaStore.IMediaColumns.RelativePath,
		    $"{Android.OS.Environment.DirectoryPictures}/YourApp");

		var uri = resolver.Insert(
		    MediaStore.Images.Media.ExternalContentUri,
		    values);

		if (uri != null)
		{
		    await using var output = resolver.OpenOutputStream(uri);
		    if (output != null)
		    {
		        await stream.CopyToAsync(output);
		    }
		}
	#endif
        }
    }
}