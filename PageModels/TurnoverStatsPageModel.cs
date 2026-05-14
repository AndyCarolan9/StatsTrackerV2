using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;
using StatsTrackerV2.Models.EventScores;
using StatsTrackerV2.Models.MatchStatistics;
using StatsTrackerV2.Models.ResultColors;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class TurnoverStatsPageModel : StatsPageModel
    {
        public event EventHandler? TurnoverEventsUpdated;

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

        public bool ShowWon
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowLost
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowIntercept
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowTackle
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowFree
        {
            get;
            set
            {
                field = value;
                FilterDrawnEvents();
                OnPropertyChanged();
            }
        } = true;

        public bool ShowBreakingBall
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

        [ObservableProperty]
        public ObservableCollection<EventResultColor> _turnoverResultColors = [];

        private List<TurnoverEvent> _turnoverEvents = [];

        [ObservableProperty]
        private ObservableCollection<TurnoverMatchStatistic> _teamStats = [];

        [ObservableProperty]
        private ObservableCollection<EventScore> _wonEventScores = [];

        [ObservableProperty]
        private ObservableCollection<EventScore> _lostEventScores = [];

        public TurnoverStatsPageModel(Match match)
        {
            Match = match;

            TurnoverResultColors.Add(new TurnoverResultColor(EventType.TurnoverWon, Colors.Green));
            TurnoverResultColors.Add(new TurnoverResultColor(EventType.TurnoverLost, Colors.Red));

            TeamStats.Add(new TurnoverMatchStatistic(EventType.TurnoverWon, "Won Turnovers", 0, 0));
            TeamStats.Add(new TurnoverMatchStatistic(EventType.TurnoverLost, "Lost Turnovers", 0, 0));
            TeamStats.Add(new TurnoverMatchStatistic(EventType.TurnoverWon, TurnoverType.Intercept, "Won by intercept", 0, 0));
            TeamStats.Add(new TurnoverMatchStatistic(EventType.TurnoverLost, TurnoverType.Intercept, "Lost by intercept", 0, 0));
            TeamStats.Add(new TurnoverMatchStatistic(EventType.TurnoverWon, TurnoverType.Tackle, "Won in tackle", 0, 0));
            TeamStats.Add(new TurnoverMatchStatistic(EventType.TurnoverLost, TurnoverType.Tackle, "Lost in tackle", 0, 0));
            TeamStats.Add(new TurnoverMatchStatistic(EventType.TurnoverWon, TurnoverType.Free, "Won by a free", 0, 0));
            TeamStats.Add(new TurnoverMatchStatistic(EventType.TurnoverLost, TurnoverType.Free, "Lost by a free", 0, 0));
            TeamStats.Add(new TurnoverMatchStatistic(EventType.TurnoverWon, TurnoverType.BreakingBall, "Won by breaking ball", 0, 0));
            TeamStats.Add(new TurnoverMatchStatistic(EventType.TurnoverLost, TurnoverType.BreakingBall, "Lost by breaking ball", 0, 0));

            WonEventScores.Add(new TurnoverEventScore(TurnoverType.Intercept));
            WonEventScores.Add(new TurnoverEventScore(TurnoverType.Tackle));
            WonEventScores.Add(new TurnoverEventScore(TurnoverType.Free));
            WonEventScores.Add(new TurnoverEventScore(TurnoverType.BreakingBall));

            LostEventScores.Add(new TurnoverEventScore(EventType.TurnoverLost, TurnoverType.Intercept));
            LostEventScores.Add(new TurnoverEventScore(EventType.TurnoverLost, TurnoverType.Tackle));
            LostEventScores.Add(new TurnoverEventScore(EventType.TurnoverLost, TurnoverType.Free));
            LostEventScores.Add(new TurnoverEventScore(EventType.TurnoverLost, TurnoverType.BreakingBall));
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

        protected override void LoadStatsForTeam()
        {
            UpdateTurnoverScores(true);
            _turnoverEvents.Clear();
            List<MatchEvent> matchEvents = Match.GetMatchEventsOfType(EventType.TurnoverWon).ToList();
            matchEvents.AddRange(Match.GetMatchEventsOfType(EventType.TurnoverLost).ToList());

            foreach (MatchEvent matchEvent in matchEvents)
            {
                TurnoverEvent? turnoverEvent = matchEvent as TurnoverEvent;
                if(turnoverEvent == null)
                {
                    continue;
                }

                _turnoverEvents.Add(turnoverEvent);
            }

            foreach (MatchStatistic item in TeamStats)
            {
                item.FirstHalfValue = 0;
                item.SecondHalfValue = 0;
            }

            FilterDrawnEvents();
            FillGraph();
            CalculateScoresFromEvent();
        }

        protected override void FilterDrawnEvents()
        {
            DotDrawable.Statistics.Clear();

            int index = 1;
            foreach(TurnoverEvent turnover in _turnoverEvents)
            {
                if(CanShowEvent(turnover))
                {
                    EventType type = InvertTurnoverEvent(turnover);

                    EventResultColor? resultColor = TurnoverResultColors.FirstOrDefault(x => x.Type == type);
                    if (resultColor != null)
                    {
                        DotDrawable.Statistics.Add(new DrawableStatistic(index, turnover.Location, resultColor.Color, turnover.HalfIndex == 1));
                        index++;
                    }
                }
            }

            TurnoverEventsUpdated?.Invoke(this, new EventArgs());
        }

        protected override void FillGraph()
        {
            foreach (TurnoverEvent turnoverEvent in _turnoverEvents)
            {
                AddToMatchStat(turnoverEvent);
                AddToMatchStat(turnoverEvent, false);
            }

            TeamStats = new ObservableCollection<TurnoverMatchStatistic>(TeamStats);
        }

        private void AddToMatchStat(TurnoverEvent turnoverEvent, bool isDefaultTurnoverType = true)
        {
            EventType statType = InvertTurnoverEvent(turnoverEvent);

            TurnoverType turnoverType = TurnoverType.Default;
            if(!isDefaultTurnoverType)
            {
                turnoverType = turnoverEvent.TurnoverType;
            }

            MatchStatistic? matchStat = TeamStats.ToList().Find(stat => stat.EventType == statType && stat.TurnoverType == turnoverType);
            if (matchStat == null)
            {
                return;
            }

            if (turnoverEvent.HalfIndex == 1)
            {
                matchStat.FirstHalfValue = matchStat.FirstHalfValue + 1;
            }
            else
            {
                matchStat.SecondHalfValue = matchStat.SecondHalfValue + 1;
            }
        }

        protected override void CalculateScoresFromEvent()
        {
            foreach (TurnoverEvent turnoverEvent in _turnoverEvents)
            {
                bool didScore = Match.DidScoreFromCurrentEvent(turnoverEvent, out MatchEvent? nextEvent);
                if (!didScore)
                    continue;

                ShotEvent? shotEvent = nextEvent as ShotEvent;
                if (shotEvent == null)
                {
                    continue;
                }

                TurnoverEventScore? turnoverEventScore;

                EventType type = InvertTurnoverEvent(turnoverEvent);
                if (type == EventType.TurnoverWon)
                {
                    turnoverEventScore = WonEventScores.ToList().Find(tes =>
                    {
                        TurnoverEventScore? eventScore = tes as TurnoverEventScore;
                        if (eventScore == null)
                        {
                            return false;
                        }

                        return eventScore.TurnoverType == turnoverEvent.TurnoverType;
                    }) as TurnoverEventScore;
                }
                else
                {
                    turnoverEventScore = LostEventScores.ToList().Find(tes =>
                    {
                        TurnoverEventScore? eventScore = tes as TurnoverEventScore;
                        if (eventScore == null)
                        {
                            return false;
                        }

                        return eventScore.TurnoverType == turnoverEvent.TurnoverType;
                    }) as TurnoverEventScore;
                }

                if (turnoverEventScore == null)
                    continue;

                if (shotEvent.HalfIndex == 1)
                {
                    if (shotEvent.ResultType == ShotResultType.Goal)
                    {
                        turnoverEventScore.FirstHalfGoals += 1;
                    }
                    else if (shotEvent.ResultType == ShotResultType.DoublePoint)
                    {
                        turnoverEventScore.FirstHalfPoints += 2;
                    }
                    else
                    {
                        turnoverEventScore.FirstHalfPoints += 1;
                    }
                }
                else
                {
                    if (shotEvent.ResultType == ShotResultType.Goal)
                    {
                        turnoverEventScore.SecondHalfGoals += 1;
                    }
                    else if (shotEvent.ResultType == ShotResultType.DoublePoint)
                    {
                        turnoverEventScore.SecondHalfPoints += 2;
                    }
                    else
                    {
                        turnoverEventScore.SecondHalfPoints += 1;
                    }
                }
            }

            UpdateTurnoverScores();
        }

        private void UpdateTurnoverScores(bool shouldReset = false)
        {
            foreach (EventScore eventScore in WonEventScores)
            {
                eventScore.UpdateScoreValues(shouldReset);
            }

            foreach (EventScore eventScore in LostEventScores)
            {
                eventScore.UpdateScoreValues(shouldReset);
            }
        }

        protected override bool CanShowEvent(MatchEvent matchEvent)
        {
            TurnoverEvent? turnoverEvent = matchEvent as TurnoverEvent;
            if(turnoverEvent == null)
            {
                return false;
            }

            bool canShowEvent = false;
            if (Show1stHalfEvents)
            {
                canShowEvent = turnoverEvent.HalfIndex == 1;
            }

            if (!canShowEvent && Show2ndHalfEvents)
            {
                canShowEvent = turnoverEvent.HalfIndex == 2;
            }

            if(!canShowEvent)
            {
                return false;
            }

            EventType type = InvertTurnoverEvent(turnoverEvent);

            if(ShowWon)
            {
                canShowEvent = type == EventType.TurnoverWon;
            }

            if(!canShowEvent && ShowLost)
            {
                canShowEvent = type == EventType.TurnoverLost;
            }

            if (!canShowEvent)
            {
                return false;
            }

            switch(turnoverEvent.TurnoverType)
            {
                case TurnoverType.Intercept:
                    return ShowIntercept;
                case TurnoverType.Free:
                    return ShowFree;
                case TurnoverType.Tackle:
                    return ShowTackle;
                case TurnoverType.BreakingBall:
                    return ShowBreakingBall;
                default:
                    return false;
            }
        }

        private EventType InvertTurnoverEvent(TurnoverEvent turnoverEvent)
        {
            EventType statType = turnoverEvent.Type;
            if (turnoverEvent.TeamName != SelectedTeam)
            {
                if (turnoverEvent.Type == EventType.TurnoverWon)
                {
                    statType = EventType.TurnoverLost;
                }
                else
                {
                    statType = EventType.TurnoverWon;
                }
            }

            return statType;
        }
    }
}
