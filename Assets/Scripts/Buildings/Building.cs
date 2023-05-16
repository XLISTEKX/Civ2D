using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int buildCost;
    public Vector3 resources;
    public bool isTile;
    public Sprite buildingSprite;

    public virtual void build(Tile_City city)
    {
        city.cityResouces += new ResourcesTile((short)resources.x, (short)resources.y, (short)resources.z);
        city.buildingsBuild.Add(gameObject);
        city.possibleBuildings.Remove(gameObject);

    }

}
