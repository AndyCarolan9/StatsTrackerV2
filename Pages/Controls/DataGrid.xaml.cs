using SkiaSharp;
using StatsTrackerV2.Models.EventScores;
using System.Collections.ObjectModel;

namespace StatsTrackerV2.Pages.Controls
{
    public partial class DataGrid : ContentView, IStatsControl
    {
        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(
                nameof(Items),
                typeof(ObservableCollection<EventScore>),
                typeof(DataGrid));

        public ObservableCollection<EventScore> Items
        {
            get => (ObservableCollection<EventScore>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public DataGrid()
        {
            InitializeComponent();
            dataGrid.BindingContext = this;
            dataGrid.SetBinding(CollectionView.ItemsSourceProperty, new Binding(nameof(Items), source: this));
        }

        public async void ExportControl(string fileName)
        {
            var rows = Items.ToArray();
            const int rowHeight = 50;
            const int width = 420;
            int height = 100 + ((rows.Count() + 1) * rowHeight);

            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);

            canvas.Clear(SKColor.Parse("#1E1E1E"));

            var titlePaint = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true
            };

            var textPaint = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true
            };

            var borderPaint = new SKPaint
            {
                Color = SKColors.Gray,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 2
            };

            var fillPaint = new SKPaint
            {
                Color = SKColor.Parse("#2A2A2A"),
                Style = SKPaintStyle.Fill
            };

            var titleBlob = SKTextBlob.Create(fileName.Replace("_", " "), new SKFont());
            canvas.DrawText(titleBlob, 20, 40, titlePaint);

            float y = 70;

            DrawRow(canvas,
                new[] { "Type", "1st Half", "2nd Half" },
                y,
                fillPaint,
                borderPaint,
                textPaint);

            y += rowHeight;

            foreach (var row in rows)
            {
                DrawRow(canvas,
                    new[] { row.Title, row.FirstHalfValue, row.SecondHalfValue },
                    y,
                    fillPaint,
                    borderPaint,
                    textPaint);

                y += rowHeight;
            }

            using var image = SKImage.FromBitmap(bitmap);
            if(image != null)
            {
                ExportHelper.ExportImage(fileName, image);
            }
        }

        private void DrawRow(SKCanvas canvas, string[] values, float y, SKPaint fill, SKPaint border, SKPaint text)
        {
            float[] widths = { 200, 100, 100 };

            float x = 10;

            for (int i = 0; i < values.Length; i++)
            {
                var rect = new SKRect(x, y, x + widths[i], y + 45);

                canvas.DrawRect(rect, fill);
                canvas.DrawRect(rect, border);

                canvas.DrawText(
                    SKTextBlob.Create(values[i], new SKFont()),
                    x + 10,
                    y + 30,
                    text);

                x += widths[i];
            }
        }
    }
}

