namespace StatsTrackerV2.Pages.Controls
{
    public partial class ToolOption : ContentView
    {
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(
                nameof(Value),
                typeof(object),
                typeof(ToolOption));

        public object? Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(
                nameof(Icon),
                typeof(ImageSource),
                typeof(ToolOption));

        public ImageSource? Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly BindableProperty IsSelectedProperty =
            BindableProperty.Create(
                nameof(IsSelected),
                typeof(bool),
                typeof(ToolOption),
                false,
                propertyChanged: OnIsSelectedChanged);

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public event EventHandler? Clicked;

        public ToolOption()
        {
            InitializeComponent();

            UpdateVisualState();
        }

        private static void OnIsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ToolOption)bindable;

            control.UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            if (ToolBorder == null)
                return;

            VisualStateManager.GoToState(ToolBorder, IsSelected ? "Selected" : "Normal");
        }

        private void OnTapped(object? sender, TappedEventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}