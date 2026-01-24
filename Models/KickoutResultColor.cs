namespace StatsTrackerV2.Models
{
    public class KickoutResultColor
    {
        public KickOutResultType Type { get; set; }

        public string Name { get; set; } = string.Empty;

        public Color Color { get; set; } = new Color();

        public KickoutResultColor(KickOutResultType type, Color color)
        {
            Type = type;
            Color = color;
        }
    }
}
