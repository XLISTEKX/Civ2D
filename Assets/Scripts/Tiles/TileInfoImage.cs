using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoImage : MonoBehaviour
{
    public Image icon;
    public TMP_Text counter;

    public void InitImage(Sprite sprite, int ammount)
    {
        icon.sprite = sprite;

        counter.text = ammount.ToString();
    }
}
