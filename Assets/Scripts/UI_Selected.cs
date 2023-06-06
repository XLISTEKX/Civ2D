
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Selected : MonoBehaviour
{
    [SerializeField] TMP_Text[] texts;   // 0 - movement, 1 - Damage
    [SerializeField] Slider healthBar;
    [SerializeField] Image icon;

    public void updateUI(Unit unit)
    {
        texts[0].text = unit.movementLeft.ToString() + "/" + unit.movementRange.ToString();
        texts[1].text = unit.damage.ToString();

        healthBar.value =  unit.health / (float) unit.maxHealth;

        icon.sprite = unit.unitSprite;
    }
}
