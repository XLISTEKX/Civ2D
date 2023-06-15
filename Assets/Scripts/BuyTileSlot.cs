using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BuyTileSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text cost;
    Tile tile;
    Tile_City city;

    public void initSlot(Tile tile, Tile_City city)
    {

        this.tile = tile;
        this.city = city;
        cost.text = tile.tileCost.ToString();
        cost.color = ColorsInfo.getColorByGold( GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().money, tile.tileCost);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();


        if(player.money >= tile.tileCost)
        {
            player.money -= tile.tileCost;
            Gameplay_Controler gameplay = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
            tile.owner = player;
            tile.updateBorderState();
            foreach (Tile tiles in gameplay.cubeRing(tile, 1))
            {
                tiles.updateBorderState();
            }
            city.cityResouces += tile.resources;
            UI_City_Controler ui_city = GameObject.Find("City_Menu").GetComponent<UI_City_Controler>();

            ui_city.updateBuyTile();
            ui_city.updateUI();
        }
        

    }
}
