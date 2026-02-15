using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;
using StatsTrackerV2.Models.MatchStatistics;
using StatsTrackerV2.Models.ResultColors;
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

        [ObservableProperty]
        public ObservableCollection<EventResultColor> _shotResultColors = [];

        [ObservableProperty]
        private ObservableCollection<ShotMatchStatistic> _teamStats = [];

        private List<ShotEvent> _shotEvents = [];

        public ShotStatsPageModel(Match match)
        {
            Match = match;

            ShotResultColors.Add(new ShotResultColor(EventType.Shots, ShotResultType.Point, Colors.White));
            ShotResultColors.Add(new ShotResultColor(EventType.Shots, ShotResultType.DoublePoint, Colors.DarkOrange));
            ShotResultColors.Add(new ShotResultColor(EventType.Shots, ShotResultType.Goal, Colors.Green));
            ShotResultColors.Add(new ShotResultColor(EventType.Shots, ShotResultType.Wide, Colors.Red));
            ShotResultColors.Add(new ShotResultColor(EventType.Shots, ShotResultType.Short, Colors.DarkRed));
            ShotResultColors.Add(new ShotResultColor(EventType.Shots, ShotResultType.Saved, Colors.IndianRed));
            ShotResultColors.Add(new ShotResultColor(EventType.Shots, ShotResultType.SavedOutFor45, Colors.MediumPurple));
            ShotResultColors.Add(new ShotResultColor(EventType.Shots, ShotResultType.OutFor45, Colors.Purple));
            ShotResultColors.Add(new ShotResultColor(EventType.Shots, ShotResultType.Blocked, Colors.PeachPuff));
            ShotResultColors.Add(new ShotResultColor(EventType.Shots, ShotResultType.OffPosts, Colors.Fuchsia));

            TeamStats.Add(new ShotMatchStatistic(EventType.PointShot, "Point Shots", 0, 0));
            TeamStats.Add(new ShotMatchStatistic(EventType.DoublePointShot, "2 Point Shots", 0, 0));
            TeamStats.Add(new ShotMatchStatistic(EventType.GoalShot, "Goal Shots", 0, 0));
            TeamStats.Add(new ShotMatchStatistic(ShotResultType.Point, "Points Scored", 0, 0));
            TeamStats.Add(new ShotMatchStatistic(ShotResultType.DoublePoint, "2 Pointers Scored", 0, 0));
            TeamStats.Add(new ShotMatchStatistic(ShotResultType.Goal, "Goals Scored", 0, 0));
            TeamStats.Add(new ShotMatchStatistic(ShotResultType.Wide, "Wides", 0, 0));
            TeamStats.Add(new ShotMatchStatistic(ShotResultType.OutFor45, "Out for 45", 0, 0));
            TeamStats.Add(new ShotMatchStatistic(ShotResultType.Saved, "Saved/Blocked/Off Posts", 0, 0));
            TeamStats.Add(new ShotMatchStatistic(ShotResultType.Short, "Short", 0, 0));
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
            
        }

        protected override bool CanShowEvent(MatchEvent matchEvent)
        {
            return true;
        }

        protected override void FillGraph()
        {
            foreach(ShotEvent shotEvent in _shotEvents)
            {
                AddToMatchStat(shotEvent, true);
                AddToMatchStat(shotEvent);
            }

            TeamStats = new ObservableCollection<ShotMatchStatistic>(TeamStats);
        }

        private void AddToMatchStat(ShotEvent shotEvent, bool isDefaultResult = false)
        {
            ShotResultType shotResultType;
            EventType eventType;
            if(isDefaultResult)
            {
                shotResultType = ShotResultType.Default;
                eventType = shotEvent.Type;
            }
            else
            {
                if(shotEvent.ResultType == ShotResultType.Saved || shotEvent.ResultType == ShotResultType.Blocked || shotEvent.ResultType == ShotResultType.OffPosts)
                {
                    shotResultType = ShotResultType.Saved;
                }
                else if(shotEvent.ResultType == ShotResultType.SavedOutFor45 || shotEvent.ResultType == ShotResultType.OutFor45)
                {
                    shotResultType = ShotResultType.OutFor45;
                }
                else
                {
                    shotResultType = shotEvent.ResultType;
                }

                eventType = EventType.Shots;
            }

            MatchStatistic? matchStat = TeamStats.ToList().Find(stat => stat.EventType == eventType && stat.ResultType == shotResultType);
            if(matchStat == null)
            {
                return;
            }

            if(shotEvent.HalfIndex == 1)
            {
                matchStat.FirstHalfValue = matchStat.FirstHalfValue + 1;
            }
            else
            {
                matchStat.SecondHalfValue = matchStat.SecondHalfValue + 1;
            }
        }

        protected override void FilterDrawnEvents()
        {
            DotDrawable.Statistics.Clear();

            foreach(ShotEvent shotEvent in _shotEvents)
            {
                if(CanShowEvent(shotEvent))
                {
                    EventResultColor? resultColor = ShotResultColors.FirstOrDefault(x =>
                    {
                        ShotResultColor? color = x as ShotResultColor;
                        if (color != null)
                        {
                            return color.ResultType == shotEvent.ResultType;
                        }

                        return false;
                    });

                    if (resultColor != null)
                    {
                        DotDrawable.Statistics.Add(new DrawableStatistic(shotEvent.Location, resultColor.Color, shotEvent.HalfIndex == 1));
                    }
                }
            }

            ShotEventsUpdated?.Invoke(this, new EventArgs());
        }

        protected override void LoadStatsForTeam()
        {
            _shotEvents.Clear();

            MatchEvent[] matchEvents = Match.GetMatchEventsOfType<ShotEvent>().Where(me => me.TeamName == SelectedTeam).ToArray();

            foreach (MatchEvent matchEvent in matchEvents)
            {
                ShotEvent? shotEvent = matchEvent as ShotEvent;
                if (shotEvent == null)
                {
                    continue;
                }

                _shotEvents.Add(shotEvent);
            }

            foreach (MatchStatistic item in TeamStats)
            {
                item.FirstHalfValue = 0;
                item.SecondHalfValue = 0;
            }

            FilterDrawnEvents();
            FillGraph();
        }
    }
}
