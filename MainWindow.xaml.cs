using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PaletteGenerator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeComponent();
            OkBt.Click += OkBt_Click;
            ColorTb.KeyDown += ColorTb_KeyDown;
            Color1Tb.TextChanged += (object sender, TextChangedEventArgs e) => TryShowColor(Color1Tb);
            Color2Tb.TextChanged += (object sender, TextChangedEventArgs e) => TryShowColor(Color2Tb);
        }

        private void ColorTb_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                OkBt_Click(null, null);
        }

        private void TryShowColor(TextBox box)
        {
            try
            {
                ColorData color = new ColorData(box.Text);
                box.Background = new SolidColorBrush(color.Color);
            }
            catch (Exception)
            {
                box.Background = new SolidColorBrush(Colors.Transparent);
            }

            AreSameTb.Text = ColorController.TryCompareColors(Color1Tb.Text, Color2Tb.Text);
        }

        private void OkBt_Click(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();

            LinkedList<(Color, string)> colors = new LinkedList<(Color, string)>();

            var primary = new ColorData(ColorTb.Text);
            colors.AddFirst((primary.Color, primary.ToString()));

            // add lighter colors
            var lighter = new ColorData(ColorTb.Text);
            while (lighter.L < 0.9)
            {
                lighter.MakeLighter(true);
                colors.AddFirst((lighter.Color, lighter.ToString()));
            }

            // add darker colors
            var darker = new ColorData(ColorTb.Text);
            while (darker.L > 0.1)
            {
                darker.MakeLighter(false);
                colors.AddLast((darker.Color, darker.ToString()));
            }

            foreach ((Color, string) item in colors)
            {
                Button button = ControlBuilder.CreateColorButton(item.Item1, item.Item2);
                button.Click += Button_Click;
                Panel.Children.Add(button);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) =>
            Clipboard.SetText(((sender as Button).Content as string).Substring(0, 6));
    }
}
