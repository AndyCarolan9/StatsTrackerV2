using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models.EventScores;
using StatsTrackerV2.Models.ResultColors;
using StatsTrackerV2.Models.MatchStatistics;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class KickoutStatsPageModel : StatsPageModel
    {
        public event EventHandler? KickoutEventsUpdated;

        [ObservableProperty]
        private Match _match;

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

        [ObservableProperty]
        public ObservableCollection<EventResultColor> _kickoutResultColors = [];        

        [ObservableProperty]
        private ObservableCollection<KickoutMatchStatistic> _teamStats = [];

        public StatisticDotDrawable DotDrawable { get; } = new();

        private Dictionary<KickOutEvent, Color> _kickoutEvents = new Dictionary<KickOutEvent, Color>();

        [ObservableProperty]
        private ObservableCollection<EventScore> _eventScores = [];

        [ObservableProperty]
        private ObservableCollection<KickoutDistanceData> _kickoutDistanceDatas = [];

        #region Filter values
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

        public bool ShowWonClean
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowWonMark
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowWonBreak
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowLostClean
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowLostMark
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowLostBreak
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

        public KickoutStatsPageModel(Match match)
        {
            Match = match;

            TeamStats.Add(new KickoutMatchStatistic(KickOutResultType.Won, "Won Clean", 0, 0));
            TeamStats.Add(new KickoutMatchStatistic(KickOutResultType.WonMark, "Won Mark", 0, 0));
            TeamStats.Add(new KickoutMatchStatistic(KickOutResultType.WonBreak, "Won Break", 0, 0));
            TeamStats.Add(new KickoutMatchStatistic(KickOutResultType.Lost, "Lost Clean", 0, 0));
            TeamStats.Add(new KickoutMatchStatistic(KickOutResultType.LostMark, "Lost Mark", 0, 0));
            TeamStats.Add(new KickoutMatchStatistic(KickOutResultType.LostBreak, "Lost Break", 0, 0));

            EventScores.Add(new KickoutEventScore(KickOutResultType.Won));
            EventScores.Add(new KickoutEventScore(KickOutResultType.WonMark));
            EventScores.Add(new KickoutEventScore(KickOutResultType.WonBreak));
            EventScores.Add(new KickoutEventScore(KickOutResultType.Lost));
            EventScores.Add(new KickoutEventScore(KickOutResultType.LostMark));
            EventScores.Add(new KickoutEventScore(KickOutResultType.LostBreak));

            KickoutResultColors.Add(new KickoutResultColor(KickOutResultType.Won, Colors.Green));
            KickoutResultColors.Add(new KickoutResultColor(KickOutResultType.WonMark, Colors.DarkGreen));
            KickoutResultColors.Add(new KickoutResultColor(KickOutResultType.WonBreak, Colors.GreenYellow));
            KickoutResultColors.Add(new KickoutResultColor(KickOutResultType.Lost, Colors.Red));
            KickoutResultColors.Add(new KickoutResultColor(KickOutResultType.LostMark, Colors.DarkRed));
            KickoutResultColors.Add(new KickoutResultColor(KickOutResultType.LostBreak, Colors.IndianRed));

            KickoutDistanceDatas.Add(new KickoutDistanceData(KickoutDistance.Short));
            KickoutDistanceDatas.Add(new KickoutDistanceData(KickoutDistance.Medium));
            KickoutDistanceDatas.Add(new KickoutDistanceData(KickoutDistance.Long));
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
            LoadStatsForTeam();
        }

        protected override void LoadStatsForTeam()
        {
            UpdateKickoutScores(true);
            _kickoutEvents.Clear();
            MatchEvent[] matchEvents = Match.GetMatchEventsOfType(EventType.KickOut).Where(me => me.TeamName == SelectedTeam).ToArray();

            foreach (KickoutDistanceData item in KickoutDistanceDatas)
            {
                item.Reset();
            }

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

            FilterDrawnEvents();
            FillGraph();
            CalculateScoresFromEvent();
            UpdateKickoutDistances();
        }

        private Color GetColorForResultType(KickOutEvent kickOutEvent)
        {
            foreach(KickoutResultColor color in KickoutResultColors)
            {
                if(color.ResultType == kickOutEvent.ResultType)
                {
                    return color.Color;
                }
            }

            return Colors.Black;
        }

        protected override void FilterDrawnEvents()
        {
            DotDrawable.Statistics.Clear();
            int index = 1;
            foreach (var item in _kickoutEvents)
            {
                if(CanShowEvent(item.Key))
                {
                    DotDrawable.Statistics.Add(new DrawableStatistic(index, item.Key.Location, item.Value, item.Key.HalfIndex == 1));
                    index++;
                }
            }
            KickoutEventsUpdated?.Invoke(this, new EventArgs());
        }

        protected override bool CanShowEvent(MatchEvent matchEvent)
        {
            KickOutEvent? kickOutEvent = matchEvent as KickOutEvent;
            if(kickOutEvent == null)
                return false;

            bool canShowEvent = false;
            if(Show1stHalfEvents)
            {
                canShowEvent = kickOutEvent.HalfIndex == 1;
            }
            
            if(!canShowEvent && Show2ndHalfEvents)
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

        protected override void FillGraph()
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

            TeamStats = new ObservableCollection<KickoutMatchStatistic>(TeamStats);
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
                stat.SecondHalfValue = stat.SecondHalfValue + 1;
            }
        }

        protected override void CalculateScoresFromEvent()
        {
            foreach (KickOutEvent kickOutEvent in _kickoutEvents.Keys)
            {
                bool didScore = Match.DidScoreFromCurrentEvent(kickOutEvent, out MatchEvent? nextEvent);
                if(!didScore)
                {
                    continue; 
                }

                ShotEvent? shotEvent = nextEvent as ShotEvent;
                if(shotEvent == null)
                {
                    continue; 
                }

                KickoutEventScore? kickoutEventScore = EventScores.ToList().Find(kes =>
                {
                    KickoutEventScore? eventScore = kes as KickoutEventScore;
                    if(eventScore == null)
                    {
                        return false;
                    }

                    return eventScore.ResultType == kickOutEvent.ResultType;
                }) as KickoutEventScore;

                if(kickoutEventScore == null)
                {
                    continue; 
                }

                if(shotEvent.HalfIndex == 1)
                {
                    if(shotEvent.ResultType == ShotResultType.Goal)
                    {
                        kickoutEventScore.FirstHalfGoals += 1;
                    }
                    else if(shotEvent.ResultType == ShotResultType.DoublePoint)
                    {
                        kickoutEventScore.FirstHalfPoints += 2;
                    }
                    else
                    {
                        kickoutEventScore.FirstHalfPoints += 1;
                    }
                }
                else
                {
                    if (shotEvent.ResultType == ShotResultType.Goal)
                    {
                        kickoutEventScore.SecondHalfGoals += 1;
                    }
                    else if (shotEvent.ResultType == ShotResultType.DoublePoint)
                    {
                        kickoutEventScore.SecondHalfPoints += 2;
                    }
                    else
                    {
                        kickoutEventScore.SecondHalfPoints += 1;
                    }
                }
            }

            UpdateKickoutScores();
        }

        private void UpdateKickoutScores(bool shouldReset = false)
        {
            foreach(EventScore eventScore in EventScores)
            {
                eventScore.UpdateScoreValues(shouldReset);
            }
        }

        private void UpdateKickoutDistances()
        {
            foreach(KickOutEvent kickOutEvent in _kickoutEvents.Keys)
            {
                // Calculate distance from end line
                // < 45 m short, < 55 medium, rest long
                KickoutDistance distanceType;
                float distance = 145 * kickOutEvent.Location.X;

                if(distance < 45)
                {
                    distanceType = KickoutDistance.Short;
                }
                else if(distance < 65)
                {
                    distanceType = KickoutDistance.Medium;
                }
                else
                {
                    distanceType = KickoutDistance.Long;
                }

                KickoutDistanceData? data = KickoutDistanceDatas.FirstOrDefault(x => x.distance == distanceType);
                if (data != null)
                {
                    data.TotalKickouts += 1;

                    if(kickOutEvent.ResultType.IsKickOutWon())
                    {
                        data.TotalWon += 1;
                    }

                    data.CalculatePercent();
                }
            }
        }
    }
}
