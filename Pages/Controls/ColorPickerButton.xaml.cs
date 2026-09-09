using CommunityToolkit.Maui.Extensions;
using StatsTrackerV2.Pages.Popups;

namespace StatsTrackerV2.Pages.Controls
{
    public partial class ColorPickerButton : ContentView
    {
        public static readonly BindableProperty SelectedColorProperty =
            BindableProperty.Create(
                nameof(SelectedColor),
                typeof(Color),
                typeof(ColorPickerButton),
                Colors.Red,
                BindingMode.TwoWay);

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public ColorPickerButton()
        {
            InitializeComponent();
        }

        private async void ColourButton_Clicked(object sender, EventArgs e)
        {
            if (Shell.Current == null)
            {
                return;
            }

            var popup = new ColorPickerPopup(SelectedColor);
            await Shell.Current.ShowPopupAsync(popup);
            var result = await popup.Result;

            if (result is Color selectedColor)
            {
                SelectedColor = selectedColor;
            }
        }
    }
}