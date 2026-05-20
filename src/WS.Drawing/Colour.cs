namespace WS.Drawing;

/// <summary>
/// Represents an RGBA colour with 8-bit channels.
/// </summary>
/// <param name="Red">Red channel (0-255).</param>
/// <param name="Green">Green channel (0-255).</param>
/// <param name="Blue">Blue channel (0-255).</param>
/// <param name="Alpha">Alpha channel (0-255), defaults to 255 (opaque).</param>
public record Colour(byte Red, byte Green, byte Blue, byte Alpha = 255)
{
    /// <summary>
    /// Modes for the `Lighten` operation.
    /// </summary>
    public enum LightenMode
    {
        /// <summary>Use HSL interpolation (preserve hue via HSL).</summary>
        Hsl,
        /// <summary>Use CIELAB L* interpolation (perceptual lightness).</summary>
        Lab
    }
    /// <summary>
    /// Converts an RGB colour to HSL components.
    /// </summary>
    /// <param name="r">Red channel (0-255).</param>
    /// <param name="g">Green channel (0-255).</param>
    /// <param name="b">Blue channel (0-255).</param>
    /// <returns>A tuple containing Hue (0..1), Saturation (0..1) and Lightness (0..1).</returns>
    public static (double H, double S, double L) RgbToHsl(byte r, byte g, byte b)
    {
        var rd = r / 255.0;
        var gd = g / 255.0;
        var bd = b / 255.0;

        var max = Math.Max(rd, Math.Max(gd, bd));
        var min = Math.Min(rd, Math.Min(gd, bd));
        var l = (max + min) / 2.0;

        if (Math.Abs(max - min) < 1e-12)
        {
            return (0.0, 0.0, l);
        }

        var d = max - min;
        var s = d / (1.0 - Math.Abs(2.0 * l - 1.0));

        double h;
        if (Math.Abs(max - rd) < 1e-12)
        {
            h = (gd - bd) / d;
        }
        else if (Math.Abs(max - gd) < 1e-12)
        {
            h = (bd - rd) / d + 2.0;
        }
        else
        {
            h = (rd - gd) / d + 4.0;
        }

        h /= 6.0;
        if (h < 0.0)
        {
            h += 1.0;
        }
        return (h, s, l);
    }

    /// <summary>
    /// Converts HSL components to an RGB triple.
    /// </summary>
    /// <param name="h">Hue value in range 0..1.</param>
    /// <param name="s">Saturation in range 0..1.</param>
    /// <param name="l">Lightness in range 0..1.</param>
    /// <returns>RGB channels as bytes (0-255).</returns>
    public static (byte R, byte G, byte B) HslToRgb(double h, double s, double l)
    {
        if (s == 0.0)
        {
            var v = (byte)Math.Round(l * 255.0);
            return (v, v, v); // achromatic
        }

        var c = (1.0 - Math.Abs(2.0 * l - 1.0)) * s;
        var hPrime = h * 6.0;
        var x = c * (1.0 - Math.Abs(hPrime % 2.0 - 1.0));
        double r2 = 0, g2 = 0, b2 = 0;
        var sector = (int)Math.Floor(hPrime) % 6;
        switch (sector)
        {
            case 0: r2 = c; g2 = x; b2 = 0; break;
            case 1: r2 = x; g2 = c; b2 = 0; break;
            case 2: r2 = 0; g2 = c; b2 = x; break;
            case 3: r2 = 0; g2 = x; b2 = c; break;
            case 4: r2 = x; g2 = 0; b2 = c; break;
            case 5: r2 = c; g2 = 0; b2 = x; break;
        }
        var m = l - c / 2.0;

        static byte ToByte(double v)
        {
            return (byte)Math.Clamp((int)Math.Round(v * 255.0), 0, 255);
        }

        return (ToByte(r2 + m), ToByte(g2 + m), ToByte(b2 + m));
    }

