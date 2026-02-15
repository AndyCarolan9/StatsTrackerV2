using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Data.Arguments;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class SubstitutionPageModel : ObservableObject, IQueryAttributable
    {
        private Match _match;

        [ObservableProperty]
        private Team _selectedTeam;

        [ObservableProperty]
        private ObservableCollection<SubstitutionParameters> _subsList;

        [ObservableProperty]
        private ObservableCollection<Player> _currentTeam;

        [ObservableProperty]
        private ObservableCollection<Player> _availablePlayers;

        [ObservableProperty]
        private Player _selectedPlayerOff = new Player();

        [ObservableProperty]
        private Player _selectedPlayerOn = new Player();

        public SubstitutionPageModel(Match match)
        {
            _match = match;
            SelectedTeam = new Team();
            CurrentTeam = new ObservableCollection<Player>();
            AvailablePlayers = new ObservableCollection<Player>();
            SubsList = new ObservableCollection<SubstitutionParameters>();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.ContainsKey("isHomeTeam"))
            {
                string? param = Convert.ToString(query["isHomeTeam"]);
                if (param == null)
                {
                    Shell.Current.GoToAsync("..");
                    return;
                }

                bool isHomeTeam = bool.Parse(param);

                if (isHomeTeam)
                {
                    SelectedTeam = _match.HomeTeam;
                }
                else
                {
                    SelectedTeam = _match.AwayTeam;
                }

                PopulatePlayerLists();
            }
        }

        private void PopulatePlayerLists()
        {
            for (int i = 0; i < SelectedTeam.CurrentTeam.Count(); i++)
            {
                CurrentTeam.Add(new Player() { Index = (i + 1).ToString(), Name = SelectedTeam.CurrentTeam[i] });
            }

            int subIndex = 16;
            for (int j = 0; j < SelectedTeam.TeamSheet.Count(); j++)
            {
                if(SelectedTeam.CurrentTeam.Contains(SelectedTeam.TeamSheet[j]))
                {
                    continue;
                }

                AvailablePlayers.Add(new Player() { Index = subIndex.ToString(), Name = SelectedTeam.TeamSheet[j] });
                subIndex++;
            }
        }

        [RelayCommand]
        private async Task MakeSub()
        {
            if (!IsValidSub())
            {
                return;
            }

            SubsList.Add(new SubstitutionParameters() { PlayerOff = SelectedPlayerOff.Name, PlayerOn = SelectedPlayerOn.Name });
            SelectedPlayerOff = new Player();
            SelectedPlayerOn = new Player();
        }

        [RelayCommand]
        private async Task Confirm()
        {
            foreach(SubstitutionParameters substitution in SubsList)
            {
                SubstitutionEventArgs substitutionEvent = new SubstitutionEventArgs();
                substitutionEvent.Player = substitution.PlayerOff;
                substitutionEvent.SubstitutePlayer = substitution.PlayerOn;
                substitutionEvent.EventType = EventType.Substitution;
                substitutionEvent.Team = SelectedTeam;

                _match.AddEvent(substitutionEvent);
            }

            await Shell.Current.GoToAsync("..");
        }

        private bool IsValidSub()
        {
            if (SelectedPlayerOn.Name == string.Empty)
            {
                return false;
            }

            if (SelectedPlayerOff.Name == string.Empty)
            {
                return false;
            }

            foreach (SubstitutionParameters substitutionParameters in SubsList)
            {
                if(substitutionParameters.PlayerOn == SelectedPlayerOn.Name || substitutionParameters.PlayerOff == SelectedPlayerOn.Name
                     || substitutionParameters.PlayerOn == SelectedPlayerOff.Name || substitutionParameters.PlayerOff == SelectedPlayerOff.Name)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
