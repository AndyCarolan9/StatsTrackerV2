using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class CreateMatchPageModel : ObservableObject
    {
        private Match _match;

        private List<Team> _teams;

        private string _selectedHomeTeam;
        public string SelectedHomeTeam
        {
            get => _selectedHomeTeam;
            set
            {
                if (_selectedHomeTeam != value)
                {
                    _selectedHomeTeam = value;
                    OnSelectedHomeTeamChanged();
                    OnPropertyChanged();
                }
            }
        }


        private string _selectedAwayTeam;
        public string SelectedAwayTeam
        {
            get => _selectedAwayTeam;
            set
            {
                if (_selectedAwayTeam != value)
                {
                    _selectedAwayTeam = value;
                    OnSelectedAwayTeamChanged();
                    OnPropertyChanged();
                }
            }
        }

        [ObservableProperty]
        private ObservableCollection<string> _teamNames = [];

        [ObservableProperty]
        private ObservableCollection<string> _homeTeamPlayers = [];

        [ObservableProperty]
        private ObservableCollection<string> _awayTeamPlayers = [];

        public CreateMatchPageModel(Match match)
        {
            _match = match;
            _teams = new List<Team>();
            _selectedHomeTeam = string.Empty;
            _selectedAwayTeam = string.Empty;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            try
            {
                Team[]? teams = JSONHelper.LoadFromJsonFile<Team[]>(Constants.TeamsJSONPath);
                if (teams == null)
                {
                    return;
                }

                foreach(Team team in teams)
                {
                    TeamNames.Add(team.TeamName);
                }

                _teams.AddRange(teams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        [RelayCommand]
        private async Task AddTeam()
        {
            await Shell.Current.GoToAsync($"teamList");
        }

        private void OnSelectedHomeTeamChanged()
        {
            HomeTeamPlayers.Clear();

            if (SelectedHomeTeam == string.Empty)
            { 
                // Clear starting players array
            }

            Team? selectedTeam = _teams.Find(team => team.TeamName.Equals(SelectedHomeTeam));
            if (selectedTeam == null)
            {
                return;
            }

            foreach(string player in selectedTeam.TeamSheet)
            {
                HomeTeamPlayers.Add(player);
            }
        }

        private void OnSelectedAwayTeamChanged()
        {
            AwayTeamPlayers.Clear();

            if (SelectedAwayTeam == string.Empty)
            {
                // Clear starting players array
            }

            Team? selectedTeam = _teams.Find(team => team.TeamName.Equals(SelectedAwayTeam));
            if (selectedTeam == null)
            {
                return;
            }

            foreach (string player in selectedTeam.TeamSheet)
            {
                AwayTeamPlayers.Add(player);
            }
        }
    }
}
