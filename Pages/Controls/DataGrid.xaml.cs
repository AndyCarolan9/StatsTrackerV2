using StatsTrackerV2.Models.EventScores;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.Pages.Controls
{
    public partial class DataGrid : ContentView
    {
        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(
                nameof(Items),
                typeof(ObservableCollection<EventScore>),
                typeof(DataGrid));

        public ObservableCollection<EventScore> Items
        {
            get => (ObservableCollection<EventScore>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public DataGrid()
        {
            InitializeComponent();
            dataGrid.BindingContext = this;
            dataGrid.SetBinding(CollectionView.ItemsSourceProperty, new Binding(nameof(Items), source: this));
        }
    }
}

