using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Data.Events.Arguments;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class CreateMatchEventPageModel : ObservableObject, IQueryAttributable
    {
        private Match _match;

        private Team _team = new Team();

        private EventType _eventType;

        [ObservableProperty]
        private Player _selectedPlayer = new Player();

        public ObservableCollection<Player> Players { get; }

        [ObservableProperty]
        private string _selectedActionType = string.Empty;

        public ObservableCollection<string> ActionTypes { get; }

        [ObservableProperty]
        private string _selectedResultType = string.Empty;

        public ObservableCollection<string> ResultTypes { get; }

        [ObservableProperty]
        private bool _isPossessionChanged = false;

        [ObservableProperty]
        public bool _isShotEvent = false;

        [ObservableProperty]
        public bool _showResultList = false;

        [ObservableProperty]
        private bool _canShowPossessionCheck = false;

        public PointF Location 
        { 
            get; 
            set
            {
                field = value;
                DotDrawable.Dots.Clear();
                DotDrawable.Dots.Add(value);
            }
        }

        public DotDrawable DotDrawable { get; } = new();

        public CreateMatchEventPageModel(Match match)
        {
            _match = match;

            Players = new();
            ActionTypes = new();
            ResultTypes = new();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("eventType"))
            {
                bool wasParsed = Enum.TryParse(Convert.ToString(query["eventType"]), out _eventType);
                if (!wasParsed)
                {
                    Shell.Current.GoToAsync("..");
                }

                IsShotEvent = _eventType.IsShotEvent();

                _team = _match.GetTeamForEvent(_eventType);

                for (int i = 0; i < _team.CurrentTeam.Count(); i++)
                {
                    Players.Add(new Player() { Index = (i + 1).ToString(), Name = _team.CurrentTeam[i] });
                }

                PopulateActionTypes();
                PopulateResultTypes();
            }

            if(query.ContainsKey("isHomeTeam"))
            {
                string? param = Convert.ToString(query["isHomeTeam"]);
                if (param == null)
                    return;

                bool isHomeTeam = bool.Parse(param);

                if (isHomeTeam)
                    _team = _match.HomeTeam;
                else
                    _team = _match.AwayTeam;
            }
        }

        private void PopulateActionTypes()
        {
            ActionTypes.Clear();

            if(_eventType.IsShotEvent())
            {
                foreach (ActionType action in Enum.GetValues(typeof(ActionType)))
                {
                    if(action == ActionType.Default)
                    {
                        continue; 
                    }

                    ActionTypes.Add(action.ToString());
                }
            }
        }

        private void PopulateResultTypes()
        {
            ResultTypes.Clear();

            if(_eventType.IsShotEvent())
            {
                foreach(ShotResultType shotResultType in Enum.GetValues(typeof(ShotResultType)))
                {
                    if(shotResultType == ShotResultType.Default)
                    {
                        continue; 
                    }

                    ResultTypes.Add(shotResultType.GetEventName());
                }
            }
            else if(_eventType == EventType.KickOut)
            {
                foreach(KickOutResultType kickOutResultType in Enum.GetValues(typeof(KickOutResultType)))
                {
                    if(kickOutResultType == KickOutResultType.Default)
                    {
                        continue; 
                    }

                    ResultTypes.Add(kickOutResultType.GetEventName());
                }
            }
            else if (_eventType.IsTurnoverEvent())
            {
                foreach (TurnoverType turnoverType in Enum.GetValues(typeof(TurnoverType)))
                {
                    if (turnoverType == TurnoverType.Default)
                    {
                        continue;
                    }

                    ResultTypes.Add(turnoverType.ToString());
                }
            }

            if (ResultTypes.Count > 0)
            {
                ShowResultList = true;
            }
        }

        [RelayCommand]
        private async Task ConfirmClicked()
        {
            if (SelectedPlayer.Name == string.Empty)
                return;

            if (_eventType.IsShotEvent())
            {
                if (SelectedResultType == string.Empty)
                    return;

                if (SelectedActionType == string.Empty)
                    return;

                bool wasActionParsed = Enum.TryParse(SelectedActionType, out ActionType actionType);

                bool wasResultParsed = Enum.TryParse(SelectedResultType.Replace(" ", ""), out ShotResultType result);
                if (!wasResultParsed)
                    return;

                ShotEventArgs shotEventArgs = new ShotEventArgs();
                shotEventArgs.Location = Location;
                shotEventArgs.Player = SelectedPlayer.Name;
                shotEventArgs.EventType = _eventType;
                shotEventArgs.Team = _team;
                shotEventArgs.ResultType = result;
                shotEventArgs.ActionType = actionType;
                shotEventArgs.IsTurnedOver = IsPossessionChanged;

                _match.AddEvent(shotEventArgs);
            }
            else if (_eventType.IsTurnoverEvent())
            {
                if (SelectedResultType == string.Empty)
                    return;

                bool wasResultParsed = Enum.TryParse(SelectedResultType, out TurnoverType result);
                if (!wasResultParsed)
                    return;

                TurnoverEventArgs turnoverEventArgs = new TurnoverEventArgs();
                turnoverEventArgs.Location = Location;
                turnoverEventArgs.EventType = _eventType;
                turnoverEventArgs.Team = _team;
                turnoverEventArgs.TurnoverType = result;
                turnoverEventArgs.Player = SelectedPlayer.Name;

                _match.AddEvent(turnoverEventArgs);
            }
            else if(_eventType == EventType.KickOut)
            {
                if (SelectedResultType == string.Empty)
                    return;

                bool wasResultParsed = Enum.TryParse(SelectedResultType, out KickOutResultType result);
                if (!wasResultParsed)
                    return;

                KickOutEventArgs kickOutEventArgs = new KickOutEventArgs();
                kickOutEventArgs.Location = Location;
                kickOutEventArgs.EventType = _eventType;
                kickOutEventArgs.Team = _team;
                kickOutEventArgs.ResultType = result;
                kickOutEventArgs.Player = SelectedPlayer.Name;

                _match.AddEvent(kickOutEventArgs);
            }
            else
            {
                InputStatEventArgs inputStatEventArgs = new InputStatEventArgs();
                inputStatEventArgs.Location = Location;
                inputStatEventArgs.EventType = _eventType;
                inputStatEventArgs.Team = _team;
                inputStatEventArgs.Player = SelectedPlayer.Name;

                _match.AddEvent(inputStatEventArgs);
            }

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task SelectedResultChanged()
        {
            if (!_eventType.IsShotEvent())
                return;

            bool wasResultParsed = Enum.TryParse(SelectedResultType, out ShotResultType result);
            if (!wasResultParsed)
                return;

            CanShowPossessionCheck = result.IsStillInPlay();
        }
    }
}
