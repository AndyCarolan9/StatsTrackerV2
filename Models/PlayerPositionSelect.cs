using StatsTrackerV2.Data.Arguments;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StatsTrackerV2.Models
{
    public class PlayerPositionSelect : INotifyPropertyChanged
    {
        public int Number { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }

        private string _selectedPlayer;

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<PlayerSelectedEventArgs>? PlayerSelected;

        public string SelectedPlayer
        {
            get => _selectedPlayer;
            set
            {
                if (_selectedPlayer != value)
                {
                    _selectedPlayer = value;
                    OnPropertyChanged();
                    PlayerSelected?.Invoke(this, new PlayerSelectedEventArgs(value));
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public PlayerPositionSelect(int number, int row, int column)
        {
            Number = number; 
            Row = row; 
            Column = column;
            _selectedPlayer = "";
        }
    }
}
