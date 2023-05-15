using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int buildCost;
    public ResourcesTile resourcesToAdd;
    public bool isTile;
    public Sprite buildingSprite;

    public virtual void build(Tile_City city)
    {
        city.cityResouces += resourcesToAdd;
        city.buildingsBuild.Add(gameObject);
        city.possibleBuildings.Remove(gameObject);

    }

}
