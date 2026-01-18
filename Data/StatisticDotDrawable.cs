namespace StatsTrackerV2.Data
{
    public class StatisticDotDrawable : DotDrawable
    {
        public List<DrawableStatistic> Statistics { get; } = new();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            foreach(DrawableStatistic stat in Statistics)
            {
                float width = dirtyRect.Width;
                float height = dirtyRect.Height;

                float x = width * stat.Location.X;
                float y = height * stat.Location.Y;

                canvas.FillColor = stat.Color;
                canvas.FillCircle(x, y, 6);
            }
        }
    }
}
