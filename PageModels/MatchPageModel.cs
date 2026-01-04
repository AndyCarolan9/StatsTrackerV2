using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;

namespace StatsTrackerV2.PageModels
{
    public partial class MatchPageModel : ObservableObject
    {
        [ObservableProperty]
        private Match _match;

        [ObservableProperty]
        public bool _isCreateOpenButtonsShown;

        [ObservableProperty]
        public bool _isMatchTitleShown;

        public MatchPageModel(Match match)
        {
            _match = match;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            IsMatchTitleShown = Match._isHydrated;
            IsCreateOpenButtonsShown = !Match._isHydrated;
        }

        [RelayCommand]
        async Task CreateMatch()
        {
            //IsCreateOpenButtonsShown = false;
            //IsMatchTitleShown = true;
        }

        [RelayCommand]
        async Task OpenMatch() 
        {
            await Shell.Current.GoToAsync($"openMatch");
        }
    }
}
