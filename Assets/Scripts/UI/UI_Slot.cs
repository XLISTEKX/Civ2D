using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Slot : MonoBehaviour
{
    public TMP_Text counter;
    public Image image;

    public void initSlot(Sprite image, string text = null)
    {
        this.image.sprite = image;

        if(text != null)
        {
            counter.text = text;
        }
        
    }
}
