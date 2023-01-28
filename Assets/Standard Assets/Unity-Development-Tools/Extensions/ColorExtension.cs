using UnityEngine;


public static class ColorExtension
{
    public static Color ChangeAlpha(this Color thisColor, float newAlpha)
    {
        return new Color(thisColor.r, thisColor.g, thisColor.b, newAlpha);
    }

    public static Color ChangeColor(this Color thisColor, Color newColor, bool keepAlpha = false)
    {
        if (keepAlpha)
        {
            return ChangeAlpha(newColor, thisColor.a);
        }

        return newColor;
    }
}