using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_City_Controler : MonoBehaviour
{
    [SerializeField]
    GameObject slotPrefab, slotQueuePrefab, slotBuyPrefab;
    [SerializeField]
    Transform spawnSlot,spawnUnitSlot, spawnQueue;
    [SerializeField]
    RectTransform layout;
    [SerializeField]
    List<TMP_Text> cityTexts; // 0 - food, 1 - cash, 2 - production

    List<UI_Slot> slots = new List<UI_Slot>();
    List<UI_Slot> slotsQueue = new List<UI_Slot>();
    List<UI_Slot> slotsUnits = new List<UI_Slot>();


    bool isBuyTileOpen = false;
    List<GameObject> buySlots = new List<GameObject>();

    public Tile_City city;

    private void OnEnable()
    {
        updateUI();
        cityTexts[3].text = city.cityName;
        cityTexts[3].color = city.owner.color;

    }
    private void OnDisable()
    {
        if (isBuyTileOpen)
        {
            openBuyTileMenu();
        }
        
        city = null;
        destroyUI();
        
    }

    public void updateUI()
    {
        destroyUI();

        cityTexts[0].text = city.cityResouces.food.ToString();
        cityTexts[1].text = city.cityResouces.cash.ToString();
        cityTexts[2].text = city.cityResouces.production.ToString();

        int i = 0;
        foreach (GameObject buildingGO in city.possibleBuildings)
        {
            Building building = buildingGO.GetComponent<Building>();

            float time = City.turnsToBuild(city.cityResouces.production, building.buildCost);

            UI_Slot slot = Instantiate(slotPrefab, spawnSlot).GetComponent<UI_Slot>();
            slot.initSlot(building.buildingSprite, time.ToString() + "turns");
            int temp = i;
            slot.GetComponent<Button>().onClick.AddListener(() => clickSlot(temp));
            slots.Add(slot);

            i++;
        }
        for (int j = 0; j < city.possibleUnits.Count; j++)
        {
            Unit unit = city.possibleUnits[j].GetComponent<Unit>();

            UI_Slot slot = Instantiate(slotPrefab, spawnUnitSlot).GetComponent<UI_Slot>();

            float time = City.turnsToBuild(city.cityResouces.production, unit.productionCost);

            slot.initSlot(unit.unitSprite, time.ToString() + "turns");
            int temp = j;
            slot.GetComponent<Button>().onClick.AddListener(() => clickUnitSlot(temp));
            slotsUnits.Add(slot);
        }
        for (int j = 0; j < city.productionQueue.Count; j++)
        {
            IProduct product = city.productionQueue[j].GetComponent<IProduct>();

            UI_Slot slot = Instantiate(slotQueuePrefab, spawnQueue).GetComponent<UI_Slot>();

            float time;

            if(j == 0)
            {
                time = city.turnsLeft;
            }
            else
            {
                time = City.turnsToBuild(city.cityResouces.production, product.getBuildCost());
            }
            

            slot.initSlot(product.getImage(), time.ToString());
            int temp = j;
            slot.GetComponent<Button>().onClick.AddListener(() => clickSlotQueue(temp));
            slotsQueue.Add(slot);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
    }
    void destroyUI()
    {
        foreach (UI_Slot slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();
        foreach (UI_Slot slot in slotsQueue)
        {
            Destroy(slot.gameObject);
        }
        slotsQueue.Clear();
        foreach (UI_Slot slot in slotsUnits)
        {
            Destroy(slot.gameObject);
        }
        slotsUnits.Clear();
        
    }

    public void clickSlot(int ID)
    {
        city.addToQueue(ID, 0);
        updateUI();

    }
    public void clickUnitSlot(int ID)
    {
        city.addToQueue(ID, 1);
        updateUI();
    }
    public void clickSlotQueue(int ID)
    {
        city.removeFromQueue(ID);
        updateUI();
    }

    public void openBuyTileMenu()
    {

        if (isBuyTileOpen)
        {
            for(int i = 0; i < buySlots.Count; i++)
            {
                Destroy(buySlots[i]);
            }
            buySlots.Clear();
            isBuyTileOpen = false;
        }
        else
        {
            updateBuyTile();
            isBuyTileOpen = true;
        }
        
    }

    public void updateBuyTile()
    {
        if(buySlots.Count > 0)
        {
            foreach(GameObject gObject in buySlots)
            {
                Destroy(gObject);
            }
        }
        Gameplay_Controler _Controler = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
        Tile[] tempTiles;
        tempTiles = _Controler.cubeRing(city, city.currentRange);

        for (int i = 0; i < tempTiles.Length; i++)
        {
            if (tempTiles[i].owner == null)
            {
                buySlots.Add(Instantiate(slotBuyPrefab, tempTiles[i].transform));
                buySlots[buySlots.Count - 1].GetComponent<BuyTileSlot>().initSlot(tempTiles[i], city);
            }

        }

    }
}
