using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Data.Arguments;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class ShotStatsPageModel : StatsPageModel
    {
        public event EventHandler? ShotEventsUpdated;

        [ObservableProperty]
        public Match _match;

        private string _selectedTeam = string.Empty;
        public string SelectedTeam
        {
            get => _selectedTeam;
            set
            {
                if (_selectedTeam != value)
                {
                    _selectedTeam = value;
                    LoadStatsForTeam();
                    OnPropertyChanged();
                }
            }
        }

        [ObservableProperty]
        private ObservableCollection<string> _teams = [];

        public StatisticDotDrawable DotDrawable { get; } = new();

        #region Filters
        public bool Show1stHalfEvents
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool Show2ndHalfEvents
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;
        #endregion

        public ShotStatsPageModel(Match match)
        {
            Match = match;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            if (!Match.IsMatchHydrated)
                return;

            Teams.Clear();

            Teams.Add(Match.HomeTeam.TeamName);
            Teams.Add(Match.AwayTeam.TeamName);
            SelectedTeam = Match.HomeTeam.TeamName;
        }

        protected override void CalculateScoresFromEvent()
        {
            throw new NotImplementedException();
        }

        protected override bool CanShowEvent(MatchEvent matchEvent)
        {
            throw new NotImplementedException();
        }

        protected override void FillGraph()
        {
            throw new NotImplementedException();
        }

        protected override void FilterDrawnEvents()
        {
            //throw new NotImplementedException();
        }

        protected override void LoadStatsForTeam()
        {
            //throw new NotImplementedException();
        }
    }
}
