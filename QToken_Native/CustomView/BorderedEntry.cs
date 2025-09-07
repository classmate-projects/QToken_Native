using Microsoft.Maui.Controls.Shapes;

namespace QToken_Native.CustomView
{
    public partial class BorderedEntry : ContentView
    {
        public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(BorderedEntry), default(string), BindingMode.TwoWay);

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(BorderedEntry), default(string));

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(BorderedEntry), Colors.Gray);

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create(nameof(BorderThickness), typeof(double), typeof(BorderedEntry), 1.0);

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(BorderedEntry), 8f);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BorderedEntry), Colors.Black);

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(BorderedEntry), Colors.Gray);

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(BorderedEntry), false);

        public static readonly BindableProperty FocusBorderColorProperty =
            BindableProperty.Create(nameof(FocusBorderColor), typeof(Color), typeof(BorderedEntry), Colors.DeepSkyBlue);


        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
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
        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }
        public Color FocusBorderColor
        {
            get => (Color)GetValue(FocusBorderColorProperty);
            set => SetValue(FocusBorderColorProperty, value);
        }

        public BorderedEntry()
        {
            var border = new Border
            {
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(CornerRadius) },
                Padding = new Thickness(8),
                Content = new Entry
                {
                    BackgroundColor = Colors.Transparent
                }
            };

            // Bind border props
            border.SetBinding(Border.StrokeProperty, new Binding(nameof(BorderColor), source: this));
            border.SetBinding(Border.StrokeThicknessProperty, new Binding(nameof(BorderThickness), source: this));
            ((RoundRectangle)border.StrokeShape).SetBinding(RoundRectangle.CornerRadiusProperty, new Binding(nameof(CornerRadius), source: this));

            // Bind entry props
            var entry = (Entry)border.Content;
            entry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this, mode: BindingMode.TwoWay));
            entry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));
            entry.SetBinding(Entry.TextColorProperty, new Binding(nameof(TextColor), source: this));
            entry.SetBinding(Entry.PlaceholderColorProperty, new Binding(nameof(PlaceholderColor), source: this));
            entry.SetBinding(Entry.IsPasswordProperty, new Binding(nameof(IsPassword), source: this));

            entry.Focused += (s, e) =>
            {
                border.Stroke = FocusBorderColor;
            };

            entry.Unfocused += (s, e) =>
            {
                border.Stroke = BorderColor;
            };


            Content = border;
        }


    }
}
