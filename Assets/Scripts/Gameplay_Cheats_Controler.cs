using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay_Cheats_Controler : MonoBehaviour
{
    public GameObject knight;
    public GameObject city;
    Gameplay_Controler gameplay_Controler;

    private void Start()
    {
        gameplay_Controler = GetComponent<Gameplay_Controler>();
    }

    public void spawnKnight()
    {
        gameplay_Controler.spawnUnit(knight, gameplay_Controler.selectedTile);
        gameplay_Controler.selectTile(gameplay_Controler.selectedTile);
    }

    public void spawnCity()
    {
        gameplay_Controler.spawnCity(city, gameplay_Controler.selectedTile);
        gameplay_Controler.selectTile(gameplay_Controler.selectedTile);
    }
}
