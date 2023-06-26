using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCamp: Tile, ITurnCity
{
    public int cooldown = 1;


    int maxCooldown;
    [SerializeField] GameObject[] unitPrefabs;
    
    public virtual void StartNextTurn()
    {
        cooldown--;

        if(cooldown <= 0)
        {
            int t = Random.Range(0, unitPrefabs.Length);
            GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>().spawnUnit(unitPrefabs[t], this, 1);
            cooldown = maxCooldown;
        }
    }

    public ResourcesTile GetResources()
    {
        return new(0, 0, 0, 0);
    }

    public override void initTile(Vector2Int position)
    {
        base.initTile(position);

        maxCooldown = cooldown;
    }

}
