using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class KickoutStatsPageModel : ObservableObject
    {
        private readonly Match _match;

        
        private string _selectedTeam = string.Empty;
        public string SelectedTeam
        {
            get => _selectedTeam;
            set
            {
                if(_selectedTeam != value)
                {
                    _selectedTeam = value;
                    LoadStatsForTeam();
                    OnPropertyChanged();
                }
            }
        }

        [ObservableProperty]
        private ObservableCollection<string> _teams = [];

        public ObservableCollection<KickoutResultColor> kickoutResultColors = new ObservableCollection<KickoutResultColor>
        {
            new KickoutResultColor(KickOutResultType.Won, Colors.Green),
            new KickoutResultColor(KickOutResultType.WonMark, Colors.DarkGreen),
            new KickoutResultColor(KickOutResultType.WonBreak, Colors.GreenYellow),
            new KickoutResultColor(KickOutResultType.Lost, Colors.Red),
            new KickoutResultColor(KickOutResultType.LostMark, Colors.DarkRed),
            new KickoutResultColor(KickOutResultType.LostBreak, Colors.IndianRed)
        };

        public StatisticDotDrawable DotDrawable { get; } = new();

        public KickoutStatsPageModel(Match match)
        {
            _match = match;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            if (!_match.IsMatchHydrated)
                return;

            Teams.Add(_match.HomeTeam.TeamName);
            Teams.Add(_match.AwayTeam.TeamName);
            SelectedTeam = _match.HomeTeam.TeamName;
        }

        private void LoadStatsForTeam()
        {
            MatchEvent[] matchEvents = _match.GetMatchEventsOfType(EventType.KickOut);
            List<KickoutResultColor> resultColors = kickoutResultColors.ToList();

            foreach (MatchEvent matchEvent in matchEvents)
            {
                KickOutEvent? kickOutEvent = matchEvent as KickOutEvent;
                if(kickOutEvent == null)
                {
                    continue;
                }

                
            }
        }
    }
}
