using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class KickoutStatsPageModel : ObservableObject
    {
        public event EventHandler? KickoutEventsUpdated;

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

        [ObservableProperty]
        private ObservableCollection<MatchStatistic> _teamStats = [];

        public StatisticDotDrawable DotDrawable { get; } = new();

        private Dictionary<KickOutEvent, Color> _kickoutEvents = new Dictionary<KickOutEvent, Color>();

        #region Filter values
        public bool Show1stHalfEvents
        {
            get;
            set
            {
                field = value;
                FilterDrawnKickoutEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool Show2ndHalfEvents
        {
            get;
            set
            {
                field = value;
                FilterDrawnKickoutEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowWonClean
        {
            get;
            set
            {
                field = value;
                FilterDrawnKickoutEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowWonMark
        {
            get;
            set
            {
                field = value;
                FilterDrawnKickoutEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowWonBreak
        {
            get;
            set
            {
                field = value;
                FilterDrawnKickoutEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowLostClean
        {
            get;
            set
            {
                field = value;
                FilterDrawnKickoutEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowLostMark
        {
            get;
            set
            {
                field = value;
                FilterDrawnKickoutEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowLostBreak
        {
            get;
            set
            {
                field = value;
                FilterDrawnKickoutEvents();
                OnPropertyChanged();
            }
        } = true;
        #endregion

        public KickoutStatsPageModel(Match match)
        {
            _match = match;

            TeamStats.Add(new MatchStatistic(KickOutResultType.Won, "Won Clean", 0, 0));
            TeamStats.Add(new MatchStatistic(KickOutResultType.WonMark, "Won Mark", 0, 0));
            TeamStats.Add(new MatchStatistic(KickOutResultType.WonBreak, "Won Break", 0, 0));
            TeamStats.Add(new MatchStatistic(KickOutResultType.Lost, "Lost Clean", 0, 0));
            TeamStats.Add(new MatchStatistic(KickOutResultType.LostMark, "Lost Mark", 0, 0));
            TeamStats.Add(new MatchStatistic(KickOutResultType.LostBreak, "Lost Break", 0, 0));
        }

        [RelayCommand]
        private async Task Appearing()
        {
            if (!_match.IsMatchHydrated)
                return;

            Teams.Add(_match.HomeTeam.TeamName);
            Teams.Add(_match.AwayTeam.TeamName);
            SelectedTeam = _match.HomeTeam.TeamName;
            LoadStatsForTeam();
        }

        private void LoadStatsForTeam()
        {
            _kickoutEvents.Clear();
            MatchEvent[] matchEvents = _match.GetMatchEventsOfType(EventType.KickOut).Where(me => me.TeamName == SelectedTeam).ToArray();
            List<KickoutResultColor> resultColors = kickoutResultColors.ToList();

            foreach (MatchEvent matchEvent in matchEvents)
            {
                KickOutEvent? kickOutEvent = matchEvent as KickOutEvent;
                if(kickOutEvent == null)
                {
                    continue;
                }

                _kickoutEvents.Add(kickOutEvent, GetColorForResultType(kickOutEvent));
            }

            foreach (MatchStatistic item in TeamStats)
            {
                item.FirstHalfValue = 0;
                item.SecondHalfValue = 0;
            }

            FilterDrawnKickoutEvents();
            FillGraph();
        }

        private Color GetColorForResultType(KickOutEvent kickOutEvent)
        {
            foreach(KickoutResultColor color in kickoutResultColors)
            {
                if(color.Type == kickOutEvent.ResultType)
                {
                    return color.Color;
                }
            }

            return Colors.Black;
        }

        private void FilterDrawnKickoutEvents()
        {
            DotDrawable.Statistics.Clear();
            List<KickOutEvent> EventsToDisplay = new List<KickOutEvent>();
            foreach (var item in _kickoutEvents)
            {
                if(CanShowEvent(item.Key))
                {
                    DotDrawable.Statistics.Add(new DrawableStatistic(item.Key.Location, item.Value));
                }
            }
            KickoutEventsUpdated?.Invoke(this, new EventArgs());
        }

        private bool CanShowEvent(KickOutEvent kickOutEvent)
        {
            bool canShowEvent = false;
            if(Show1stHalfEvents)
            {
                canShowEvent = kickOutEvent.HalfIndex == 1;
            }
            else if(Show2ndHalfEvents)
            {
                canShowEvent = kickOutEvent.HalfIndex == 2;
            }

            if (!canShowEvent)
                return false;

            switch(kickOutEvent.ResultType)
            {
                case KickOutResultType.Won:
                    return ShowWonClean;
                case KickOutResultType.WonMark:
                    return ShowWonMark;
                case KickOutResultType.WonBreak:
                    return ShowWonBreak;
                case KickOutResultType.Lost:
                    return ShowLostClean;
                case KickOutResultType.LostMark:
                    return ShowLostMark;
                case KickOutResultType.LostBreak:
                    return ShowLostBreak;
                default:
                    return false;
            }
        }

        private void FillGraph()
        {
            foreach(var kickoutEvent in _kickoutEvents)
            {
                switch(kickoutEvent.Key.ResultType)
                {
                    case KickOutResultType.Won:
                        AddToMatchStat(KickOutResultType.Won, kickoutEvent.Key);
                        break;
                    case KickOutResultType.WonMark:
                        AddToMatchStat(KickOutResultType.WonMark, kickoutEvent.Key);
                        break;
                    case KickOutResultType.WonBreak:
                        AddToMatchStat(KickOutResultType.WonBreak, kickoutEvent.Key);
                        break;
                    case KickOutResultType.Lost:
                        AddToMatchStat(KickOutResultType.Lost, kickoutEvent.Key);
                        break;
                    case KickOutResultType.LostMark:
                        AddToMatchStat(KickOutResultType.LostMark, kickoutEvent.Key);
                        break;
                    case KickOutResultType.LostBreak:
                        AddToMatchStat(KickOutResultType.LostBreak, kickoutEvent.Key);
                        break;
                    default:
                        break;
                }
            }

            TeamStats = new ObservableCollection<MatchStatistic>(TeamStats);
        }

        private void AddToMatchStat(KickOutResultType resultType, KickOutEvent kickoutEvent)
        {
            MatchStatistic? stat = TeamStats.ToList().Find(matchStat => matchStat.KickOutResultType == resultType);
            if (stat == null)
                return;

            if (kickoutEvent.HalfIndex == 1)
            {
                stat.FirstHalfValue = stat.FirstHalfValue + 1;
            }
            else
            {
                stat.SecondHalfValue++;
            }
        }
    }
}
