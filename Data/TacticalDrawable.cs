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
                canvas.DrawLine(
                    line.X1,
                    line.Y1,
                    line.X2,
                    line.Y2);
            }
        }
    }
}
