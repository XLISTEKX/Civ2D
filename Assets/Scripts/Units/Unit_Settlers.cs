using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Settlers : Unit
{
    [SerializeField] GameObject city;

    public void createCity()
    {
        Gameplay_Controler gameplay = Gameplay_Controler.GetControler();
        Tile tile = GetComponentInParent<Tile>();
        if (gameplay.isNeighborOwnerTile(tile))
            return;

        

        gameplay.SpawnCity(city, tile, owner.ID);
        
        if(owner.ID == 0)
        {
            gameplay.SelectTile(null);
            owner.allUnits.Remove(this);
            Destroy(gameObject);
        }
        else
        {
            PlayerAI player = owner.GetComponent<PlayerAI>();

            player.toDestroy.Add(this);
        }
    }

}
