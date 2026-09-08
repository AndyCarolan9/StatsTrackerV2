using StatsTrackerV2.Data.DrawItems;

namespace StatsTrackerV2.Data
{
    public class TacticalDrawable : IDrawable
    {
        public List<DrawItem> DrawItems { get; } = new();
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 4;

            foreach (var drawItem in DrawItems)
            {
                drawItem.Draw(canvas, dirtyRect);
            }
        }
    }
}
