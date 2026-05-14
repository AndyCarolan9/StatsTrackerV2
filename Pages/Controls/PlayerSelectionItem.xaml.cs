using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StatsTrackerV2.Pages.Controls;

public partial class PlayerSelectionItem : ContentView
{
	public static readonly BindableProperty JerseyColorProperty =
		BindableProperty.Create(
			nameof(JerseyColor),
			typeof(Color),
			typeof(PlayerSelectionItem),
			Colors.White,
			propertyChanged: OnJerseyColorChanged);

	public Color JerseyColor
	{
		get => (Color)GetValue(JerseyColorProperty);
		set => SetValue(JerseyColorProperty, value);
    }

	public static readonly BindableProperty JerseyNumberProperty =
		BindableProperty.Create(
			nameof(JerseyNumber),
			typeof(string),
			typeof(PlayerSelectionItem),
			"0");

	public string JerseyNumber
	{
		get => (string)GetValue(JerseyNumberProperty);
		set => SetValue(JerseyNumberProperty, value);
	}

	public static readonly BindableProperty PlayersListProperty =
		BindableProperty.Create(
			nameof(PlayersList),
			typeof(ObservableCollection<string>),
			typeof(PlayerSelectionItem));

	public ObservableCollection<string> PlayersList
	{
		get => (ObservableCollection<string>)GetValue(PlayersListProperty);
		set => SetValue(PlayersListProperty, value);
	}

	public static readonly BindableProperty SelectedPlayerProperty =
		BindableProperty.Create(
			nameof(SelectedPlayer),
			typeof(string),
			typeof(PlayerSelectionItem),
			"",
			BindingMode.TwoWay);

	public string? SelectedPlayer
	{
		get => (string)GetValue(SelectedPlayerProperty);
		set => SetValue(SelectedPlayerProperty, value);
	}

	public static readonly BindableProperty ClearCommandProperty =
		BindableProperty.Create(
			nameof(ClearCommand),
			typeof(ICommand),
			typeof(PlayerSelectionItem),
			default(ICommand),
			BindingMode.OneWay);

	public ICommand ClearCommand
	{
		get => (ICommand)GetValue(ClearCommandProperty);
		set => SetValue(ClearCommandProperty, value);
	}

	public static readonly BindableProperty ClearCommandParameterProperty =
		BindableProperty.Create(
			nameof(ClearCommandParameter),
			typeof(object),
			typeof(PlayerSelectionItem));

	public object ClearCommandParameter
	{
		get => GetValue(ClearCommandParameterProperty);
		set => SetValue(ClearCommandParameterProperty, value);
	}

	public Color TextColor
	{
		get;
		set;
	}

	public PlayerSelectionItem()
	{
		InitializeComponent();
		TextColor = Colors.Black;
        OnPropertyChanged(nameof(TextColor));
    }

	private static void OnJerseyColorChanged(BindableObject bindable, object oldValue, object newValue)
	{
        var control = (PlayerSelectionItem)bindable;
        var newColor = (Color)newValue;

        control.UpdateTextColor(newColor);
    }

	private void UpdateTextColor(Color newJerseyColor)
	{
        if (ColorsHelper.IsColorDark(newJerseyColor))
        {
            TextColor = Colors.White;
        }
        else
        {
            TextColor = Colors.Black;
        }

		OnPropertyChanged(nameof(TextColor));
    }
}