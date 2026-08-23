namespace StatsTrackerV2.Data
{
    public class DrawLine
    {
        public DrawLine() { }

        public DrawLine(PointF start, PointF end)
        {
            X1 = start.X;
            Y1 = start.Y;
            X2 = end.X;
            Y2 = end.Y;
        }

        public float X1 { get; set; }
        public float Y1 { get; set; }

        public float X2 { get; set; }
        public float Y2 { get; set; }
    }
}
