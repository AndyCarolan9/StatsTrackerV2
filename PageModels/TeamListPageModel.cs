using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Data;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class TeamListPageModel : ObservableObject, IQueryAttributable
    {
        private Team? _team;

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

            Players = new()
            {
                "Andy",
                "Alex"
            };
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

            _team = new Team();
            _team.TeamName = TeamName;
            _team.TeamColor = SelectedColor;
            string[] PlayersToAdd = Players.ToArray();
            Array.Sort(PlayersToAdd);
            _team.TeamSheet.AddRange(PlayersToAdd);

            try
            {
                bool bWasTeamAdded = false;
                Team[]? teams = JSONHelper.LoadFromJsonFile<Team[]>(Constants.TeamsJSONPath);
                if (teams == null)
                {
                    teams = [_team];
                    bWasTeamAdded = true;
                }

                if(!bWasTeamAdded)
                {
                    for (int index = 0; index < teams.Count(); index++)
                    {
                        if (teams[index].TeamName == TeamName)
                        {
                            teams[index] = _team;
                        }
                    }
                }
                
                if(!bWasTeamAdded)
                {
                    teams.Append(_team);
                }

                JSONHelper.SaveToJsonFile(Constants.TeamsJSONPath, teams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            //throw new NotImplementedException();
        }
    }
}
