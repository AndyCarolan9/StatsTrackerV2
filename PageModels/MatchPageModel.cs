using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;

namespace StatsTrackerV2.PageModels
{
    public partial class MatchPageModel : ObservableObject
    {
        [ObservableProperty]
        private Match _match;

        public MatchPageModel(Match match)
        {
            _match = match;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            
        }

        [RelayCommand]
        async Task CreateMatch()
        {
            await Shell.Current.GoToAsync($"createMatch");
        }

        [RelayCommand]
        async Task OpenMatch() 
        {
            await Shell.Current.GoToAsync($"openMatch");
        }
    }
}
