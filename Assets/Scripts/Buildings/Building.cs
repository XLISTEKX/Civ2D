using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour, IProduct
{
    public int buildCost;
    public Vector3 resources;
    public bool isTile;
    public Sprite buildingSprite;

    public int getBuildCost()
    {
        return buildCost;
    }
    public Sprite getImage()
    {
        return buildingSprite;
    }
    public virtual void construct(Tile_City city)
    {
        city.cityResouces += new ResourcesTile((short)resources.x, (short)resources.y, (short)resources.z);
    }
    public int type()
    {
        return 0;
    }

}
