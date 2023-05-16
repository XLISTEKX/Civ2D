using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_City : MonoBehaviour
{
    [SerializeField]
    Transform buildingTransform;
    [SerializeField]
    List<TMP_Text> cityTexts;
    [SerializeField]
    GameObject slotPrefab, slotBuildingPrefab;

    List<UI_Slot> slots = new List<UI_Slot>();

    public Tile_City city;

    private void OnEnable()
    {
      
        cityTexts[0].text = city.cityResouces.food.ToString();
        cityTexts[1].text = city.cityResouces.cash.ToString();
        cityTexts[2].text = city.cityResouces.production.ToString();
    }
    private void OnDisable()
    {
        city = null;
    }

}
