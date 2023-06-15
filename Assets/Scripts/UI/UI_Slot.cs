using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour
{
    public TMP_Text counter;
    public Image image, border,backgroud;
    [SerializeField] Sprite[] borderTypes, backgroundTypes;

    public void initSlot(Sprite image, string text = null, int type = 0)
    {
        this.image.sprite = image;

        if(text != null)
        {
            counter.text = text;
        }
        border.sprite = borderTypes[type];
        backgroud.sprite = backgroundTypes[type];
    }
}
