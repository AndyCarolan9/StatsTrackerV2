using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.Pages.Controls;

public partial class KickoutPercentTable : ContentView
{
	public static readonly BindableProperty ItemsProperty =
		BindableProperty.Create(
			nameof(Items),
			typeof(ObservableCollection<KickoutDistanceData>),
			typeof(KickoutPercentTable));

	public ObservableCollection<KickoutDistanceData> Items
	{
		get => (ObservableCollection<KickoutDistanceData>)GetValue(ItemsProperty);
		set => SetValue(ItemsProperty, value);
	}

	public KickoutPercentTable()
	{
		InitializeComponent();
		dataGrid.BindingContext = this;
		dataGrid.SetBinding(CollectionView.ItemsSourceProperty, new Binding(nameof(Items), source: this));
	}
}