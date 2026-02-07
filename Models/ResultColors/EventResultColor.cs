namespace StatsTrackerV2.Models.ResultColors
{
    public class EventResultColor
    {
        public EventType Type { get; set; }

        public string Name { get; set; } = string.Empty;

        public Color Color { get; set; } = new Color();

        public EventResultColor(EventType type, string name, Color color)
        {
            Type = type;
            Name = name;
            Color = color;
        }

        public EventResultColor(EventType type, Color color)
        {
            Type = type;
            Color = color;
        }
    }
}
