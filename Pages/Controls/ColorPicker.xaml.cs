namespace StatsTrackerV2.Pages.Controls
{
    public partial class ColorPicker : ContentView
    {
        private float _hue;
        private float _saturation;
        private float _value;
        private float _alpha;

        private bool _updating;

        private Color _previousColor = Colors.Red;

        public static readonly BindableProperty ColorProperty =
            BindableProperty.Create(
                nameof(Color),
                typeof(Color),
                typeof(ColorPicker),
                Colors.Red,
                BindingMode.TwoWay,
                propertyChanged: OnColorChanged);

        public static readonly BindableProperty PreviousColorProperty =
            BindableProperty.Create(
                nameof(PreviousColor),
                typeof(Color),
                typeof(ColorPicker),
                Colors.Red);

        public static readonly BindableProperty HexValueProperty =
            BindableProperty.Create(
                nameof(HexValue),
                typeof(string),
                typeof(ColorPicker),
                "#FF0000",
                BindingMode.TwoWay);

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public Color PreviousColor
        {
            get => (Color)GetValue(PreviousColorProperty);
            set => SetValue(PreviousColorProperty, value);
        }

        public string HexValue
        {
            get => (string)GetValue(HexValueProperty);
            private set => SetValue(HexValueProperty, value);
        }

        public ColorFieldDrawable ColorFieldDrawable { get; }

        public HueDrawable HueDrawable { get; }

        public AlphaDrawable AlphaDrawable { get; }

        public ColorPicker()
        {
            InitializeComponent();

            ColorFieldDrawable = new ColorFieldDrawable(this);
            HueDrawable = new HueDrawable(this);
            AlphaDrawable = new AlphaDrawable(this);

            ColorField.Drawable = ColorFieldDrawable;
            HueField.Drawable = HueDrawable;
            AlphaField.Drawable = AlphaDrawable;

            UpdateFromColor(Color);
        }

        private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not ColorPicker picker)
                return;

            if (picker._updating)
                return;

            if (oldValue is Color oldColor)
            {
                picker.PreviousColor = oldColor;
            }

            if (newValue is Color newColor)
            {
                picker.UpdateFromColor(newColor);
            }
        }

        private void UpdateFromColor(Color color)
        {
            _updating = true;

            RgbToHsv(color.Red, color.Green, color.Blue, out _hue, out _saturation, out _value);

            _alpha = color.Alpha;

            HexValue = ColorToHex(color);

            _updating = false;

            InvalidateCanvases();
        }

        private void SetColorFromHsv()
        {
            var color = HsvToColor(_hue, _saturation, _value, _alpha);

            SetColorInternal(color);
        }

        private void SetColorInternal(Color color)
        {
            if (_updating)
                return;

            _updating = true;

            Color = color;
            HexValue = ColorToHex(color);

            _updating = false;

            InvalidateCanvases();
        }

        private void InvalidateCanvases()
        {
            ColorField?.Invalidate();
            HueField?.Invalidate();
            AlphaField?.Invalidate();
        }

        // ------------------------------------------------------------
        // Saturation / brightness
        // ------------------------------------------------------------

        private void SetColorFieldPosition(PointF point)
        {
            float width = (float)ColorField.Width;
            float height = (float)ColorField.Height;

            if (width <= 0 || height <= 0)
                return;

            _saturation = Math.Clamp(point.X / width, 0f, 1f);

            // Top = 1, bottom = 0
            _value = Math.Clamp(1f - (point.Y / height), 0f, 1f);

            SetColorFromHsv();
        }

        private void ColorField_StartInteraction(object sender, TouchEventArgs e)
        {
            if (e.Touches.Length == 0)
                return;

            SetColorFieldPosition(e.Touches[0]);
        }

        private void ColorField_DragInteraction(object sender, TouchEventArgs e)
        {
            if (e.Touches.Length == 0)
                return;

            SetColorFieldPosition(e.Touches[0]);
        }

        private void ColorField_EndInteraction(object sender, TouchEventArgs e)
        {
        }

        // ------------------------------------------------------------
        // Hue
        // ------------------------------------------------------------

        private void SetHuePosition(PointF point)
        {
            float height = (float)HueField.Height;

            if (height <= 0)
                return;

            float position = Math.Clamp(point.Y / height, 0f, 1f);

            _hue = position * 360f;

            SetColorFromHsv();
        }

        private void Hue_StartInteraction(object sender, TouchEventArgs e)
        {
            if (e.Touches.Length == 0)
                return;

            SetHuePosition(e.Touches[0]);
        }

        private void Hue_DragInteraction(object sender, TouchEventArgs e)
        {
            if (e.Touches.Length == 0)
                return;

            SetHuePosition(e.Touches[0]);
        }

        private void Hue_EndInteraction(object sender, TouchEventArgs e)
        {
        }

        // ------------------------------------------------------------
        // Alpha
        // ------------------------------------------------------------

        private void SetAlphaPosition(PointF point)
        {
            float width = (float)AlphaField.Width;

            if (width <= 0)
                return;

            _alpha = Math.Clamp(point.X / width, 0f, 1f);

            SetColorFromHsv();
        }

        private void Alpha_StartInteraction(object sender, TouchEventArgs e)
        {
            if (e.Touches.Length == 0)
                return;

            SetAlphaPosition(e.Touches[0]);
        }

        private void Alpha_DragInteraction(object sender, TouchEventArgs e)
        {
            if (e.Touches.Length == 0)
                return;

            SetAlphaPosition(e.Touches[0]);
        }

        private void Alpha_EndInteraction(object sender, TouchEventArgs e)
        {
        }

        // ------------------------------------------------------------
        // Hex
        // ------------------------------------------------------------

        private void HexEntry_Completed(object sender, EventArgs e)
        {
            UpdateFromHex();
        }

        private void UpdateFromHex()
        {
            string value = HexEntry.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(value))
                return;

            if (value.StartsWith("#"))
                value = value[1..];

            if (value.Length != 6 && value.Length != 8)
            {
                HexValue = ColorToHex(Color);
                return;
            }

            if (!uint.TryParse(
                    value,
                    System.Globalization.NumberStyles.HexNumber,
                    null,
                    out uint number))
            {
                HexValue = ColorToHex(Color);
                return;
            }

            float r;
            float g;
            float b;
            float a = 1f;

            if (value.Length == 6)
            {
                r = ((number >> 16) & 0xFF) / 255f;
                g = ((number >> 8) & 0xFF) / 255f;
                b = (number & 0xFF) / 255f;
            }
            else
            {
                r = ((number >> 24) & 0xFF) / 255f;
                g = ((number >> 16) & 0xFF) / 255f;
                b = ((number >> 8) & 0xFF) / 255f;
                a = (number & 0xFF) / 255f;
            }

            SetColorInternal(new Color(r, g, b, a));

            UpdateFromColor(Color);
        }

        // ------------------------------------------------------------
        // Reset
        // ------------------------------------------------------------

        private void Reset_Clicked(object sender, EventArgs e)
        {
            SetColorInternal(Colors.Red);
            UpdateFromColor(Colors.Red);
        }

        // ------------------------------------------------------------
        // HSV conversion
        // ------------------------------------------------------------

        private static void RgbToHsv(float r, float g, float b, out float h, out float s, out float v)
        {
            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));

            float delta = max - min;

            v = max;

            if (max == 0)
            {
                s = 0;
            }
            else
            {
                s = delta / max;
            }

            if (delta == 0)
            {
                h = 0;
                return;
            }

            if (max == r)
            {
                h = 60f * (((g - b) / delta) % 6f);
            }
            else if (max == g)
            {
                h = 60f * (((b - r) / delta) + 2f);
            }
            else
            {
                h = 60f * (((r - g) / delta) + 4f);
            }

            if (h < 0)
                h += 360f;
        }

        public static Color HsvToColor(float h, float s, float v, float a = 1f)
        {
            h = ((h % 360f) + 360f) % 360f;

            float c = v * s;
            float x = c * (1f - Math.Abs((h / 60f % 2f) - 1f));

            float m = v - c;

            float r;
            float g;
            float b;

            if (h < 60)
            {
                r = c;
                g = x;
                b = 0;
            }
            else if (h < 120)
            {
                r = x;
                g = c;
                b = 0;
            }
            else if (h < 180)
            {
                r = 0;
                g = c;
                b = x;
            }
            else if (h < 240)
            {
                r = 0;
                g = x;
                b = c;
            }
            else if (h < 300)
            {
                r = x;
                g = 0;
                b = c;
            }
            else
            {
                r = c;
                g = 0;
                b = x;
            }

            return new Color(
                r + m,
                g + m,
                b + m,
                a);
        }

        private static string ColorToHex(Color color)
        {
            int r = (int)Math.Round(color.Red * 255);
            int g = (int)Math.Round(color.Green * 255);
            int b = (int)Math.Round(color.Blue * 255);

            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}