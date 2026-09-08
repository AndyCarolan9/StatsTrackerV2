namespace StatsTrackerV2.Data.DrawItems
{
    public class DrawItem
    {
        public Color Color { get; set; }

        public DrawItem()
        {
            Color = Colors.Red;
        }

        public DrawItem(Color color)
        {
            Color = color;
        }

        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {

        }
    }
}
