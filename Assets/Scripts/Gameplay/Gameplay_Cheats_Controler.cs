using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay_Cheats_Controler : MonoBehaviour
{
    [SerializeField] GameObject knight, city, enemy, cave;
    Gameplay_Controler gameplay_Controler;

    private void Start()
    {
        gameplay_Controler = GetComponent<Gameplay_Controler>();
    }

    public void spawnKnight()
    {
        gameplay_Controler.spawnUnit(knight, gameplay_Controler.selectedTile);
        gameplay_Controler.SelectTile(gameplay_Controler.selectedTile);
    }

    public void spawnCity()
    {
        gameplay_Controler.spawnCity(city, gameplay_Controler.selectedTile);
        gameplay_Controler.SelectTile(gameplay_Controler.selectedTile);
    }

    public void SpawnEnemy()
    {
        gameplay_Controler.spawnUnit(enemy, gameplay_Controler.selectedTile, 1);
        gameplay_Controler.SelectTile(gameplay_Controler.selectedTile);
    }
    public void SpawnCave()
    {
        gameplay_Controler.SpawnCamp(cave, gameplay_Controler.selectedTile, 1);
        gameplay_Controler.SelectTile(gameplay_Controler.selectedTile);
    }
}
