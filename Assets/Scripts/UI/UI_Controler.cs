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

    int ID = 0;


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


    public void openCloseToolbar()
    {
        Vector3 pos = toolbar.position;

        if (toolBarState)
        {
            pos.x = -toolbar.sizeDelta.x / 4;
            toolbar.position = pos;
            toolBarState = false;
            return;
        }

        pos.x = toolbar.sizeDelta.x / 4;
        toolbar.position = pos;
        toolBarState = true;
    }

    void updateUI()
    {
        resourcesText[0].text = player.money.ToString();
        resourcesText[1].text = player.science.ToString();

    }
    public void nextTurn(int turn)
    {
        updateUI();
        turnTxt.text = "Turn: " + turn.ToString();
    }
}
