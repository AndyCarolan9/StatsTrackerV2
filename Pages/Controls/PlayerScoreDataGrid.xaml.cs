using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.Pages.Controls;

public partial class PlayerScoreDataGrid : ContentView
{
    public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(
                nameof(Items),
                typeof(ObservableCollection<PlayerScore>),
                typeof(DataGrid));

    public ObservableCollection<PlayerScore> Items
    {
        get => (ObservableCollection<PlayerScore>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public PlayerScoreDataGrid()
	{
		InitializeComponent();
        dataGrid.BindingContext = this;
        dataGrid.SetBinding(CollectionView.ItemsSourceProperty, new Binding(nameof(Items), source: this));
    }
}