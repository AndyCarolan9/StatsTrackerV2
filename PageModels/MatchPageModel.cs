using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;

namespace StatsTrackerV2.PageModels
{
    public partial class MatchPageModel : ObservableObject
    {
        [ObservableProperty]
        private Match _match;

        public MatchPageModel(Match match)
        {
            _match = match;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            
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
    }
}
