using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Settlers : Unit
{
    [SerializeField] GameObject city;

    public void createCity()
    {
        Gameplay_Controler gameplay = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();

        if (gameplay.selectedTile.owner != null)
            return;

        gameplay.spawnCity(city, gameplay.selectedTile, owner.ID);
        gameplay.selectTile(null);

        Destroy(gameObject);
    }
}
