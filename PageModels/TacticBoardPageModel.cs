using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Models;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class TacticBoardPageModel : ObservableObject
    {
        public ObservableCollection<TacticalPlayerMarker> TacticalPlayers { get; }

        public TacticBoardPageModel()
        {
            TacticalPlayers = new ObservableCollection<TacticalPlayerMarker>();
        }

        [RelayCommand]
        private async Task PopulateStartingLayout()
        {
            // These layouts should be loaded from JSON
            // Home Players
            TacticalPlayers.Add(new TacticalPlayerMarker(1, 0.05f, 0.5f));
            TacticalPlayers.Add(new TacticalPlayerMarker(2, 0.15f, 0.17f));
            TacticalPlayers.Add(new TacticalPlayerMarker(3, 0.15f, 0.48f));
            TacticalPlayers.Add(new TacticalPlayerMarker(4, 0.15f, 0.83f));
            TacticalPlayers.Add(new TacticalPlayerMarker(5, 0.3f, 0.17f));
            TacticalPlayers.Add(new TacticalPlayerMarker(6, 0.3f, 0.48f));
            TacticalPlayers.Add(new TacticalPlayerMarker(7, 0.3f, 0.83f));
            TacticalPlayers.Add(new TacticalPlayerMarker(8, 0.48f, 0.5f));
            TacticalPlayers.Add(new TacticalPlayerMarker(9, 0.5f, 0.98f));
            TacticalPlayers.Add(new TacticalPlayerMarker(10, 0.7f, 0.13f));
            TacticalPlayers.Add(new TacticalPlayerMarker(11, 0.7f, 0.52f));
            TacticalPlayers.Add(new TacticalPlayerMarker(12, 0.7f, 0.87f));
            TacticalPlayers.Add(new TacticalPlayerMarker(13, 0.85f, 0.13f));
            TacticalPlayers.Add(new TacticalPlayerMarker(14, 0.85f, 0.52f));
            TacticalPlayers.Add(new TacticalPlayerMarker(15, 0.85f, 0.87f));

            // Away Players
            TacticalPlayers.Add(new TacticalPlayerMarker(1, 0.95f, 0.5f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(2, 0.85f, 0.17f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(3, 0.85f, 0.48f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(4, 0.85f, 0.83f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(5, 0.7f, 0.17f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(6, 0.7f, 0.48f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(7, 0.7f, 0.83f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(8, 0.52f, 0.5f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(9, 0.5f, 0.02f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(10, 0.3f, 0.13f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(11, 0.3f, 0.52f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(12, 0.3f, 0.87f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(13, 0.15f, 0.13f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(14, 0.15f, 0.52f, false));
            TacticalPlayers.Add(new TacticalPlayerMarker(15, 0.15f, 0.87f, false));
        }
    }
}
