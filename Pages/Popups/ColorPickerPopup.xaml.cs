using CommunityToolkit.Maui.Views;

namespace StatsTrackerV2.Pages.Popups
{
    public partial class ColorPickerPopup : Popup
    {
        private readonly Color _originalColor;

        private readonly TaskCompletionSource<Color?> _result = new();

        public Task<Color?> Result => _result.Task;

        public ColorPickerPopup(Color initialColor)
        {
            InitializeComponent();
            _originalColor = initialColor;
            PickerControl.Color = initialColor;
            PickerControl.PreviousColor = initialColor;
        }
        private void OK_Clicked(object sender, EventArgs e)
        {
            _result.TrySetResult(PickerControl.Color);
            CloseAsync();
        }
        private void Cancel_Clicked(object sender, EventArgs e)
        {
            _result.TrySetResult(null);
            CloseAsync();
        }
        private void Close_Clicked(object sender, EventArgs e)
        {
            _result.TrySetResult(null);
            CloseAsync();
        }
    }
}