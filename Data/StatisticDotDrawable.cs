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

                if (ColorsHelper.IsColorDark(stat.Color))
                {
                    canvas.FontColor = Colors.White;
                }
                else
                {
                    canvas.FontColor = Colors.Black;
                }

                canvas.FontSize = 10;

                if(stat.IsFirstHalf)
                {
                    canvas.FillCircle(x, y, 8);
                    canvas.DrawString(stat.Index.ToString(), x - 6, y - 8, 12, 12, HorizontalAlignment.Center, VerticalAlignment.Top);
                    canvas.DrawCircle(x, y, 8);
                }
                else
                {
                    canvas.FillRectangle(x - 7, y - 7, 14, 14);
                    canvas.DrawString(stat.Index.ToString(), x - 6, y - 8, 12, 12, HorizontalAlignment.Center, VerticalAlignment.Top);
                    canvas.DrawRectangle(x - 7, y - 7, 14, 14);
                }
                    
            }
        }
    }
}