    /// <summary>
    /// Converts an sRGB colour to CIELAB (L*, a*, b*).
    /// </summary>
    /// <param name="r">Red channel (0-255).</param>
    /// <param name="g">Green channel (0-255).</param>
    /// <param name="b">Blue channel (0-255).</param>
    /// <returns>CIELAB tuple where L* is in 0..100.</returns>
    public static (double L, double A, double B) RgbToLab(byte r, byte g, byte b)
    {
        // Convert to linear sRGB
        static double SrgbToLinear(double c)
        {
            if (c <= 0.04045) return c / 12.92;
            return Math.Pow((c + 0.055) / 1.055, 2.4);
        }

        var rd = SrgbToLinear(r / 255.0);
        var gd = SrgbToLinear(g / 255.0);
        var bd = SrgbToLinear(b / 255.0);

        // Convert linear RGB to XYZ (D65)
        var x = rd * 0.4124564 + gd * 0.3575761 + bd * 0.1804375;
        var y = rd * 0.2126729 + gd * 0.7151522 + bd * 0.0721750;
        var z = rd * 0.0193339 + gd * 0.1191920 + bd * 0.9503041;

        // Scale to the reference white (percentage)
        x *= 100.0;
        y *= 100.0;
        z *= 100.0;

        // D65 reference white
        const double Xn = 95.047;
        const double Yn = 100.000;
        const double Zn = 108.883;

        static double F(double t)
        {
            const double delta = 6.0 / 29.0; // ~0.2068965517
            if (t > Math.Pow(delta, 3)) return Math.Pow(t, 1.0 / 3.0);
            return t / (3 * delta * delta) + 4.0 / 29.0;
        }

        var fx = F(x / Xn);
        var fy = F(y / Yn);
        var fz = F(z / Zn);

        var L = 116.0 * fy - 16.0;
        var A = 500.0 * (fx - fy);
        var Bv = 200.0 * (fy - fz);

        return (L, A, Bv);
    }

    /// <summary>
    /// Converts CIELAB (L*, a*, b*) to sRGB bytes.
    /// </summary>
    /// <param name="L">Lightness (0..100).</param>
    /// <param name="A">a* channel.</param>
    /// <param name="B">b* channel.</param>
    /// <returns>RGB channels as bytes (0-255).</returns>
    public static (byte R, byte G, byte B) LabToRgb(double L, double A, double B)
    {
        // D65 reference white
        const double Xn = 95.047;
        const double Yn = 100.000;
        const double Zn = 108.883;

        double fy = (L + 16.0) / 116.0;
        double fx = A / 500.0 + fy;
        double fz = fy - B / 200.0;

        static double InvF(double f)
        {
            const double delta = 6.0 / 29.0;
            if (f > delta) return f * f * f;
            return 3 * delta * delta * (f - 4.0 / 29.0);
        }

        var xr = InvF(fx);
        var yr = InvF(fy);
        var zr = InvF(fz);

        var x = xr * Xn / 100.0;
        var y = yr * Yn / 100.0;
        var z = zr * Zn / 100.0;

        // Convert XYZ to linear RGB
        var rl = x * 3.2404542 + y * -1.5371385 + z * -0.4985314;
        var gl = x * -0.9692660 + y * 1.8760108 + z * 0.0415560;
        var bl = x * 0.0556434 + y * -0.2040259 + z * 1.0572252;

        static double LinearToSrgb(double c)
        {
            if (c <= 0.0031308) return 12.92 * c;
            return 1.055 * Math.Pow(c, 1.0 / 2.4) - 0.055;
        }

        var r = LinearToSrgb(rl);
        var g = LinearToSrgb(gl);
        var b = LinearToSrgb(bl);

        static byte ToByteClamp(double v)
        {
            return (byte)Math.Clamp((int)Math.Round(v * 255.0), 0, 255);
        }

        return (ToByteClamp(r), ToByteClamp(g), ToByteClamp(b));
    }


    /// <summary>
    /// Returns a new <see cref="Colour"/> lightened toward white by the specified percentage.
    /// </summary>
    /// <param name="percentage">A value between 0 and 100 inclusive. 0 returns the original colour; 100 returns white.</param>
    /// <returns>A new <see cref="Colour"/> with channels moved toward 255 by the given percentage.</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="percentage"/> is not between 0 and 100.</exception>
    public Colour Lighten(float percentage)
    {
        // Preserve previous default behaviour (HSL) for backward compatibility.
        return Lighten(percentage, LightenMode.Hsl);
    }

    /// <summary>
    /// Returns a new <see cref="Colour"/> lightened toward white by the specified percentage,
    /// using the requested interpolation <paramref name="mode"/>.
    /// </summary>
    public Colour Lighten(float percentage, LightenMode mode)
    {
        if (percentage < 0f || percentage > 100f)
        {
            throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be between 0 and 100.");
        }

        if (percentage == 0f)
        {
            return this;
        }

        if (percentage == 100f)
        {
            return new Colour(255, 255, 255, Alpha);
        }

        var factor = percentage / 100f;

        if (mode == LightenMode.Lab)
        {
            // Use CIELAB L* interpolation to better match perceived lightness.
            var (L0, A0, B0) = RgbToLab(Red, Green, Blue);
            var L1 = L0 + (100.0 - L0) * factor;
            var (r1, g1, b1) = LabToRgb(L1, A0, B0);
            return new Colour(r1, g1, b1, Alpha);
        }

        // HSL fallback (preserve hue via HSL interpolation — restores previous behaviour).
        var (h0, s0, l0) = RgbToHsl(Red, Green, Blue);
        var l1h = l0 + (1.0 - l0) * factor;
        var (rh, gh, bh) = HslToRgb(h0, s0, l1h);
        return new Colour(rh, gh, bh, Alpha);
    }
}