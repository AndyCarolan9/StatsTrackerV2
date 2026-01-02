using StatsTrackerV2.Models;
using StatsTrackerV2.PageModels;

namespace StatsTrackerV2.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}