using StatsTrackerV2.Models;

namespace StatsTrackerV2.Pages;

public partial class CreateMatchEventPage : ContentPage
{
	public CreateMatchEventPage(CreateMatchEventPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

	private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		BoxView? boxView = sender as BoxView;
		if (boxView == null)
		{
			return;
		}

		CreateMatchEventPageModel? vm = BindingContext as CreateMatchEventPageModel;
        if (vm == null)
        {
			return;
        }

		Point? point = e.GetPosition(boxView);
		if(point == null)
		{
			return; 
		}

		float x = (float)point.Value.X / (float)boxView.Width;
		float y = (float)point.Value.Y / (float)boxView.Height;

		vm.Location = new PointF(x, y);
		DrawingView.Invalidate();
    }
}