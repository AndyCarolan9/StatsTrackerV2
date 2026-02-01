namespace StatsTrackerV2.Data
{
    public class StatisticDotDrawable : IDrawable
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

                if(stat.IsFirstHalf)
                {
                    canvas.FillCircle(x, y, 6);
                    canvas.DrawCircle(x, y, 6);
                }
                else
                {
                    canvas.FillRectangle(x - 6, y - 6, 12, 12);
                    canvas.DrawRectangle(x - 6, y - 6, 12, 12);
                }
                    
            }
        }
    }
}
