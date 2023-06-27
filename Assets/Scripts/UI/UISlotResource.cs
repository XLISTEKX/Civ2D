using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlotResource : MonoBehaviour
{
    [SerializeField] Image icon;

    public void InitSlot(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void TurnVisibility(bool turn)
    {
        gameObject.SetActive(turn);
    }
}
