using CommunityToolkit.Mvvm.ComponentModel;
using StatsTrackerV2.Data.Events;
using StatsTrackerV2.Data.Events.Arguments;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace StatsTrackerV2.Models
{
    public partial class Match : ObservableObject
    {
        #region Fields

        private Stopwatch _matchTimer;

        // Dictionary storing which players have a black card and how much time has elapsed on their card.
        private Dictionary<string, long> _blackCardedPlayers = new Dictionary<string, long>();

        private int _half = 0;

        private bool _isHomeTeamInPossession = false;

        private bool _isPlayStarted = false;

        [ObservableProperty]
        [JsonIgnore]
        public string _matchDisplayName = "";

        [ObservableProperty]
        [JsonIgnore]
        public string _homeTeamScore = "0-00";

        [ObservableProperty]
        [JsonIgnore]
        public string _AwayTeamScore = "0-00";

        [ObservableProperty]
        [JsonIgnore]
        public bool _isMatchHydrated;

        [ObservableProperty]
        [JsonIgnore]
        public bool _isDefaultMatch = true;

        [ObservableProperty]
        [JsonIgnore]
        public string _halfDisplayText = "1st Half";

        [ObservableProperty]
        [JsonIgnore]
        public string _displayTime = "00:00";

        #endregion

        #region Constructors
        public Match()
        {
            MatchDisplayName = string.Empty;
            MatchName = Guid.NewGuid().ToString();
            MatchEvents = new List<MatchEvent>();
            _matchTimer = new Stopwatch();
            HomeTeam = new Team();
            AwayTeam = new Team();
        }

        public Match(Team homeTeam, Team awayTeam)
        {
            AppVersion = "1.0";
            MatchName = Guid.NewGuid().ToString();
            MatchEvents = new List<MatchEvent>();
            _matchTimer = new Stopwatch();
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
        }

        public Match(Team homeTeam, Team awayTeam, string matchName, List<MatchEvent> matchEvents)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            MatchName = matchName;
            MatchEvents = matchEvents;
            _matchTimer = new Stopwatch();
        }
        #endregion

        #region Properties
        public string? AppVersion { get; set; }

        public string MatchName { get; set; }

        [ObservableProperty]
        public List<MatchEvent> _matchEvents;

        [ObservableProperty]
        public Team _homeTeam;

        [ObservableProperty]
        public Team _awayTeam;

        partial void OnHomeTeamChanged(Team value)
        {
            if (AwayTeam != null)
            {
                MatchDisplayName = $"{value.TeamName} V {AwayTeam.TeamName}";
            }
        }
        
        partial void OnAwayTeamChanged(Team value)
        {      
            if (HomeTeam != null)
            {
                MatchDisplayName = $"{HomeTeam.TeamName} V {value.TeamName}";
            }
        }

        #endregion

        #region Methods
        public void HydrateObject(Match match)
        {
            AppVersion = match.AppVersion;
            MatchName = match.MatchName;
            MatchEvents = match.MatchEvents;
            HomeTeam = match.HomeTeam;
            AwayTeam = match.AwayTeam;
            IsMatchHydrated = true;
            IsDefaultMatch = false;
        }

        public Dictionary<string, long> GetBlackCardedPlayers()
        {
            return _blackCardedPlayers;
        }

        public bool IsHomeTeamInPossession()
        {
            return _isHomeTeamInPossession;
        }

        public void SetIsHomeTeamInPossession(bool isHomeTeamInPossession)
        {
            _isHomeTeamInPossession = isHomeTeamInPossession;
        }

        public bool IsMatchValid()
        {
            return HomeTeam.IsTeamValid() && AwayTeam.IsTeamValid();
        }

        public MatchEvent? GetLastMatchEvent()
        {
            if (MatchEvents.Count == 0)
            {
                return null;
            }

            return MatchEvents[MatchEvents.Count - 1];
        }

        #region Timer Methods
        /// <summary>
        /// Starts the match timer and increments the half number.
        /// </summary>
        public void StartHalf()
        {
            if (_isPlayStarted)
            {
                StopHalf();
                return;
            }

            _matchTimer = Stopwatch.StartNew();
            _half += 1;
            _isPlayStarted = true;
            AddEvent(new MatchEvent(new PointF(), "", 0, EventType.HalfStart, null, _half));
        }

        /// <summary>
        /// Stops the match timer.
        /// </summary>
        private void StopHalf()
        {
            _matchTimer.Stop();
            _isPlayStarted = false;
            AddEvent(new MatchEvent(new PointF(), "", _matchTimer.ElapsedMilliseconds, EventType.HalfEnd, null, _half));
        }

        /// <summary>
        /// Pauses the match timer.
        /// </summary>
        public void PauseTimer()
        {
            if (_isPlayStarted && !_matchTimer.IsRunning)
            {
                ResumeTimer();
                return;
            }
            _matchTimer.Stop();
        }

        /// <summary>
        /// Resumes the match timer.
        /// </summary>
        private void ResumeTimer()
        {
            _matchTimer.Start();
        }

        public TimeSpan GetElapsedTime()
        {
            return _matchTimer.Elapsed;
        }

        public long GetElapsedTimeInMilliseconds()
        {
            return _matchTimer.ElapsedMilliseconds;
        }

        public int GetHalf()
        {
            return _half;
        }

        public bool IsMatchPlaying()
        {
            return _isPlayStarted;
        }
        #endregion

        #region Event Methods
        public void AddEvent(MatchEvent matchEvent)
        {
            MatchEvents.Add(matchEvent);
            if (matchEvent.Type.IsTurnoverEvent())
            {
                _isHomeTeamInPossession = !_isHomeTeamInPossession;
            }
        }

        public void RemoveEvent(MatchEvent matchEvent)
        {
            MatchEvents.Remove(matchEvent);
        }

        public void AddEvent(InputStatEventArgs statArgs)
        {
            if (statArgs is ShotEventArgs shotArgs)
            {
                AddEvent(shotArgs);
                return;
            }

            if (statArgs is KickOutEventArgs kickOutArgs)
            {
                AddEvent(kickOutArgs);
                return;
            }

            if (statArgs is TurnoverEventArgs turnOverArgs)
            {
                AddEvent(turnOverArgs);
                return;
            }

            if (statArgs is SubstitutionEventArgs subEventArgs)
            {
                AddEvent(subEventArgs);
                return;
            }

            if (statArgs.EventType == EventType.BlackCard)
            {
                // Start timer linked to main timer tracking when this player is back in play.
                AddBlackCardEvent(statArgs);
            }

            var matchEvent = new MatchEvent(statArgs.Location, statArgs.Player, _matchTimer.ElapsedMilliseconds,
                statArgs.EventType, statArgs.Team, _half);
            MatchEvents.Add(matchEvent);

            if (statArgs.EventType.IsTurnoverEvent())
            {
                _isHomeTeamInPossession = !_isHomeTeamInPossession;
            }

            if (statArgs.EventType == EventType.ThrowInWon)
            {
                if (statArgs.Team is not null && GetInPossessionTeam().TeamName != statArgs.Team.TeamName)
                {
                    _isHomeTeamInPossession = !_isHomeTeamInPossession;
                }
            }
        }

        private void AddBlackCardEvent(InputStatEventArgs eventArgs)
        {
            if (_blackCardedPlayers.ContainsKey(eventArgs.Player))
            {
                return; // No duplicates
            }

            // Player and when the card starts.
            _blackCardedPlayers.Add(eventArgs.Player, _matchTimer.ElapsedMilliseconds);
        }

        public void RefreshCardList()
        {
            // find finished timer
            foreach (var playerTimer in _blackCardedPlayers)
            {
                long timeSinceStartOfCard = _matchTimer.ElapsedMilliseconds - playerTimer.Value;
                if (timeSinceStartOfCard >= 600000)
                {
                    _blackCardedPlayers.Remove(playerTimer.Key);
                }
            }
        }

        private void AddEvent(ShotEventArgs shotArgs)
        {
            var matchEvent = new ShotEvent(shotArgs.Location, shotArgs.Player, _matchTimer.ElapsedMilliseconds,
                shotArgs.EventType, shotArgs.Team, _half, shotArgs.ActionType, shotArgs.ResultType);
            MatchEvents.Add(matchEvent);

            if (shotArgs.IsTurnedOver)
            {
                _isHomeTeamInPossession = !_isHomeTeamInPossession;
            }
        }

        private void AddEvent(KickOutEventArgs kickOutEventArgs)
        {
            var matchEvent = new KickOutEvent(kickOutEventArgs.Location, kickOutEventArgs.Player, _matchTimer.ElapsedMilliseconds,
                kickOutEventArgs.EventType, kickOutEventArgs.Team, _half, kickOutEventArgs.ResultType);
            MatchEvents.Add(matchEvent);

            if (!kickOutEventArgs.ResultType.IsKickOutWon())
            {
                _isHomeTeamInPossession = !_isHomeTeamInPossession;
            }
        }

        private void AddEvent(TurnoverEventArgs turnoverEventArgs)
        {
            var matchEvent = new TurnoverEvent(turnoverEventArgs.Location, turnoverEventArgs.Player,
                _matchTimer.ElapsedMilliseconds,
                turnoverEventArgs.EventType, turnoverEventArgs.Team, _half, turnoverEventArgs.TurnoverType);
            MatchEvents.Add(matchEvent);

            _isHomeTeamInPossession = !_isHomeTeamInPossession;
        }

        private void AddEvent(SubstitutionEventArgs substitutionEventArgs)
        {
            var matchEvent = new SubstitutionEvent(substitutionEventArgs.Player, substitutionEventArgs.SubstitutePlayer,
                _matchTimer.ElapsedMilliseconds, EventType.Substitution, substitutionEventArgs.Team,
                _half);
            MatchEvents.Add(matchEvent);

            substitutionEventArgs.Team.MakeSubstitution(substitutionEventArgs.SubstitutePlayer,
                substitutionEventArgs.Player);
        }

        public MatchEvent[] GetMatchEventsOfType<T>()
        {
            return MatchEvents.FindAll(me => me is T).ToArray();
        }

        public MatchEvent[] GetMatchEventsOfType(EventType eventType)
        {
            return MatchEvents.FindAll(me => me.Type == eventType).ToArray();
        }

        public MatchEvent[] GetKickOutEventsOfType(KickOutResultType resultType)
        {
            return MatchEvents.FindAll(me =>
            {
                if (me is KickOutEvent kickOutEvent)
                {
                    return kickOutEvent.ResultType == resultType;
                }

                return false;
            }).ToArray();
        }

        public MatchEvent[] GetTurnoverEventsOfType(TurnoverType turnoverType)
        {
            return MatchEvents.FindAll(me =>
            {
                if (me is TurnoverEvent turnoverEvent)
                {
                    return turnoverEvent.TurnoverType == turnoverType;
                }

                return false;
            }).ToArray();
        }

        public MatchEvent[] GetShotEventsOfType(ShotResultType resultType)
        {
            return MatchEvents.FindAll(me =>
            {
                if (me is ShotEvent shotEvent)
                {
                    return shotEvent.ResultType == resultType;
                }

                return false;
            }).ToArray();
        }

        public MatchEvent[] GetShotEventOfActionType(ActionType actionType)
        {
            return MatchEvents.FindAll(me =>
            {
                if (me is ShotEvent shotEvent)
                {
                    return shotEvent.ActionType == actionType;
                }

                return false;
            }).ToArray();
        }

        /// <summary>
        /// Gets all positive events for player. Positive events are scores, turnovers won and kick outs won.
        /// </summary>
        /// <param name="playerName">The name of the player the events should match.</param>
        public MatchEvent[] GetPositiveMatchEvents(string playerName)
        {
            List<MatchEvent> positiveEvents = new List<MatchEvent>();
            foreach (var matchEvent in MatchEvents)
            {
                if (matchEvent.Player != playerName)
                {
                    continue;
                }

                if (matchEvent is ShotEvent shotEvent)
                {
                    if (shotEvent.ResultType.IsScore())
                    {
                        positiveEvents.Add(matchEvent);
                    }
                }

                if (matchEvent is KickOutEvent kickOutEvent)
                {
                    if (kickOutEvent.ResultType.IsKickOutWon())
                    {
                        positiveEvents.Add(matchEvent);
                    }
                }

                if (matchEvent.Type == EventType.TurnoverWon)
                {
                    positiveEvents.Add(matchEvent);
                }
            }

            return positiveEvents.ToArray();
        }

        /// <summary>
        /// Gets all negative events for player. Negative events are misses, turnovers lost and frees conceded.
        /// </summary>
        /// <param name="playerName">The name of the player the events should match.</param>
        public MatchEvent[] GetNegativeMatchEvents(string playerName)
        {
            List<MatchEvent> negativeEvents = new List<MatchEvent>();
            foreach (var matchEvent in MatchEvents)
            {
                if (matchEvent.Player != playerName)
                {
                    continue;
                }

                if (matchEvent is ShotEvent shotEvent)
                {
                    if (!shotEvent.ResultType.IsScore())
                    {
                        negativeEvents.Add(matchEvent);
                    }
                }

                if (matchEvent.Type.IsNegativeEvent())
                {
                    negativeEvents.Add(matchEvent);
                }
            }

            return negativeEvents.ToArray();
        }
        #endregion

        #region Team Methods
        public Team GetTeamForEvent(EventType eventType)
        {
            if (eventType.IsInPossessionTeamEvent())
            {
                return GetInPossessionTeam();
            }

            return GetDefendingTeam();
        }

        public Team GetInPossessionTeam()
        {
            if (_isHomeTeamInPossession)
            {
                return HomeTeam;
            }

            return AwayTeam;
        }

        public Team GetDefendingTeam()
        {
            if (_isHomeTeamInPossession)
            {
                return AwayTeam;
            }

            return HomeTeam;
        }

        public string GetTeamAndSubsString()
        {
            string returnString = "";
            string[] team = HomeTeam.TeamSheet.ToArray();
            MatchEvent[] subs = MatchEvents.FindAll(me => me.Type == EventType.Substitution && me.TeamName == HomeTeam.TeamName).ToArray();
            MatchEvent[] scores = MatchEvents.FindAll(me => me.TeamName == HomeTeam.TeamName && me is ShotEvent).ToArray();

            for (int i = 0; i < team.Length; i++)
            {
                int playerNum = i + 1;
                returnString += playerNum + " " + team[i] + "\n";
            }

            returnString += "\n";

            foreach (var sub in subs)
            {
                SubstitutionEvent subEvent = (SubstitutionEvent)sub;
                returnString += "Sub Off: " + subEvent.Player + " : Sub On: " + subEvent.PlayerOnName + "\n";
            }

            returnString += "\n";
            returnString += "Scorers:\n";

            Dictionary<string, List<ShotEvent>> playerScores = new Dictionary<string, List<ShotEvent>>();
            foreach (var score in scores)
            {
                ShotEvent scoreEvent = (ShotEvent)score;
                if (scoreEvent.ResultType.IsScore())
                {
                    if (playerScores.ContainsKey(scoreEvent.Player))
                    {
                        playerScores[scoreEvent.Player].Add(scoreEvent);
                    }
                    else
                    {
                        playerScores.Add(scoreEvent.Player, [scoreEvent]);
                    }
                }
            }

            foreach (var playerScore in playerScores)
            {
                int frees = 0;
                int doublePoints = 0;
                int doublePointFree = 0;
                int goals = 0;
                int points = 0;

                foreach (var shot in playerScore.Value)
                {
                    if (shot.ResultType == ShotResultType.Goal)
                    {
                        goals += 1;
                    }
                    else if (shot.ResultType == ShotResultType.Point)
                    {
                        points += 1;

                        if (shot.ActionType == ActionType.Free)
                        {
                            frees += 1;
                        }
                    }
                    else if (shot.ResultType == ShotResultType.DoublePoint)
                    {
                        points += 2;

                        if (shot.ActionType == ActionType.Free)
                        {
                            doublePointFree += 1;
                        }
                        else
                        {
                            doublePoints += 1;
                        }
                    }
                }

                returnString += playerScore.Key + ": " + goals + "-" + points + " ";
                if (frees > 0)
                {
                    returnString += frees + "f ";
                }

                if (doublePointFree > 0)
                {
                    returnString += doublePointFree + "x2pf ";
                }

                if (doublePoints > 0)
                {
                    returnString += doublePoints + "x2p";
                }

                returnString += "\n";
            }

            return returnString;
        }
        #endregion
        #endregion
    }
}