using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Controler : MonoBehaviour
{
    [SerializeField] 
    List<GameObject> UIPanels;
    [SerializeField]
    List<TMP_Text> resourcesText;

    [Header("Toolbar")]
    [SerializeField] RectTransform toolbar;
    bool toolBarState;
    [Header("City Settings")]
    [SerializeField] 
    TMP_Text turnTxt;
    [SerializeField]
    GameObject slotPrefab;

    Player player;

    public int ID = 0;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        updateUI();
    }

    public void openPanel(int id)
    {
        UIPanels[id].SetActive(true);
        UIPanels[ID].SetActive(false);
        ID = id;
        if (id == 0)
            updateUI();
    }

    public void openCloseCity(Tile_City city)
    {
        if (ID == 2)
        {
            
            openPanel(0);
            UIPanels[2].GetComponent<UI_City_Controler>().destroyBuildTiles();
            return;
        }
        UIPanels[2].GetComponent<UI_City_Controler>().city = city;
        Vector3 position = city.transform.position;
        Camera.main.transform.position = new(position.x, position.y + 0.5f, Camera.main.transform.position.z);
        openPanel(2);

        
    }


    public void openCloseToolbar()
    {
        Vector3 pos = toolbar.anchoredPosition;

        if (toolBarState)
        {
            pos.x = -toolbar.sizeDelta.x / 2;
            toolbar.anchoredPosition = pos;
            toolBarState = false;
            return;
        }

        pos.x = toolbar.sizeDelta.x / 2;
        toolbar.anchoredPosition = pos;
        toolBarState = true;
    }

    public void updateUI()
    {
        int money_gain = 0;
        int science = 0;

        foreach (Tile_City city in player.allCities)
        {
            ResourcesTile resources = city.GetResources();

            money_gain += resources.cash;
            science += resources.science;
        }

        resourcesText[0].text = player.money + "+" + money_gain;
        resourcesText[1].text = "+" + science.ToString();

    }
    public void nextTurn(int turn)
    {
        updateUI();
        turnTxt.text = "Turn: " + turn.ToString();
    }
}
