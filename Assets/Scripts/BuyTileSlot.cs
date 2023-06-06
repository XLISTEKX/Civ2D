using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BuyTileSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text cost;
    Tile tile;

    public void initSlot(Tile tile)
    {
        this.tile = tile;

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
            GameObject.Find("City_Menu").GetComponent<UI_City_Controler>().updateBuyTile();
        }
        

    }
}
