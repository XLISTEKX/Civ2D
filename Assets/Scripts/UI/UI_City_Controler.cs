using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_City_Controler : MonoBehaviour
{
    [SerializeField]
    GameObject slotPrefab, slotQueuePrefab;
    [SerializeField]
    Transform spawnSlot, spawnQueue;
    [SerializeField]
    RectTransform layout;
    [SerializeField]
    List<TMP_Text> cityTexts; // 0 - food, 1 - cash, 2 - production

    List<UI_Slot> slots = new List<UI_Slot>();
    List<UI_Slot> slotsQueue = new List<UI_Slot>();

    public Tile_City city;

    private void OnEnable()
    {
        updateUI();
        
    }
    private void OnDisable()
    {
        city = null;
        destroyUI();
    }

    void updateUI()
    {
        destroyUI();

        cityTexts[0].text = city.cityResouces.food.ToString();
        cityTexts[1].text = city.cityResouces.cash.ToString();
        cityTexts[2].text = city.cityResouces.production.ToString();

        int i = 0;
        foreach (GameObject buildingGO in city.possibleBuildings)
        {
            Building building = buildingGO.GetComponent<Building>();

            float time = (float) building.buildCost / city.cityResouces.production;
            int newTime = Mathf.CeilToInt(time);

            UI_Slot slot = Instantiate(slotPrefab, spawnSlot).GetComponent<UI_Slot>();
            slot.initSlot(building.buildingSprite, newTime.ToString() + "turns");
            int temp = i;
            slot.GetComponent<Button>().onClick.AddListener(() => clickSlot(temp));
            slots.Add(slot);

            i++;
        }
        for (int j = 0; j < city.productionQueue.Count; j++)
        {
            Building building = city.productionQueue[j].GetComponent<Building>();

            UI_Slot slot = Instantiate(slotQueuePrefab, spawnQueue).GetComponent<UI_Slot>();

            float time;

            if(j == 0)
            {
                time = (city.buildProduction - city.buildingProgress) / (float)city.cityResouces.production;
            }
            else
            {
                time = (float)building.buildCost / city.cityResouces.production;
            }
            
            int newTime = Mathf.CeilToInt(time);

            slot.initSlot(building.buildingSprite, newTime.ToString());
            int temp = j;
            slot.GetComponent<Button>().onClick.AddListener(() => clickSlotQueue(temp));
            slotsQueue.Add(slot);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
    }
    void destroyUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Destroy(slots[i].gameObject);
        }
        slots.Clear();
        for (int i = 0; i < slotsQueue.Count; i++)
        {
            Destroy(slotsQueue[i].gameObject);
        }
        slotsQueue.Clear();
    }

    public void clickSlot(int ID)
    {
        city.addToQueue(ID);
        updateUI();

    }
    public void clickSlotQueue(int ID)
    {
        city.removeFromQueue(ID);
        updateUI();
    }
}
