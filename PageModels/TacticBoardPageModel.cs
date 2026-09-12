using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StatsTrackerV2.Data.Constants;
using StatsTrackerV2.Models;
using StatsTrackerV2.Models.TacticalLayouts;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.PageModels
{
    public partial class TacticBoardPageModel : ObservableObject
    {
        public ObservableCollection<TacticalPlayerMarker> TacticalPlayers { get; set; }

        [ObservableProperty]
        public partial string SelectedTool { get; set; } = "Select";

        [ObservableProperty]
        public partial Color SelectedDrawingColor { get; set; } = Colors.White;

        [ObservableProperty]
        public partial Color SelectedHomeColor { get; set; } = Colors.DarkGreen;

        [ObservableProperty]
        public partial Color SelectedAwayColor { get; set; } = Colors.Blue;

        [ObservableProperty]
        public partial List<TacticalLayout> TacticalLayouts { get; set; }

        [ObservableProperty]
        public partial TacticalLayout? SelectedLayout { get; set; }

        public TacticBoardPageModel()
        {
            TacticalLayouts = [];
            TacticalPlayers = new ObservableCollection<TacticalPlayerMarker>();
            LoadLayouts();
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

            TacticalPlayers.Add(new TacticalPlayerMarker(0, 0.5f, 0.5f, true, true));
        }

        [RelayCommand]
        private async Task SaveLayout()
        {
            string layoutName = await Shell.Current.DisplayPromptAsync("Save Layout", "Enter the name of the layout");
            if (string.IsNullOrEmpty(layoutName))
            {
                return;
            }

            if (TacticalLayouts.Find(tacLayout => tacLayout.Name == layoutName) != null)
            {
                // Exists already
                await AppShell.DisplayToastAsync("Cannot save layout as layout already exists");
                return;
            }

            TacticalLayouts.Add(new TacticalLayout(layoutName, TacticalPlayers.ToArray()));
            JSONHelper.SaveToJsonFile(JSONConstants.TacticalLayoutsJSONPath, TacticalLayouts);
        }

        [RelayCommand]
        private async Task ApplyLayout()
        {
            if (SelectedLayout is null)
                return;

            TacticalPlayers.Clear();
            foreach(var marker in SelectedLayout.TacticalMarkers)
            {
                TacticalPlayers.Add(marker);
            }
        }

        private void LoadLayouts()
        {
            List<TacticalLayout>? layouts = JSONHelper.LoadFromJsonFile<List<TacticalLayout>>(JSONConstants.TacticalLayoutsJSONPath);
            if(layouts == null)
            {
                TacticalLayouts = [];
                return;
            }

            TacticalLayouts = layouts;
        }
    }
}
