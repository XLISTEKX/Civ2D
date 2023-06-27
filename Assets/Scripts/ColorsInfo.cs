using UnityEngine;

public class ColorsInfo : MonoBehaviour
{
    public static Color getColorByGold(int yourCash, int needCash)
    {
        if(yourCash >= needCash)
        {
            return Color.green;
        }
        else
        {
            return Color.red;
        }
    }
    public static Color GetColorByHealth(int currentHealth, int maxHealth)
    {
        float x = currentHealth / (float) maxHealth;

        return x switch
        {
            <= 0.2f => Color.red,
            <= 0.4f => new(255, 128, 0),
            <= 0.6f => Color.yellow,
            <= 1f => Color.green,
            _ => Color.white
        };
    }

    public static Color RGBtoRGBHSV(Color input)
    {
        Color.RGBToHSV(input, out float h, out float s, out float v);
        return Color.HSVToRGB(h, 0.6f, v);
    }
}
