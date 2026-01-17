using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class TeamListPageModel : ObservableObject, IQueryAttributable
    {
        private List<Team> _teams = [];

        [ObservableProperty]
        private string _teamName = string.Empty;

        [ObservableProperty]
        private Color _selectedColor = Colors.White;

        [ObservableProperty]
        private bool _isOpen = false;

        [ObservableProperty]
        private ObservableCollection<string> _players = [];

        [ObservableProperty]
        private ObservableCollection<Color> _pickableColors;

        [ObservableProperty]
        private string _newPlayer = string.Empty;

        public TeamListPageModel() 
        {
            PickableColors = new()
            {
                Colors.Green,
                Colors.GreenYellow,
                Colors.YellowGreen,
                Colors.Red,
                Colors.DarkRed,
                Colors.OrangeRed,
                Colors.MediumVioletRed,
                Colors.PaleVioletRed,
                Colors.Blue,
                Colors.Aqua,
                Colors.DeepSkyBlue,
                Colors.DarkSlateBlue,
                Colors.BlueViolet,
                Colors.Maroon,
                Colors.MediumPurple,
                Colors.Yellow,
                Colors.LightYellow,
                Colors.LightGoldenrodYellow,
                Colors.Black,
                Colors.White,
                Colors.Gray,
                Colors.DarkGray,
                Colors.LightGray,
                Colors.Silver,
                Colors.Brown,
            };

            Team[]? teams = JSONHelper.LoadFromJsonFile<Team[]>(Constants.TeamsJSONPath);
            if (teams == null)
            {
                _teams = new List<Team>();
                return;
            }

            _teams = teams.ToList();
        }

        [RelayCommand]
        private async Task OpenColorPicker()
        {
            IsOpen = !IsOpen;
        }

        [RelayCommand]
        private async Task AddNewPlayer()
        {
            if(NewPlayer == string.Empty)
            {
                return;
            }

            Players.Add(NewPlayer);
            NewPlayer = string.Empty;
        }

        [RelayCommand]
        private async Task DeletePlayer(string playerToDelete)
        {
            if(playerToDelete == string.Empty)
            {
                return;
            }

            Players.Remove(playerToDelete);
        }

        [RelayCommand]
        private async Task Save()
        {
            if (TeamName == string.Empty)
            {
                return;
            }

            if(Players.Count == 0)
            {
                return;
            }

            Team team = new Team();
            team.TeamName = TeamName;
            team.TeamColor = SelectedColor;
            string[] PlayersToAdd = Players.ToArray();
            Array.Sort(PlayersToAdd);
            team.TeamSheet.AddRange(PlayersToAdd);

            bool bWasTeamAdded = false;
            if(!bWasTeamAdded)
            {
                for (int index = 0; index < _teams.Count(); index++)
                {
                    if (_teams[index].TeamName == TeamName)
                    {
                        _teams[index] = team;
                    }
                }
            }
            
            if(!bWasTeamAdded)
            {
                _teams.Add(team);
            }

            JSONHelper.SaveToJsonFile(Constants.TeamsJSONPath, _teams);
            await Shell.Current.GoToAsync("..");
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("teamName"))
            {
                string? teamName = Convert.ToString(query["teamName"]);
                if (teamName != null)
                {
                    Team? foundTeam = _teams.FirstOrDefault(x => x.TeamName == teamName);
                    if (foundTeam != null)
                    {
                        TeamName = foundTeam.TeamName;
                        SelectedColor = foundTeam.TeamColor;
                        Players = new(foundTeam.TeamSheet);
                    }
                }
            }
        }
    }
}
