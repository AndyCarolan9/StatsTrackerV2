namespace StatsTrackerV2.Data.DrawItems
{
    public class DrawArrow : DrawLine
    {
        public DrawArrow(Color color, PointF start, PointF end) : base(color, start, end) 
        {

        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float x1 = Start.X * dirtyRect.Width;
            float y1 = Start.Y * dirtyRect.Height;

            float x2 = End.X * dirtyRect.Width;
            float y2 = End.Y * dirtyRect.Height;

            // Direction from Start to End
            float dx = x2 - x1;
            float dy = y2 - y1;

            // Length of the line
            float length = MathF.Sqrt(dx * dx + dy * dy);

            if (length == 0)
                return;

            // Normalised direction vector
            float dirX = dx / length;
            float dirY = dy / length;

            // Perpendicular vector
            float perpX = -dirY;
            float perpY = dirX;

            // Arrow dimensions
            float arrowLength = 15;
            float arrowWidth = 8;

            // Centre point of the triangle's base
            float baseX = x2 - dirX * arrowLength;
            float baseY = y2 - dirY * arrowLength;

            // Left corner of triangle base
            float leftX = baseX + perpX * arrowWidth;
            float leftY = baseY + perpY * arrowWidth;

            // Right corner of triangle base
            float rightX = baseX - perpX * arrowWidth;
            float rightY = baseY - perpY * arrowWidth;

            // Draw the line
            canvas.StrokeColor = Color;
            canvas.StrokeSize = 2;

            canvas.DrawLine(x1, y1, baseX, baseY);

            // Draw arrow head
            PathF arrowHead = new PathF();

            arrowHead.MoveTo(x2, y2);           // Tip
            arrowHead.LineTo(leftX, leftY);     // Left base
            arrowHead.LineTo(rightX, rightY);   // Right base
            arrowHead.Close();

            canvas.FillColor = Color;
            canvas.FillPath(arrowHead);
        }
    }
}
