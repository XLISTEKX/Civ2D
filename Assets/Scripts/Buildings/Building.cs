using Unity.VisualScripting;
using UnityEngine;

public class Building : MonoBehaviour, IProduct
{
    public int buildCost;
    public Vector4 resources;
    public bool isTile;
    public Sprite buildingSprite;
    public int typeN;

    public int GetBuildCost()
    {
        return buildCost;
    }
    public Sprite GetImage()
    {
        return buildingSprite;
    }
    public virtual void Construct(Tile_City city)
    {
        city.cityResouces += new ResourcesTile((short)resources.x, (short)resources.y, (short)resources.z, (short) resources.w);
    }
    public int type()
    {
        return typeN;
    }

}
