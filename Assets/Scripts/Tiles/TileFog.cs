using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFog : MonoBehaviour
{
    public GameObject fogTile;

    public void SetFog()
    {
        fogTile = Instantiate(fogTile, transform);
    }

    public void UnCoverFog()
    {
        Destroy(fogTile);
        Destroy(this);
    }
}
