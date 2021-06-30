using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PaletteGenerator
{
    public partial class ColorView : UserControl
    {
        public ColorView()
        {
            InitializeComponent();

            Button.Background = new SolidColorBrush(Colors.Transparent);
            Button.Foreground = new SolidColorBrush(Colors.Black);
            Button.Width = 75;
            Button.FontSize = 10;

            Button.MouseUp += Button_Click;
            Image.MouseUp += Button_Click;
            ContentGrid.MouseEnter += ContentGrid_MouseEnter;
            ContentGrid.MouseLeave += ContentGrid_MouseLeave;

            Image.Visibility = Visibility.Hidden;
        }

        private void ContentGrid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) =>
            Image.Visibility = Visibility.Hidden;

        private void ContentGrid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) =>
            Image.Visibility = Visibility.Visible;

        private void Button_Click(object sender, RoutedEventArgs e) =>
            Clipboard.SetText((Button.Content as string).Substring(0, 6));

        public Color Color
        {
            set
            {
                (Button.Background as SolidColorBrush).Color = value;
                (Button.Foreground as SolidColorBrush).Color = value.Opposite();
            }
        }

        public string Text
        {
            set => Button.Content = value;
        }
    }
}
