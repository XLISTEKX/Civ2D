using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour
{
    public TMP_Text counter;
    public Image image;

    public void initSlot(Sprite image, string text)
    {
        this.image.sprite = image;
        counter.text = text;
    }
}
