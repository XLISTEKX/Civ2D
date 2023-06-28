using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Selected : MonoBehaviour
{
    [SerializeField] TMP_Text[] texts;   // 0 - movement, 1 - Damage
    [SerializeField] Slider healthBar;
    [SerializeField] Image icon;
    [SerializeField] Transform transform_usables;
    [SerializeField] GameObject slotPrefab;

    List<UI_Slot> slots = new();

    public void updateUI(Unit unit)
    {
        if (slots.Count != 0)
            destroySlots();

        texts[0].text = unit.movementLeft.ToString() + "/" + unit.movementRange.ToString();
        texts[1].text = unit.damage.ToString();

        //healthBar.value =  unit.health / (float) unit.maxHealth;

        icon.sprite = unit.unitSprite;

        if(unit.actions != null)
            setUsables(unit);
        
    }

    public void setUsables(Unit unit)
    {
        foreach(UnityEvent action in unit.actions)
        {
            slots.Add(Instantiate(slotPrefab, transform_usables).GetComponent<UI_Slot>());

            GameObject gameObject = slots[^1].gameObject;

            gameObject.GetComponent<Button>().onClick.AddListener(action.Invoke);

            slots[^1].initSlot(unit.actionsIcons[slots.Count - 1]);
        }
    }

    void destroySlots()
    {
        foreach(UI_Slot slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();
    }
}
