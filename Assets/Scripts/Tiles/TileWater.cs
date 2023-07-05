using UnityEngine;

public class TileWater : Tile
{

    public override void TurnRender(bool turn)
    {

        if (discovered)
        {
            GetComponent<SpriteRenderer>().enabled = turn;
            GetComponent<Animator>().enabled = turn;
        }
        else
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<TileFog>().TurnFog(turn);
        }
    }
}
