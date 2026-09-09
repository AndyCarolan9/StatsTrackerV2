using StatsTrackerV2.Pages.Controls;

namespace StatsTrackerV2.Data
{
    // ============================================================
    // Saturation / Brightness square
    // ============================================================

    public class ColorFieldDrawable : IDrawable
    {
        private readonly ColorPicker _picker;

        public ColorFieldDrawable(ColorPicker picker)
        {
            _picker = picker;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (dirtyRect.Width <= 0 || dirtyRect.Height <= 0)
                return;

            float hue = GetHue();

            Color pureHue = ColorPicker.HsvToColor(hue, 1f, 1f);

            // ----------------------------------------------------
            // Base colour = fully saturated hue
            // ----------------------------------------------------

            canvas.FillColor = pureHue;
            canvas.FillRectangle(dirtyRect);

            // ----------------------------------------------------
            // White -> transparent horizontal gradient
            // Left = white
            // Right = transparent
            // ----------------------------------------------------

            var whiteGradient = new LinearGradientPaint(
                new PaintGradientStop[]
                {
                new PaintGradientStop(0f, Colors.White),
                new PaintGradientStop(1f, new Color(1f, 1f, 1f, 0f))
                },
                new Point(0, 0),
                new Point(1, 0));

            canvas.SetFillPaint(whiteGradient, dirtyRect);

            canvas.FillRectangle(dirtyRect);

            // ----------------------------------------------------
            // Transparent -> black vertical gradient
            // Top = transparent
            // Bottom = black
            // ----------------------------------------------------

            var blackGradient = new LinearGradientPaint(
                new PaintGradientStop[]
                {
                new PaintGradientStop(0f, new Color(0f, 0f, 0f, 0f)),
                new PaintGradientStop(1f, new Color(0f, 0f, 0f, 1f))
                },
                new Point(0, 0),
                new Point(0, 1));

            canvas.SetFillPaint(
                blackGradient,
                dirtyRect);

            canvas.FillRectangle(dirtyRect);

            // ----------------------------------------------------
            // Selection indicator
            // ----------------------------------------------------

            float saturation = GetSaturation();
            float value = GetValue();

            float x = saturation * dirtyRect.Width;

            float y = (1f - value) * dirtyRect.Height;

            const float radius = 8f;

            // Outer black ring
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 3;

            canvas.DrawCircle(new PointF(x, y), radius);

            // Inner white ring
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 1.5f;

            canvas.DrawCircle(new PointF(x, y), radius);
        }

        private float GetHue()
        {
            return GetField<float>("_hue");
        }

        private float GetSaturation()
        {
            return GetField<float>("_saturation");
        }

        private float GetValue()
        {
            return GetField<float>("_value");
        }

        private T GetField<T>(string name)
        {
            var field = typeof(ColorPicker).GetField(
                            name,
                            System.Reflection.BindingFlags.Instance |
                            System.Reflection.BindingFlags.NonPublic);

            if (field == null)
                return default!;

            return (T)field.GetValue(_picker)!;
        }
    }


    // ============================================================
    // Hue slider
    // ============================================================

    public class HueDrawable : IDrawable
    {
        private readonly ColorPicker _picker;

        public HueDrawable(ColorPicker picker)
        {
            _picker = picker;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (dirtyRect.Width <= 0 || dirtyRect.Height <= 0)
                return;

            var hueGradient = new LinearGradientPaint(
                new PaintGradientStop[]
                {
                new PaintGradientStop(0f, ColorPicker.HsvToColor(0, 1, 1)),
                new PaintGradientStop(1f / 6f, ColorPicker.HsvToColor(60, 1, 1)),
                new PaintGradientStop(2f / 6f, ColorPicker.HsvToColor(120, 1, 1)),
                new PaintGradientStop(3f / 6f, ColorPicker.HsvToColor(180, 1, 1)),
                new PaintGradientStop(4f / 6f, ColorPicker.HsvToColor(240, 1, 1)),
                new PaintGradientStop(5f / 6f, ColorPicker.HsvToColor(300, 1, 1)),
                new PaintGradientStop(1f, ColorPicker.HsvToColor(360, 1, 1))
                },
                new Point(0, 0),
                new Point(0, 1));

            canvas.SetFillPaint(hueGradient, dirtyRect);

            canvas.FillRectangle(dirtyRect);

            // ----------------------------------------------------
            // Selection indicator
            // ----------------------------------------------------

            float hue = GetHue();

            float y = (hue / 360f) * dirtyRect.Height;

            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 3;

            canvas.DrawLine(0, y, dirtyRect.Width, y);

            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 1;

            canvas.DrawLine(0, y - 2, dirtyRect.Width, y - 2);

            canvas.DrawLine(0, y + 2, dirtyRect.Width, y + 2);
        }

        private float GetHue()
        {
            var field =
                typeof(ColorPicker).GetField(
                    "_hue",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);

            return field == null ? 0 : (float)field.GetValue(_picker)!;
        }
    }


    // ============================================================
    // Alpha slider
    // ============================================================

    public class AlphaDrawable : IDrawable
    {
        private readonly ColorPicker _picker;

        public AlphaDrawable(ColorPicker picker)
        {
            _picker = picker;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (dirtyRect.Width <= 0 || dirtyRect.Height <= 0)
                return;

            DrawCheckerboard(canvas, dirtyRect);

            var color = _picker.Color;

            var alphaGradient = new LinearGradientPaint(
                new PaintGradientStop[]
                {
                new PaintGradientStop(0f, new Color(color.Red, color.Green, color.Blue, 0)),
                new PaintGradientStop(1f, new Color(color.Red, color.Green, color.Blue, 1))
                },
                new Point(0, 0),
                new Point(1, 0));

            canvas.SetFillPaint(alphaGradient, dirtyRect);

            canvas.FillRectangle(dirtyRect);

            // ----------------------------------------------------
            // Selection indicator
            // ----------------------------------------------------

            float alpha = GetAlpha();

            float x = alpha * dirtyRect.Width;

            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 3;

            canvas.DrawLine(x, 0, x, dirtyRect.Height);

            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 1;

            canvas.DrawLine(x - 2, 0, x - 2, dirtyRect.Height);

            canvas.DrawLine(x + 2, 0, x + 2, dirtyRect.Height);
        }

        private static void DrawCheckerboard(ICanvas canvas, RectF rect)
        {
            const float size = 8;

            for (float y = 0; y < rect.Height; y += size)
            {
                for (float x = 0; x < rect.Width; x += size)
                {
                    bool light = ((int)(x / size) + (int)(y / size)) % 2 == 0;

                    canvas.FillColor = light ? Colors.White : new Color(0.75f, 0.75f, 0.75f);

                    canvas.FillRectangle(x, y, size, size);
                }
            }
        }

        private float GetAlpha()
        {
            var field =
                typeof(ColorPicker).GetField(
                    "_alpha",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);

            return field == null ? 1 : (float)field.GetValue(_picker)!;
        }
    }
}