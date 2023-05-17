using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile_City : Tile, IPointerClickHandler
{
    Player owner;

    public string cityName;

    public ResourcesTile cityResouces = new ResourcesTile(0,0,0);
    public List<GameObject> buildingsBuild = new List<GameObject>();
    public List<GameObject> productionQueue = new List<GameObject>();
    public List<GameObject> possibleBuildings;
    Gameplay_Controler gameplay_Controler;
    [SerializeField] TMP_Text text_cityName;
    [SerializeField] Image constructionImage,panelColor;
    [SerializeField] Sprite gearImage;
    [SerializeField] TMP_Text constructionTurnLeft;
    public int buildingProgress;
    public int buildProduction;

    public int turnsLeft;
    public void initCityTile(Vector2Int position, Player owner)
    {
        this.owner = owner;
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";
        cityName = City.randomCityName();

        gameplay_Controler = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
        possibleBuildings = new List<GameObject>(owner.possibleBuildings);
        updateUI();
        text_cityName.text = cityName;
        panelColor.color = owner.color;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (canClick)
        {
            gameplay_Controler.openCity(this);
        }
            
    }

    public void addToQueue(int id)
    {
        productionQueue.Add(possibleBuildings[id]);
        possibleBuildings.RemoveAt(id);

        if(productionQueue.Count == 1)
        {
            
            buildProduction = productionQueue[0].GetComponent<Building>().buildCost;
            turnsLeft = Mathf.CeilToInt((buildProduction - buildingProgress) / (float)cityResouces.production);
        }
        updateUI();
    }
    void updateUI()
    {
        
        if(productionQueue.Count == 0)
        {
            constructionImage.sprite = gearImage;
            constructionTurnLeft.text = "";
            return;
        }
        constructionImage.sprite = productionQueue[0].GetComponent<Building>().buildingSprite;
        constructionTurnLeft.text = turnsLeft.ToString();
    }

    public void removeFromQueue(int ID)
    {
        possibleBuildings.Add(productionQueue[ID]);
        productionQueue.RemoveAt(ID);
        updateUI();
    }
    public void nextTurn()
    {
        if (productionQueue.Count == 0) {
            return;
        }
        int temp = cityResouces.production;

        buildingProgress = buildingProgress + temp;
        turnsLeft = Mathf.CeilToInt((buildProduction - buildingProgress) / (float)cityResouces.production);

        if(buildingProgress >= buildProduction)
        {
            buildingProgress -= buildProduction;
            buildProduction = productionQueue[0].GetComponent<Building>().buildCost;
            productionQueue[0].GetComponent<Building>().build(this);
            productionQueue.RemoveAt(0);
        }
        updateUI();
    }

    
}
