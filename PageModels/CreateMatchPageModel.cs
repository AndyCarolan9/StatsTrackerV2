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

        private Team _homeTeam;

        private Team _awayTeam;

        private const int MaxStartingPlayers = 15;

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

        public ObservableCollection<Player> HomeAvailablePlayers { get; }

        public ObservableCollection<Player> HomeStartingTeam { get; }

        public ObservableCollection<Player> AwayAvailablePlayers { get; }

        public ObservableCollection<Player> AwayStartingTeam { get; }

        public CreateMatchPageModel(Match match)
        {
            _match = match;
            _teams = new List<Team>();
            _homeTeam = new Team();
            _awayTeam = new Team();
            _selectedHomeTeam = string.Empty;
            _selectedAwayTeam = string.Empty;

            HomeAvailablePlayers = new();
            HomeStartingTeam = new();
            AwayAvailablePlayers = new();
            AwayStartingTeam = new();
        }

        public void MoveToHomeStartingTeam(Player player)
        {
            if (HomeStartingTeam.Count >= MaxStartingPlayers)
                return;

            if (HomeStartingTeam.Contains(player))
                return;

            if (!HomeAvailablePlayers.Contains(player))
                return;

            int Index = HomeStartingTeam.Count + 1;
            player.Index = Index.ToString();

            HomeAvailablePlayers.Remove(player);
            HomeStartingTeam.Add(player);
        }

        public void ReorderHomeStartingTeam(Player dragged, Player target)
        {
            if (dragged == target)
                return;

            int oldIndex = HomeStartingTeam.IndexOf(dragged);
            int newIndex = HomeStartingTeam.IndexOf(target);

            dragged.Index = (newIndex + 1).ToString();
            target.Index = (oldIndex + 1).ToString();

            if (oldIndex < 0 || newIndex < 0)
                return;

            HomeStartingTeam.Move(oldIndex, newIndex);
        }

        public void MoveToHomeAvailablePlayers(Player player)
        {
            if (HomeAvailablePlayers.Contains(player))
                return;

            if (!_homeTeam.TeamSheet.Contains(player.Name))
                return;

            player.Index = string.Empty;
            HomeStartingTeam.Remove(player);

            for(int i = 0; i < HomeStartingTeam.Count; i++)
            {
                int pos = i + 1;
                HomeStartingTeam[i].Index = pos.ToString();
            }

            HomeAvailablePlayers.Add(player);
        }

        public void MoveToAwayStartingTeam(Player player)
        {
            if (AwayStartingTeam.Count >= MaxStartingPlayers)
                return;

            if (AwayStartingTeam.Contains(player))
                return;

            if (!AwayAvailablePlayers.Contains(player))
                return;

            int Index = AwayStartingTeam.Count + 1;
            player.Index = Index.ToString();

            AwayAvailablePlayers.Remove(player);
            AwayStartingTeam.Add(player);
        }

        public void ReorderAwayStartingTeam(Player dragged, Player target)
        {
            if (dragged == target)
                return;

            int oldIndex = AwayStartingTeam.IndexOf(dragged);
            int newIndex = AwayStartingTeam.IndexOf(target);

            dragged.Index = (newIndex + 1).ToString();
            target.Index = (oldIndex + 1).ToString();

            if (oldIndex < 0 || newIndex < 0)
                return;

            AwayStartingTeam.Move(oldIndex, newIndex);
        }

        public void MoveToAwayAvailablePlayers(Player player)
        {
            if (AwayAvailablePlayers.Contains(player))
                return;

            if(!_awayTeam.TeamSheet.Contains(player.Name))
                return;

            player.Index = string.Empty;
            AwayStartingTeam.Remove(player);

            for (int i = 0; i < AwayStartingTeam.Count; i++)
            {
                int pos = i + 1;
                AwayStartingTeam[i].Index = pos.ToString();
            }

            AwayAvailablePlayers.Add(player);
        }

        [RelayCommand]
        private async Task Appearing()
        {
            _teams.Clear();
            TeamNames.Clear();

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

        private void OnSelectedHomeTeamChanged()
        {
            HomeAvailablePlayers.Clear();
            HomeStartingTeam.Clear();

            Team? selectedTeam = _teams.Find(team => team.TeamName.Equals(SelectedHomeTeam));
            if (selectedTeam == null)
            {
                return;
            }

            _homeTeam = selectedTeam;

            foreach(string player in selectedTeam.TeamSheet)
            {
                HomeAvailablePlayers.Add(new Player { Name = player });
            }
        }

        private void OnSelectedAwayTeamChanged()
        {
            AwayAvailablePlayers.Clear();
            AwayStartingTeam.Clear();

            Team? selectedTeam = _teams.Find(team => team.TeamName.Equals(SelectedAwayTeam));
            if (selectedTeam == null)
            {
                return;
            }

            _awayTeam = selectedTeam;

            foreach (string player in selectedTeam.TeamSheet)
            {
                AwayAvailablePlayers.Add(new Player { Name = player });
            }
        }

        [RelayCommand]
        private async Task ConfirmClicked()
        {
            _homeTeam.CurrentTeam = GetStartingTeam(HomeStartingTeam);
            _awayTeam.CurrentTeam = GetStartingTeam(AwayStartingTeam);
            _match.HydrateObject(new Match(_homeTeam, _awayTeam));
            _match.StartAutoSave();

            await Shell.Current.GoToAsync("..");
        }

        private string[] GetStartingTeam(ObservableCollection<Player> team)
        {
            string[] players = new string[15];

            for (int i = 0; i < 15; i++)
            {
                if(i < team.Count)
                {
                    players[i] = team[i].Name;
                }
                else
                {
                    players[i] = (i + 1).ToString();
                }
                
            }

            return players;
        }
    }
}
