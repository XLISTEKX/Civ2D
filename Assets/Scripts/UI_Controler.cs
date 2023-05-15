using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Controler : MonoBehaviour
{
    [SerializeField] 
    List<GameObject> UIPanels;
    [Header("City Settings")]
    [SerializeField] 
    List<TMP_Text> cityTexts; // 0 - food, 1 - cash, 2 - production
    [SerializeField] 
    TMP_Text turnTxt;
    [SerializeField]
    GameObject slotPrefab;
    int ID = 0;
    
    public void openPanel(int id)
    {
        UIPanels[id].SetActive(true);
        UIPanels[ID].SetActive(false);
        ID = id;
    }

    public void openCloseCity(Tile_City city, bool isOpen = true)
    {
        if (ID == 2)
        {
            openPanel(0);
            return;
        }
        openPanel(2);

        cityTexts[0].text = city.cityResouces.food.ToString();
        cityTexts[1].text = city.cityResouces.cash.ToString();
        cityTexts[2].text = city.cityResouces.production.ToString();
    }
    public void nextTurn(int turn)
    {
        turnTxt.text = "Turn: " + turn.ToString();
    }
}
