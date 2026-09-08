using Microsoft.Maui.Controls.Shapes;

namespace StatsTrackerV2.Data.DrawItems
{
    public class DrawLine : DrawItem
    {
        public PointF Start { get; set; }

        public PointF End { get; set; }

        public DrawLine() { }

        public DrawLine(Color color, PointF start, PointF end) : base(color)
        {
            Start = start;
            End = end;
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float x1 = Start.X * dirtyRect.Width;
            float y1 = Start.Y * dirtyRect.Height;

            float x2 = End.X * dirtyRect.Width;
            float y2 = End.Y * dirtyRect.Height;

            canvas.StrokeColor = Color;
            canvas.DrawLine(x1, y1, x2, y2);
        }
    }
}
