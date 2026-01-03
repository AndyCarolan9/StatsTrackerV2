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
        private bool _isCreateOpenButtonsShown;

        [ObservableProperty]
        private bool _isMatchTitleShown;

        public MatchPageModel(Match match)
        {
            _match = match;
            IsCreateOpenButtonsShown = true;
            IsMatchTitleShown = false;
        }

        [RelayCommand]
        async Task CreateMatch()
        {
            //Match.HomeTeam = "Glen Emmets";
            //Match.AwayTeam = "Dundalk Gaels";

            IsCreateOpenButtonsShown = false;
            IsMatchTitleShown = true;
        }

        [RelayCommand]
        async Task OpenMatch() 
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, "Test.JSON");
            Match? match = JSONHelper.LoadFromJsonFile<Match>(filePath);
            if(match is not null)
            {
                Match = match;
                //Match.MatchDisplayName = $"{Match.HomeTeam.TeamName} V {Match.AwayTeam.TeamName}";
                IsCreateOpenButtonsShown = false;
                IsMatchTitleShown = true;
            }
        }
    }
}
