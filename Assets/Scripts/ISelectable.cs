using UnityEngine;

public interface ISelectable
{
    public void select()
    {

    }
}

public interface IProduct
{
    public int getBuildCost()
    {
        return 0;
    }
    public Sprite getImage()
    {
        return null;
    }
    public void construct(Tile_City city)
    {

    }
    public int type()
    {
        return 0;
    }
}


public enum TileBiom
{
    Ocean,
    Water,
    Desert,
    Woods,
    Plains,
    Mountain,
}

