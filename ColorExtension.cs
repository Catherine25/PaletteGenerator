using System.Windows.Media;

namespace PaletteGenerator
{
    public static class ColorExtension
    {
        public static Color Opposite(this Color color) =>
            Color.FromArgb(255, (byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));
    }
}
