using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_City_Controler : MonoBehaviour
{
    [SerializeField]
    GameObject slotPrefab, slotBuyPrefab, slotBuild;
    [SerializeField]
    Transform spawnSlot,spawnUnitSlot, spawnQueue;
    [SerializeField]
    RectTransform layout;
    [SerializeField]
    List<TMP_Text> cityTexts; // 0 - food, 1 - cash, 2 - production, 3 - science, 4 - name

    List<UI_Slot> slots = new List<UI_Slot>();
    List<UI_Slot> slotsQueue = new List<UI_Slot>();
    List<UI_Slot> slotsUnits = new List<UI_Slot>();
    List<GameObject> slotsBuild = new();

    bool isBuyTileOpen = false;
    List<GameObject> buySlots = new List<GameObject>();

    public Tile_City city;

    private void OnEnable()
    {
        updateUI();
        cityTexts[4].text = city.cityName;
        cityTexts[4].color = city.owner.color;

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

        string[] strings = City.GetUIResourceValue(city.cityResouces);

        cityTexts[0].text = strings[0];
        cityTexts[1].text = strings[1];
        cityTexts[2].text = strings[2];
        cityTexts[3].text = strings[3];


        for (int i = 0; i < city.possibleBuildings.Count; i++)
        {
            IProduct product = city.possibleBuildings[i].GetComponent<IProduct>();

            float time = City.turnsToBuild(city.cityResouces.production, product.GetBuildCost());


                UI_Slot slot = Instantiate(slotPrefab, spawnSlot).GetComponent<UI_Slot>();
                slot.initSlot(product.GetImage(), time.ToString(), product.type());
                int temp = i;
                slot.GetComponent<Button>().onClick.AddListener(() => clickSlot(temp, product.type()));
                slots.Add(slot);

            
        }
        for (int j = 0; j < city.possibleUnits.Count; j++)
        {
            Unit unit = city.possibleUnits[j].GetComponent<Unit>();

            UI_Slot slot = Instantiate(slotPrefab, spawnUnitSlot).GetComponent<UI_Slot>();

            float time = City.turnsToBuild(city.cityResouces.production, unit.productionCost);

            slot.initSlot(unit.unitSprite, time.ToString(), 1);
            int temp = j;
            slot.GetComponent<Button>().onClick.AddListener(() => clickSlot(temp, unit.type()));
            slotsUnits.Add(slot);
        }
        for (int j = 0; j < city.productionQueue.Count; j++)
        {
            IProduct product = city.productionQueue[j].GetComponent<IProduct>();

            UI_Slot slot = Instantiate(slotPrefab, spawnQueue).GetComponent<UI_Slot>();

            slot.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);

            float time;

            if(j == 0)
            {
                time = city.turnsLeft;
            }
            else
            {
                time = City.turnsToBuild(city.cityResouces.production, product.GetBuildCost());
            }
            

            slot.initSlot(product.GetImage(), time.ToString(), product.type());
            int temp = j;
            slot.GetComponent<Button>().onClick.AddListener(() => clickSlotQueue(temp));
            slotsQueue.Add(slot);
        }

        layout.gameObject.SetActive(false);
        layout.gameObject.SetActive(true);
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

    public void clickSlot(int ID, int type = 0)
    {
        if(type == 2)
        {
            OpenBuildTile(ID);
            
            return;
        }

        city.AddToQueue(ID, type);
        updateUI();
    }
    void OpenBuildTile(int ID)
    {
        if (slotsBuild.Count > 0)
            return;
        Gameplay_Controler gameplay = Gameplay_Controler.GetControler();
        foreach (Tile tile in city.cityTiles)
        {
            if (!gameplay.IsNeighborCityTile(tile))
                continue;

            if (!tile.block && tile.getType() == 0)
            {
                slotsBuild.Add(Instantiate(slotBuild, tile.transform.position, slotBuild.transform.rotation));
                SlotTileBuild slot = slotsBuild[^1].GetComponent<SlotTileBuild>();
                slot.tile = tile;
                slot.UIcity = this;
                slot.ID = ID;
            }
        }
    }

    public void destroyBuildTiles()
    {
        if (slotsBuild.Count == 0)
            return;

        foreach(GameObject slot in slotsBuild)
        {
            Destroy(slot);
        }
        slotsBuild.Clear();
    }
    public void clickSlotQueue(int ID)
    {
        city.RemoveFromQueue(ID);
        updateUI();
    }

    public void openBuyTileMenu()
    {
        Gameplay_Controler _Controler = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
        if (isBuyTileOpen)
        {
            for(int i = 0; i < buySlots.Count; i++)
            {
                Destroy(buySlots[i]);
            }
            buySlots.Clear();
            isBuyTileOpen = false;
            _Controler.canClick = true;
        }
        else
        {
            updateBuyTile();
            isBuyTileOpen = true;
            _Controler.canClick = false;
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
        Tile[] tiles = _Controler.GetTilesBuyExpanse(city, city.currentRange);

        if(tiles.Length == 0)
        {
            city.currentRange++;
            tiles = _Controler.GetTilesBuyExpanse(city, city.currentRange);
        }
            
        foreach (Tile tempTile in tiles)
        {
            buySlots.Add(Instantiate(slotBuyPrefab, tempTile.transform.position, slotBuyPrefab.transform.rotation));
            buySlots[^1].GetComponent<BuyTileSlot>().initSlot(tempTile, city);
        }

    }
}
