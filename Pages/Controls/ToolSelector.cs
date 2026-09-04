namespace StatsTrackerV2.Pages.Controls
{
    public class ToolSelector : HorizontalStackLayout
    {
        public ToolSelector()
        {
            Spacing = 4;

            Padding = 5;

            BackgroundColor = Colors.Transparent;

            // Allow the selector to size itself around its children
            HorizontalOptions = LayoutOptions.Start;
            VerticalOptions = LayoutOptions.Start;
        }

        public static readonly BindableProperty SelectedToolProperty =
            BindableProperty.Create(
                nameof(SelectedTool),
                typeof(object),
                typeof(ToolSelector),
                null,
                BindingMode.TwoWay,
                propertyChanged: OnSelectedToolChanged);

        public object? SelectedTool
        {
            get => GetValue(SelectedToolProperty);
            set => SetValue(SelectedToolProperty, value);
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child is ToolOption option)
            {
                option.Clicked -= OnOptionClicked;
                option.Clicked += OnOptionClicked;

                option.IsSelected =
                    Equals(option.Value, SelectedTool);
            }
        }

        protected override void OnChildRemoved(Element child, int oldLogicalIndex)
        {
            if (child is ToolOption option)
            {
                option.Clicked -= OnOptionClicked;
            }

            base.OnChildRemoved(child, oldLogicalIndex);
        }

        private static void OnSelectedToolChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var selector = (ToolSelector)bindable;
            selector.UpdateSelection();
        }

        private void OnOptionClicked(object? sender, EventArgs e)
        {
            if (sender is not ToolOption option)
                return;

            SelectedTool = option.Value;
            UpdateSelection();
        }

        private void UpdateSelection()
        {
            foreach (var child in Children)
            {
                if (child is ToolOption option)
                {
                    option.IsSelected =
                        Equals(option.Value, SelectedTool);
                }
            }
        }
    }
}
