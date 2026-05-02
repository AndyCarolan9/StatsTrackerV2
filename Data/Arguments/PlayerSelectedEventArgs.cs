using System;
using System.Collections.Generic;
using System.Text;

namespace StatsTrackerV2.Data.Arguments
{
    public class PlayerSelectedEventArgs : EventArgs
    {
        private readonly string _playerName;

        public PlayerSelectedEventArgs(string playerName)
        {
            _playerName = playerName;
        }

        public string PlayerName
        {
            get => _playerName;
        }
    }
}
