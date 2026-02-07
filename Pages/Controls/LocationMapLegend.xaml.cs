using StatsTrackerV2.Models.ResultColors;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.Pages.Controls;

public partial class LocationMapLegend : ContentView
{
    public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(
                nameof(Items),
                typeof(ObservableCollection<EventResultColor>),
                typeof(LocationMapLegend));

    public ObservableCollection<EventResultColor> Items
    {
        get => (ObservableCollection<EventResultColor>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public LocationMapLegend()
	{
		InitializeComponent();
        legendList.BindingContext = this;
        legendList.SetBinding(CollectionView.ItemsSourceProperty, new Binding(nameof(Items), source: this));
	}
}