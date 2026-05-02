using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Data.Arguments;
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
        private string _teamToEdit = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> _teamNames = [];

        [ObservableProperty]
        private ObservableCollection<string> _homePlayers;

        [ObservableProperty]
        private ObservableCollection<PlayerPositionSelect> _homePositions;

        [ObservableProperty]
        private Team? _homeSelectedTeam;

        [ObservableProperty]
        private ObservableCollection<string> _awayPlayers;

        [ObservableProperty]
        private ObservableCollection<PlayerPositionSelect> _awayPositions;

        [ObservableProperty]
        private Team? _awaySelectedTeam;

        [ObservableProperty]
        private bool _showHomeTeam = true;

        [ObservableProperty]
        private bool _showAwayTeam = false;

        public CreateMatchPageModel(Match match)
        {
            _match = match;
            _teams = new List<Team>();
            _selectedHomeTeam = string.Empty;
            _selectedAwayTeam = string.Empty;

            HomePlayers = new();
            HomePositions = new ObservableCollection<PlayerPositionSelect>
            {
                new PlayerPositionSelect(1, 1, 0),
                new PlayerPositionSelect(2, 2, 1),
                new PlayerPositionSelect(3, 1, 1),
                new PlayerPositionSelect(4, 0, 1),
                new PlayerPositionSelect(5, 2, 2),
                new PlayerPositionSelect(6, 1, 2),
                new PlayerPositionSelect(7, 0, 2),
                new PlayerPositionSelect(8, 0, 3),
                new PlayerPositionSelect(9, 2, 3),
                new PlayerPositionSelect(10, 2, 4),
                new PlayerPositionSelect(11, 1, 4),
                new PlayerPositionSelect(12, 0, 4),
                new PlayerPositionSelect(13, 2, 5),
                new PlayerPositionSelect(14, 1, 5),
                new PlayerPositionSelect(15, 0, 5),
            };

            AwayPlayers = new();
            AwayPositions = new ObservableCollection<PlayerPositionSelect>
            {
                new PlayerPositionSelect(1, 1, 5),
                new PlayerPositionSelect(2, 0, 4),
                new PlayerPositionSelect(3, 1, 4),
                new PlayerPositionSelect(4, 2, 4),
                new PlayerPositionSelect(5, 0, 3),
                new PlayerPositionSelect(6, 1, 3),
                new PlayerPositionSelect(7, 2, 3),
                new PlayerPositionSelect(8, 2, 2),
                new PlayerPositionSelect(9, 0, 2),
                new PlayerPositionSelect(10, 0, 1),
                new PlayerPositionSelect(11, 1, 1),
                new PlayerPositionSelect(12, 2, 1),
                new PlayerPositionSelect(13, 0, 0),
                new PlayerPositionSelect(14, 1, 0),
                new PlayerPositionSelect(15, 2, 0),
            };
        }

        [RelayCommand]
        private async Task Appearing()
        {
            LoadTeams();
        }

        [RelayCommand]
        private async Task AddTeam()
        {
            await Shell.Current.GoToAsync($"teamList");
        }

        [RelayCommand]
        private async Task EditTeam()
        {
            Team? selectedTeam = _teams.Find(team => team.TeamName.Equals(TeamToEdit));
            if (selectedTeam == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"teamList?teamName={selectedTeam.TeamName}");
        }

        [RelayCommand]
        private async Task ClearPosition(PlayerPositionSelect playerPositionSelect)
        {
            playerPositionSelect.SelectedPlayer = "";
        }

        [RelayCommand]
        private async Task ChangeVisibleTeam()
        {
            ShowHomeTeam = !ShowHomeTeam;
            ShowAwayTeam = !ShowAwayTeam;
        }

        private void OnSelectedHomeTeamChanged()
        {
            HomePlayers.Clear();

            Team? selectedTeam = _teams.Find(team => team.TeamName.Equals(SelectedHomeTeam));
            if (selectedTeam == null)
            {
                return;
            }

            HomeSelectedTeam = selectedTeam;

            foreach (string player in selectedTeam.TeamSheet)
            {
                HomePlayers.Add(player);
            }
        }

        private void OnSelectedAwayTeamChanged()
        {
            AwayPlayers.Clear();

            Team? selectedTeam = _teams.Find(team => team.TeamName.Equals(SelectedAwayTeam));
            if (selectedTeam == null)
            {
                return;
            }

            AwaySelectedTeam = selectedTeam;

            foreach (string player in selectedTeam.TeamSheet)
            {
                AwayPlayers.Add(player);
            }
        }

        [RelayCommand]
        private async Task ConfirmClicked()
        {
            if(HomeSelectedTeam == null || AwaySelectedTeam == null) { return; }

            HomeSelectedTeam.SetStartingTeam(GetHomeStartingTeam());
            AwaySelectedTeam.SetStartingTeam(GetAwayStartingTeam());
            _match.HydrateObject(new Match(HomeSelectedTeam, AwaySelectedTeam));
            _match.StartAutoSave();

            await Shell.Current.GoToAsync("..");
        }

        private string[] GetHomeStartingTeam()
        {
            List<string> startingTeam = new List<string>();

            foreach (PlayerPositionSelect p in HomePositions)
            {
                startingTeam.Add(p.SelectedPlayer);
            }

            return startingTeam.ToArray();
        }

        private string[] GetAwayStartingTeam()
        {
            List<string> startingTeam = new List<string>();

            foreach (PlayerPositionSelect p in AwayPositions)
            {
                startingTeam.Add(p.SelectedPlayer);
            }

            return startingTeam.ToArray();
        }

        [RelayCommand]
        private async Task ExportTeamsJSON()
        {
            if(File.Exists(Constants.TeamsJSONPath))
            {
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Share Teams JSON",
                    File = new ShareFile(Constants.TeamsJSONPath)
                });
            }
        }

        [RelayCommand]
        private async Task ImportTeamsJSON()
        {
            await JSONHelper.ImportTeamsJSON(_teams);

            HomeSelectedTeam = null;
            HomePlayers.Clear();
            AwaySelectedTeam = null;
            AwayPlayers.Clear();
            LoadTeams();
        }

        private void LoadTeams()
        {
            _teams.Clear();
            TeamNames.Clear();

            Team[]? teams = JSONHelper.LoadFromJsonFile<Team[]>(Constants.TeamsJSONPath);
            if (teams == null)
            {
                return;
            }

            foreach (Team team in teams)
            {
                TeamNames.Add(team.TeamName);
            }

            _teams.AddRange(teams);
        }
    }
}
