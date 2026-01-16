using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class MatchPageModel : ObservableObject
    {
        [ObservableProperty]
        private Match _match;

        [ObservableProperty]
        public ObservableCollection<MatchStatistic> _homeTeamStats;

        [ObservableProperty]
        public ObservableCollection<MatchStatistic> _awayTeamStats;

        public MatchPageModel(Match match)
        {
            Match = match;

            HomeTeamStats = new ObservableCollection<MatchStatistic>();
            HomeTeamStats.Add(new MatchStatistic(EventType.KickOut, "Kick Outs", 0, 0));
            HomeTeamStats.Add(new MatchStatistic(EventType.FreeConceded, "Frees Conceded", 0, 0));
            HomeTeamStats.Add(new MatchStatistic(EventType.PointShot, "Shots", 0, 0));
            HomeTeamStats.Add(new MatchStatistic(EventType.TurnoverWon, "Turnovers Won", 0, 0));
            HomeTeamStats.Add(new MatchStatistic(EventType.TurnoverLost, "Turnovers Lost", 0, 0));

            AwayTeamStats = new ObservableCollection<MatchStatistic>();
            AwayTeamStats.Add(new MatchStatistic(EventType.KickOut, "Kick Outs", 0, 0));
            AwayTeamStats.Add(new MatchStatistic(EventType.FreeConceded, "Frees Conceded", 0, 0));
            AwayTeamStats.Add(new MatchStatistic(EventType.PointShot, "Shots", 0, 0));
            AwayTeamStats.Add(new MatchStatistic(EventType.TurnoverWon, "Turnovers Won", 0, 0));
            AwayTeamStats.Add(new MatchStatistic(EventType.TurnoverLost, "Turnovers Lost", 0, 0));
        }

        [RelayCommand]
        private async Task Appearing()
        {
            UpdateMatchStats();
        }

        [RelayCommand]
        async Task CreateMatch()
        {
            await Shell.Current.GoToAsync($"createMatch");
        }

        [RelayCommand]
        async Task OpenMatch() 
        {
            await Shell.Current.GoToAsync($"openMatch");
        }

        [RelayCommand]
        async Task StartMatch()
        {
            if(!Match.IsMatchHydrated)
            {
                return;
            }

            Match.StartHalf();
        }

        [RelayCommand]
        async Task PauseMatch()
        {
            if(!Match.IsMatchHydrated)
            {
                return;
            }

            Match.PauseTimer();
        }

        [RelayCommand]
        async Task AddPointShotEvent()
        {
            await OpenCreateMatchEventPage(EventType.PointShot.ToString());
        }

        [RelayCommand]
        async Task AddGoalShotEvent()
        {
            await OpenCreateMatchEventPage(EventType.GoalShot.ToString());
        }

        [RelayCommand]
        async Task Add2PointShotEvent()
        {
            await OpenCreateMatchEventPage(EventType.DoublePointShot.ToString());
        }

        [RelayCommand]
        async Task AddKickoutEvent()
        {
            await OpenCreateMatchEventPage(EventType.KickOut.ToString());
        }

        [RelayCommand]
        async Task AddTurnoverWonEvent()
        {
            await OpenCreateMatchEventPage(EventType.TurnoverWon.ToString());
        }

        [RelayCommand]
        async Task AddTurnoverLostEvent()
        {
            await OpenCreateMatchEventPage(EventType.TurnoverLost.ToString());
        }

        [RelayCommand]
        async Task AddThrowInWonEvent(string isHomeTeamParam)
        {
            if (!Match.IsMatchHydrated || !Match.IsMatchPlaying())
            {
                return;
            }
            await Shell.Current.GoToAsync($"createMatchEvent?eventType=ThrowInWon&isHomeTeam={isHomeTeamParam}");
        }

        [RelayCommand]
        async Task AddFreeConcededEvent()
        {
            await OpenCreateMatchEventPage(EventType.FreeConceded.ToString());
        }

        async Task OpenCreateMatchEventPage(string eventType)
        {
            if (!Match.IsMatchHydrated || !Match.IsMatchPlaying())
            {
                return;
            }
            await Shell.Current.GoToAsync($"createMatchEvent?eventType={eventType}");
        }

        private void UpdateMatchStats()
        {
            if (!Match.IsMatchHydrated || !Match.IsMatchPlaying())
            {
                return;
            }

            if(Match.MatchEvents.Count == 0)
            {
                return;
            }

            bool isHomeTeamStatsChanged = false;
            bool isAwayTeamStatsChanged = false;
            foreach(MatchStatistic matchStatistic in HomeTeamStats)
            {
                int fhTotal = 0;
                int shTotal = 0;

                if (matchStatistic.EventType == EventType.PointShot)
                {
                    fhTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.PointShot, true, 1);
                    fhTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.DoublePointShot, true, 1);
                    fhTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.GoalShot, true, 1);

                    shTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.PointShot, true, 2);
                    shTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.DoublePointShot, true, 2);
                    shTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.GoalShot, true, 2);
                }
                else
                {
                    fhTotal = Match.GetNumberOfEventsOfTypeForTeam(matchStatistic.EventType, true, 1);
                    shTotal = Match.GetNumberOfEventsOfTypeForTeam(matchStatistic.EventType, true, 2);
                }

                if (matchStatistic.FirstHalfValue != fhTotal || matchStatistic.SecondHalfValue != shTotal)
                {
                    matchStatistic.FirstHalfValue = fhTotal;
                    matchStatistic.SecondHalfValue = shTotal;
                    isHomeTeamStatsChanged = true;
                }
            }

            foreach (MatchStatistic matchStatistic in AwayTeamStats)
            {
                int fhTotal = 0;
                int shTotal = 0;

                if (matchStatistic.EventType == EventType.PointShot)
                {
                    fhTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.PointShot, false, 1);
                    fhTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.DoublePointShot, false, 1);
                    fhTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.GoalShot, false, 1);

                    shTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.PointShot, false, 2);
                    shTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.DoublePointShot, false, 2);
                    shTotal += Match.GetNumberOfEventsOfTypeForTeam(EventType.GoalShot, false, 2);
                }
                else
                {
                    fhTotal = Match.GetNumberOfEventsOfTypeForTeam(matchStatistic.EventType, false, 1);
                    shTotal = Match.GetNumberOfEventsOfTypeForTeam(matchStatistic.EventType, false, 2);
                }

                if (matchStatistic.FirstHalfValue != fhTotal || matchStatistic.SecondHalfValue != shTotal)
                {
                    matchStatistic.FirstHalfValue = fhTotal;
                    matchStatistic.SecondHalfValue = shTotal;
                    isAwayTeamStatsChanged = true;
                }
            }

            if(isHomeTeamStatsChanged)
            {
                HomeTeamStats = new ObservableCollection<MatchStatistic>(HomeTeamStats);
            }

            if(isAwayTeamStatsChanged)
            {
                AwayTeamStats = new ObservableCollection<MatchStatistic>(AwayTeamStats);
            }
        }
    }
}
