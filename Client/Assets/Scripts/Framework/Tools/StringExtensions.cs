using UnityEngine;

public static class StringExtensions {
    public static Color ToColor(this string hex, float defaultAlpha = 1f)
    {
        if (string.IsNullOrEmpty(hex))
            return Color.white;

        if (!hex.StartsWith("#"))
            hex = "#" + hex;

        // RGB
        if (hex.Length == 7 && ColorUtility.TryParseHtmlString(hex, out var rgb))
            return rgb;

        // RGBA
        if (hex.Length == 9 && ColorUtility.TryParseHtmlString(hex, out var rgba))
            return rgba;

        return new Color(1, 1, 1, defaultAlpha);
    }
}
