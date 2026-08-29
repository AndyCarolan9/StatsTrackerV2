namespace StatsTrackerV2.Models
{
    public class TacticalPlayerMarker
    {
        public int Number {  get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public bool IsHomeMarker { get; set; }

        public TacticalPlayerMarker() { }

        public TacticalPlayerMarker(int number, float x, float y, bool isHomeMarker = true)
        {
            Number = number;
            X = x;
            Y = y;
            IsHomeMarker = isHomeMarker;
        }
    }
}
