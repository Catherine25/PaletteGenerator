using System;
using System.Windows.Media;

namespace PaletteGenerator
{
    public class ColorData
    {
        public ColorData(string hex)
        {
            hex = hex.Replace("#", "");
            SetRGB(Convert.ToByte(hex.Substring(0, 2), 16),
                Convert.ToByte(hex.Substring(2, 2), 16),
                Convert.ToByte(hex.Substring(4, 2), 16));
        }

        public Color Color { get; private set; }

        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }

        public double H { get; private set; }
        public double L { get; private set; }
        public double S { get; private set; }

        public const double StepH = 10;
        public const double StepL = 0.1;
        public const double StepS = 0.1;

        public void SetRGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;

            RgbToHls(r, g, b, out double h, out double l, out double s);

            H = h;
            L = l;
            S = s;

            Color = Color.FromRgb(r, g, b);
        }

        public void SetHSL(double h, double l, double s)
        {
            H = h;
            L = l;
            S = s;

            HlsToRgb(h, l, s, out int r, out int g, out int b);

            R = (byte)r;
            G = (byte)g;
            B = (byte)b;

            Color = Color.FromRgb(R, G, B);
        }

        public void MakeLighter(bool lighter)
        {
            if (lighter)
            {
                //yellower => 60
                double h = MoveToNumber(value: H, needed: 60, StepH);
                double l = MoveToNumber(value: L, needed: 1, StepL);
                double s = MoveToNumber(value: S, needed: 0, StepS);

                SetHSL(h, l, s);
            }
            else
            {
                //bluer => 240
                double h = MoveToNumber(value: H, needed: 240, StepH);
                double l = MoveToNumber(value: L, needed: 0, StepL);
                double s = MoveToNumber(value: S, needed: 1, StepS);

                SetHSL(h, l, s);
            }
        }

        public override string ToString() => $"\n{(int)R} {(int)G} {(int)B}\n\n{(int)H} {(int)S} {(int)L}";

        private double MoveToNumber(double value, double needed, double step)
        {
            // copy value
            if (value == needed)
                return needed;
            else if(value > needed)
            {
                value -= step;
                if (value < needed)
                    return needed;
            }
            else
            {
                value += step;
                if (value > needed)
                    return needed;
            }

            return value;
        }

        public void RgbToHls(int r, int g, int b,
            out double h, out double l, out double s)
        {
            // Convert RGB to a 0.0 to 1.0 range.
            double double_r = r / 255.0;
            double double_g = g / 255.0;
            double double_b = b / 255.0;

            // Get the maximum and minimum RGB components.
            double max = double_r;
            if (max < double_g) max = double_g;
            if (max < double_b) max = double_b;

            double min = double_r;
            if (min > double_g) min = double_g;
            if (min > double_b) min = double_b;

            double diff = max - min;
            l = (max + min) / 2;
            if (Math.Abs(diff) < 0.00001)
            {
                s = 0;
                h = 0;  // H is really undefined.
            }
            else
            {
                if (l <= 0.5) s = diff / (max + min);
                else s = diff / (2 - max - min);

                double r_dist = (max - double_r) / diff;
                double g_dist = (max - double_g) / diff;
                double b_dist = (max - double_b) / diff;

                if (double_r == max) h = b_dist - g_dist;
                else if (double_g == max) h = 2 + r_dist - b_dist;
                else h = 4 + g_dist - r_dist;

                h = h * 60;
                if (h < 0) h += 360;
            }
        }

        // Convert an HLS value into an RGB value.
        public static void HlsToRgb(double h, double l, double s,
            out int r, out int g, out int b)
        {
            double p2;
            if (l <= 0.5) p2 = l * (1 + s);
            else p2 = l + s - l * s;

            double p1 = 2 * l - p2;
            double double_r, double_g, double_b;
            if (s == 0)
            {
                double_r = l;
                double_g = l;
                double_b = l;
            }
            else
            {
                double_r = QqhToRgb(p1, p2, h + 120);
                double_g = QqhToRgb(p1, p2, h);
                double_b = QqhToRgb(p1, p2, h - 120);
            }

            // Convert RGB to the 0 to 255 range.
            r = (int)(double_r * 255.0);
            g = (int)(double_g * 255.0);
            b = (int)(double_b * 255.0);
        }

        private static double QqhToRgb(double q1, double q2, double hue)
        {
            if (hue > 360) hue -= 360;
            else if (hue < 0) hue += 360;

            if (hue < 60) return q1 + (q2 - q1) * hue / 60;
            if (hue < 180) return q2;
            if (hue < 240) return q1 + (q2 - q1) * (240 - hue) / 60;
            return q1;
        }
    }
}
