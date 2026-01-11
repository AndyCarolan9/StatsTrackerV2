namespace StatsTrackerV2.Data
{
    public class DotDrawable : IDrawable
    {
        public List<PointF> Dots { get; } = new();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.Crimson;

            foreach (var dot in Dots)
            {
                float width = dirtyRect.Width;
                float height = dirtyRect.Height;

                float x = width * dot.X;
                float y = height * dot.Y;

                canvas.FillCircle(x, y, 6);
            }
        }
    }
}
