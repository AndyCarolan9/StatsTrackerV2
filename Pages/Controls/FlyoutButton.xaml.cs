using System.Windows.Input;

namespace StatsTrackerV2.Pages.Controls;

public partial class FlyoutButton : ContentView
{
	bool isExpanded = false;

	public static readonly BindableProperty MainBtnImageProperty =
		BindableProperty.Create(
			nameof(MainBtnImage),
			typeof(ImageSource),
			typeof(FlyoutButton));

	public ImageSource MainBtnImage
	{
		get => (ImageSource)GetValue(MainBtnImageProperty);
		set => SetValue(MainBtnImageProperty, value);
	}

    public static readonly BindableProperty Option1ImageProperty =
        BindableProperty.Create(
            nameof(Option1Image),
            typeof(ImageSource),
            typeof(FlyoutButton));

    public ImageSource Option1Image
    {
        get => (ImageSource)GetValue(Option1ImageProperty);
        set => SetValue(Option1ImageProperty, value);
    }

    public static readonly BindableProperty Option2ImageProperty =
        BindableProperty.Create(
            nameof(Option2Image),
            typeof(ImageSource),
            typeof(FlyoutButton));

    public ImageSource Option2Image
    {
        get => (ImageSource)GetValue(Option2ImageProperty);
        set => SetValue(Option2ImageProperty, value);
    }

	public static readonly BindableProperty Option1CommandProperty =
		BindableProperty.Create(
			nameof(Option1Command),
			typeof(ICommand),
			typeof(FlyoutButton),
			default(ICommand),
			BindingMode.OneWay);

	public ICommand Option1Command
	{
		get => (ICommand)GetValue(Option1CommandProperty);
		set => SetValue(Option1CommandProperty, value);
	}

    public static readonly BindableProperty Option2CommandProperty =
        BindableProperty.Create(
            nameof(Option2Command),
            typeof(ICommand),
            typeof(FlyoutButton),
            default(ICommand),
            BindingMode.OneWay);

    public ICommand Option2Command
    {
        get => (ICommand)GetValue(Option2CommandProperty);
        set => SetValue(Option2CommandProperty, value);
    }

    public FlyoutButton()
	{
		InitializeComponent();
	}

	async void MainBtn_Clicked(object sender, EventArgs e)
	{
		if (!isExpanded)
		{
			Option1.IsVisible = true;
			Option2.IsVisible = true;

			await Task.WhenAll(
				Option1.FadeToAsync(1, 200),
				Option2.FadeToAsync(1, 200)
				);
		}
		else
		{
			await Task.WhenAll(
				Option1.FadeToAsync(0, 200),
				Option2.FadeToAsync(0, 200)
				);

            Option1.IsVisible = false;
            Option2.IsVisible = false;
        }
		isExpanded = !isExpanded;
	}
}
