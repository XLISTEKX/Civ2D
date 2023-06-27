using UnityEngine;

public interface IRenderable
{
    public void TurnRender(bool turn)
    {

    }
}

public interface IProduct
{
    public int GetBuildCost()
    {
        return 0;
    }
    public Sprite GetImage()
    {
        return null;
    }
    public void Construct(Tile_City city)
    {

    }
    public int type()
    {
        return 0;
    }
}


public enum TileBiom
{
    None,
    Ocean,
    Water,
    Desert,
    Woods,
    Plains,
    Snow,
    FireWood,
    Mountain,
}

public interface IDamageable
{
    public void TakeDamage(int damage)
    {

    }
}

public interface ITurnCity
{
    public void StartNextTurn()
    {

    }

    public ResourcesTile GetResources()
    {
        return null;
    }
}

