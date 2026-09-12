namespace StatsTrackerV2.Models.TacticalLayouts
{
    public class TacticalLayout
    {
        public string Name { get; set; }

        public TacticalPlayerMarker[] TacticalMarkers { get; set; }

        public TacticalLayout()
        {
            Name = "";
            TacticalMarkers = [];
        }

        public TacticalLayout(string name, TacticalPlayerMarker[] tacticalMarkers)
        {
            Name = name;
            TacticalMarkers = tacticalMarkers;
        }
    }
}
