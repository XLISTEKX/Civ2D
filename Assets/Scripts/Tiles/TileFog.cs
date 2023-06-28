using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFog : MonoBehaviour
{
    public GameObject fogTile;
    GameObject tile;

    public void SpawnFog()
    {
        tile = Instantiate(fogTile, transform);
    }

    public void TurnFog(bool turn)
    {
        tile.SetActive(turn);
    }

    public void UnCoverFog()
    {
        Destroy(tile);
        Destroy(this);
    }
}
