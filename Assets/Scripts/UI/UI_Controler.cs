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
        UIPanels[2].GetComponent<UI_City_Controler>().city = city;

        openPanel(2);

        
    }
    public void nextTurn(int turn)
    {
        turnTxt.text = "Turn: " + turn.ToString();
    }
}
