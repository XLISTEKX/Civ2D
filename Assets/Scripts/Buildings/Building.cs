using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour, IProduct
{
    public int buildCost;
    public Vector4 resources;
    public bool isTile;
    public Sprite buildingSprite;
    public int typeN;

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
        city.cityResouces += new ResourcesTile((short)resources.x, (short)resources.y, (short)resources.z, (short) resources.w);
    }
    public int type()
    {
        return typeN;
    }

}
