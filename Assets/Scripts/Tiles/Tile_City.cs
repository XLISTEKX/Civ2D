using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile_City : Tile, IPointerClickHandler, ITurnCity
{

    public string cityName;
    public int population = 1;
    public int currentRange = 1;

    public ResourcesTile cityResouces = new ResourcesTile(0,0,0,0);
    public List<GameObject> buildingsBuild = new();
    public List<GameObject> productionQueue = new();
    public List<GameObject> possibleBuildings, possibleUnits;
    public List<TileConstruction> buildLocations = new();
    public List<Tile> cityTiles = new();
    
    [SerializeField] TMP_Text text_cityName, text_pop;
    [SerializeField] Image constructionImage,panelColor;
    [SerializeField] Sprite gearImage;
    [SerializeField] TMP_Text constructionTurnLeft;

    public int buildingProgress;
    public int buildProduction;

    public int popProgress;
    int popProgressMax = 50;

    public int turnsLeft;
    public void InitCityTile(Vector2Int position, Player owner)
    {
        this.owner = owner;
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";
        cityName = City.randomCityName();

        possibleBuildings = new List<GameObject>(owner.possibleBuildings);
        possibleUnits = new List<GameObject>(owner.possibleUnits);

        UpdateUI();
        


    }

    public override int getType()
    {
        return 1;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (canClick)
        {
            GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>().SelectTile(this);
        }
            
    }

    public void AddToQueue(int id, int listID)
    {
        switch (listID)
        {
            case 0:
                productionQueue.Add(possibleBuildings[id]);
                possibleBuildings.RemoveAt(id);
                break;
            case 1:
                productionQueue.Add(possibleUnits[id]);
                break;
            case 2:
                productionQueue.Add(possibleBuildings[id]);
                buildLocations[^1].setConstruction(possibleBuildings[id], this);
                break;
        }
        

        if(productionQueue.Count == 1)
        {
            
            buildProduction = productionQueue[0].GetComponent<IProduct>().GetBuildCost();
            turnsLeft = Mathf.CeilToInt((buildProduction - buildingProgress) / (float)cityResouces.production);
        }
        UpdateUI();
    }
    void UpdateUI()
    {
        text_cityName.text = cityName;
        panelColor.color = owner.color - new Color(0,0,0,0.45f);
        text_pop.text = population.ToString();

        if (productionQueue.Count == 0)
        {
            constructionImage.sprite = gearImage;
            constructionTurnLeft.text = "";
            return;
        }
        constructionImage.sprite = productionQueue[0].GetComponent<IProduct>().GetImage();
        constructionTurnLeft.text = turnsLeft.ToString();
    }

    public void RemoveFromQueue(int ID)
    {
        IProduct product = productionQueue[ID].GetComponent<IProduct>();

        switch (product.type()) 
        {
            case 0:
                possibleBuildings.Add(productionQueue[ID]);
                
                break;

            case 2:
                int newID = 0;

                for(int i = 0; i < ID; i++)
                {
                    if (product.type() == 2)
                        newID++;
                }
                buildLocations[newID].AbortConstruction();
                buildLocations.RemoveAt(newID);
                break;
        }

        productionQueue.RemoveAt(ID);

        if (ID == 0 && productionQueue.Count != 0)
        {
            buildProduction = productionQueue[0].GetComponent<IProduct>().GetBuildCost();
            turnsLeft = Mathf.CeilToInt((buildProduction - buildingProgress) / (float)cityResouces.production);
        }

        UpdateUI();
        
    }
    public void StartNextTurn()
    {
        UpdatePopulation();

        if (productionQueue.Count == 0)
        {
            UpdateUI();
            return;
        }
            

        int temp = cityResouces.production;

        buildingProgress += temp;

        if(buildingProgress >= buildProduction)
        {
            ConstructionComplete();
        }
        turnsLeft = Mathf.CeilToInt((buildProduction - buildingProgress) / (float)cityResouces.production);
        UpdateUI();
    }

    public void UpdatePopulation()
    {
        popProgress += cityResouces.food;

        if(popProgress >= popProgressMax)
        {
            GrowPopulation(1);
            popProgress = 0;
        }
        else if(popProgress <= -popProgressMax)
        {
            GrowPopulation(-1);
            popProgress = 0;
        }
    }

    public void GrowPopulation(int ammount)
    {
        population += ammount;

        cityResouces.food -= (short)(ammount * City.GetPopCost());
    }

    public ResourcesTile GetResources()
    {
        return cityResouces;
    }
    void ConstructionComplete()
    {
        IProduct product = productionQueue[0].GetComponent<IProduct>();
        product.Construct(this);

        switch (product.type())
        {
            case 0:
                buildingsBuild.Add(productionQueue[0]);
                break;
            case 2:
                break;
        }

        buildingProgress -= buildProduction;
        productionQueue.RemoveAt(0);

        if(productionQueue.Count != 0)
        {
            buildProduction = productionQueue[0].GetComponent<IProduct>().GetBuildCost();
        }
    }
   
}
