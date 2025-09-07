using Microsoft.Maui.Controls.Shapes;

namespace QToken_Native.CustomView
{
    public partial class BorderedPicker : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty =
    BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<object>), typeof(BorderedPicker), default(IEnumerable<object>));

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(BorderedPicker), null, BindingMode.TwoWay);

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(BorderedPicker), default(string));

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(BorderedPicker), Colors.Gray);

        public static readonly BindableProperty FocusBorderColorProperty =
            BindableProperty.Create(nameof(FocusBorderColor), typeof(Color), typeof(BorderedPicker), Colors.DeepSkyBlue);

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create(nameof(BorderThickness), typeof(double), typeof(BorderedPicker), 1.0);

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(BorderedPicker), 8f);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BorderedPicker), Colors.Black);

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(BorderedPicker), Colors.Gray);
        public static readonly BindableProperty TitleColorProperty =
            BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(BorderedPicker), Colors.Gray);

        public IEnumerable<object> ItemsSource
        {
            get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public Color FocusBorderColor
        {
            get => (Color)GetValue(FocusBorderColorProperty);
            set => SetValue(FocusBorderColorProperty, value);
        }

        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }
        public Color TitleColor
        {
            get => (Color)GetValue(TitleColorProperty);
            set => SetValue(TitleColorProperty, value);
        }

        public BorderedPicker()
        {
            var border = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(CornerRadius) },
                Padding = new Thickness(8),
                Content = new Picker
                {
                    BackgroundColor = Colors.Transparent
                }
            };

            var picker = (Picker)border.Content; 
            picker.ItemDisplayBinding = new Binding("Name");


            // Bind border props
            border.SetBinding(Border.StrokeProperty, new Binding(nameof(BorderColor), source: this));
            border.SetBinding(Border.StrokeThicknessProperty, new Binding(nameof(BorderThickness), source: this));
            ((RoundRectangle)border.StrokeShape).SetBinding(RoundRectangle.CornerRadiusProperty, new Binding(nameof(CornerRadius), source: this));

            // Bind picker props
            picker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(ItemsSource), source: this));
            picker.SetBinding(Picker.SelectedItemProperty, new Binding(nameof(SelectedItem), source: this, mode: BindingMode.TwoWay));
            picker.SetBinding(Picker.TitleProperty, new Binding(nameof(Title), source: this));
            picker.SetBinding(Picker.TextColorProperty, new Binding(nameof(TextColor), source: this));
            picker.SetBinding(Picker.TitleColorProperty, new Binding(nameof(PlaceholderColor), source: this));
            picker.SetBinding(Picker.TitleColorProperty, new Binding(nameof(TitleColor), source: this));

            // Focus events to change border color
            picker.Focused += (s, e) =>
            {
                border.Stroke = FocusBorderColor;
            };

            picker.Unfocused += (s, e) =>
            {
                border.Stroke = BorderColor;
            };

            Content = border;
        }

    }
}
