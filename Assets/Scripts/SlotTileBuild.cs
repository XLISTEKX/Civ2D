using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotTileBuild : MonoBehaviour, IPointerClickHandler
{
    public Tile tile;
    public UI_City_Controler UIcity;
    public int ID;

    public void OnPointerClick(PointerEventData eventData)
    {
        tile.construction = true;
        UIcity.city.buildLocations.Add(tile);
        UIcity.city.addToQueue(ID, 2);
        UIcity.destroyBuildTiles();
        UIcity.updateUI();
    }
}
