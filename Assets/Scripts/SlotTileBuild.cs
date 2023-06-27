using UnityEngine;
using UnityEngine.EventSystems;

public class SlotTileBuild : MonoBehaviour, IPointerClickHandler
{
    public Tile tile;
    public UI_City_Controler UIcity;
    public int ID;

    [SerializeField] GameObject prefabConstruction;

    public void OnPointerClick(PointerEventData eventData)
    {
        TileConstruction construction = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>().spawnCityTile(prefabConstruction, tile, UIcity.city, false).GetComponent<TileConstruction>();
        construction.InitConstruction(tile.position, tile);

        tile.gameObject.SetActive(false);

        UIcity.city.buildLocations.Add(construction);
        UIcity.city.AddToQueue(ID, 2);
        UIcity.destroyBuildTiles();
        UIcity.updateUI();
    }
}
