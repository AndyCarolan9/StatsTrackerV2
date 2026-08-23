using CommunityToolkit.Maui.Views;

namespace StatsTrackerV2.Pages.Controls;

public partial class TacticalBoard : ContentView
{
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
}