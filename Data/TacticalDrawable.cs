namespace StatsTrackerV2.Data
{
    public class TacticalDrawable : IDrawable
    {
        public List<DrawLine> Lines { get; } = new();
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 4;

            foreach (var line in Lines)
            {
                float x1 = line.X1 * dirtyRect.Width;
                float y1 = line.Y1 * dirtyRect.Height;

                float x2 = line.X2 * dirtyRect.Width;
                float y2 = line.Y2 * dirtyRect.Height;

                canvas.DrawLine(x1, y1, x2, y2);
            }
        }
    }
}
