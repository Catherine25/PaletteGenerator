using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace PaletteGenerator
{
    public static class ColorController
    {
        public static bool AreClose(ColorData x, ColorData y)
        {
            bool hueNotVisible = false;

            if (x.S < 0.03 && y.S < 0.03 || x.L < 0.03 && y.L < 0.03)
                hueNotVisible = true;

            if (hueNotVisible || Math.Max(x.H, y.H) - Math.Min(x.H, y.H) < ColorData.StepH)
                if (Math.Max(x.L, y.L) - Math.Min(x.L, y.L) < ColorData.StepL)
                    if (Math.Max(x.S, y.S) - Math.Min(x.S, y.S) < ColorData.StepS)
                        return true;
            return false;
        }

        public static string TryCompareColors(string x, string y)
        {
            try
            {
                ColorData c1 = new ColorData(x);
                ColorData c2 = new ColorData(y);

                return AreClose(c1, c2) ? "Close" : "Not close";
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }
    }

    public static class ControlBuilder
    {
        public static Button CreateColorButton(Color color, string s)
        {
            string hex = BitConverter.ToString(new byte[] { color.R, color.G, color.B });

            return new Button
            {
                Background = new SolidColorBrush(color),
                Width = 75,
                FontSize = 10,
                Content = hex.Replace("-", string.Empty) + "\n" + s
            };
        }
    }
}
