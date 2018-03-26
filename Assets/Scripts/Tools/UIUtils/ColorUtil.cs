using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ColorEnum
{
    NULL,
    White,
    Black,
    Purple0,
    Purple1,
    Green0,
    Green1,
    Orange0,
}

public class ColorUtil{
    public static Color GetColor(ColorEnum color_enum)
    {
        Color color;
        switch (color_enum)
        {
            case ColorEnum.White:
                color = Color.white;
                break;
            case ColorEnum.Black:
                color = Color.black;
                break;
            case ColorEnum.Purple0:
                color = new Color(0.9f, 0.55f, 1);
                break;
            case ColorEnum.Purple1:
                color = new Color(0.25f, 0.1f, 0.35f);
                break;
            case ColorEnum.Green0:
                color = new Color(0.4f, 0.9f, 0.05f);
                break;
            case ColorEnum.Orange0:
                color = new Color(1f, 0.7f, 0.53f);
                break;
            default:
                color = Color.white;
                break;
        }
        return color;
    }
}
