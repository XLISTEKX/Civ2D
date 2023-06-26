using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid_Controler : MonoBehaviour
{
    public Tile[,] tiles;
    [Header("Special Tiles")]
    [SerializeField] GameObject camp;
    [Space(20)]
    [Header("Tiles: ")]
    [SerializeField] GameObject tileOcean;
    [SerializeField] GameObject tileWater;
    [SerializeField] GameObject[] tileDesert;
    [SerializeField] GameObject[] tilePlains;
    [SerializeField] GameObject[] tileWoods;
    [SerializeField] GameObject[] tileFireWoods;
    [SerializeField] GameObject[] tileSnow;
    [SerializeField] GameObject[] tileMountain;
    [SerializeField] GameObject[] tileSpecialDesert;
    [SerializeField] GameObject[] tileSpecialPlains;
    [Space(10)]
    [Header("Map Generation Settings")]
    [SerializeField] [Range(0, 1)] float mountainsRate;
    [SerializeField] [Range(0, 1)] float campsRate;
    [SerializeField] int maxMountainsTiles;
    [Space(10)]
    public GameObject[] borders;
    [Header("Settings: ")]
    [SerializeField] Transform grid;
    public int column = 10;
    public int row = 5;

    [HideInInspector]public Tile[] landTiles;
    List<Vector2Int> positions;

    public Vector2Int noiseOffset;
    float magnitudeOffset = 10f;
    int biomsCount = 5;


    Vector2 biomsOffset;
    float magnitudeBiom;

    int[,] mapBioms;
    int[,] map;

    void Start()
    {
        generateNewMap();

    }

    public void generateNewMap()
    {
        if (tiles != null)
        {
            foreach(Tile tile in tiles)
            {
                Destroy(tile.gameObject);
            }
            tiles = null;
        }
        noiseOffset = new Vector2Int(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
        magnitudeOffset = Random.Range(7f, 9f);
        biomsOffset = new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
        magnitudeBiom = Random.Range(40f, 45f);


        tiles = new Tile[column, row];
        map = generateLandMap(column, row);
        mapBioms = clampLandMass(map);
        mapBioms = generateMapBioms(mapBioms);
        map = addMaps(map, mapBioms);
        createGrid();

        List<Tile> tilesPH = new();
        foreach(Vector2Int pos in positions)
        {
            tilesPH.Add(GetTileByCordinates(pos));
        }
        landTiles = tilesPH.ToArray();
        positions = null;

        SpawnCamps();
        SpawnMountains();
    }

    void createGrid()
    {
        Vector2 bottomOffset = new Vector2(0.5f, -15f/ 32);
        Vector2 rightOffset = new Vector2(33 / 32, 0);

        Vector2 offset = new Vector2();
        for (int i = 0; i < row; i++)
        {
            if (i % 2 == 0)
            {
                offset.x += bottomOffset.x;
                offset.y += bottomOffset.y;
            }
            else
            {
                offset.x += -bottomOffset.x;
                offset.y += bottomOffset.y;
            }
            if (i == 0)
            {
                offset = new Vector2(0, 0);
            }

            for (int j = 0; j < column; j++)
            {
                GameObject spawn = getBiomVariant(map[j, i]);
                Tile temp = Instantiate(spawn, offset + rightOffset * j, spawn.transform.rotation).GetComponent<Tile>();
                temp.GetComponent<SpriteRenderer>().enabled = false;
                temp.transform.SetParent(grid);
                temp.initTile(new Vector2Int(j, i));
                

                tiles[j, i] = temp;
            }

        }
    }

    void SpawnCamps()
    {
        int allTiles = column * row;
        int tilesCount = Mathf.FloorToInt(campsRate * allTiles);

        Gameplay_Controler gameplay = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();


        for(int i = 0; i < tilesCount; i++)
        {
            Tile location = landTiles[Random.Range(0, landTiles.Length)];

            gameplay.SpawnCamp(camp, location);
        }
    }

    void SpawnMountains()
    {
        Gameplay_Controler gameplay = Gameplay_Controler.GetControler();
        int mountainsCount = Mathf.FloorToInt(row * column * mountainsRate);
        List<Vector2Int> fringle = new();

        for(int i = 0; i < mountainsCount; i++) 
        {
            int mountainsTileCount = Random.Range(1, maxMountainsTiles);
            fringle.Add(landTiles[Random.Range(0, landTiles.Length)].position);

            for (int k = 0; k < mountainsTileCount; k++)
            {
                List<Vector3Int> directions = Gameplay_Controler.cubeDirections.ToList();
                Vector3Int startPoint = Gameplay_Controler.axisToCube(fringle[^1]);
                for (int j = 0; j < 6; j++)
                {
                    Vector3Int direction = directions[Random.Range(0, directions.Count)];
                    Vector2Int destination = Gameplay_Controler.cubeToAxis(direction + startPoint);

                    if(destination.x < 0 || destination.x > column || destination.y < 0 || destination.y > row)
                    {
                        directions.Remove(direction);
                        continue;
                    }

                    Tile returnTile = tiles[destination.x, destination.y];
                    if (returnTile.biom != TileBiom.Water && !fringle.Contains(returnTile.position))
                    {
                        fringle.Add(returnTile.position);
                        gameplay.SpawnTile(tileMountain[Random.Range(0, tileMountain.Length)], returnTile);
                        break;
                    }
                    directions.Remove(direction);
                }
            }
        }

    }

    GameObject getBiomVariant(int Biom)
    {
        int temp;
        switch(Biom)
        {
            case -1:
                return null;

            case 0:
                return tileOcean;
            case 1:
                return tileWater;
            case 2:
                if(Random.value > 0.98f)
                {
                    temp = Random.Range(0, tileSpecialDesert.Length);
                    return tileSpecialDesert[temp];
                }
                else
                {
                    temp = Random.Range(0, tileDesert.Length);
                    return tileDesert[temp];
                }
            case 3:
                if(Random.value > 0.995f)
                {
                    temp = Random.Range(0, tileSpecialPlains.Length);
                    return tileSpecialPlains[temp];
                }
                temp = Random.Range(0, tilePlains.Length);
                return tilePlains[temp];

            case 4:
                temp = Random.Range(0, tileWoods.Length);
                return tileWoods[temp];
            case 5:
                temp = Random.Range(0, tileFireWoods.Length);
                return tileFireWoods[temp];

            case 6:
                temp = Random.Range(0, tileSnow.Length);
                return tileSnow[temp];


            default:
                return null;
        }
    }

    int GetBitIDByNoise(int x, int y)
    {
        float perlin = Mathf.PerlinNoise((x + noiseOffset.x) / magnitudeOffset, (y + noiseOffset.y) / magnitudeOffset);
        float perlin_clamped = Mathf.Clamp(perlin, 0f, 1f);

        switch (perlin_clamped)
        {
            case >= 0.45f:
                return 2;

            case >= 0.40f:
                return 1;

            case < 0.4f:
                return 0;

            default:
                return 0;
        }
         
    }

    int[,] generateLandMap(int col, int row)
    {
        int[,] returnMap = new int[col, row];

        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                returnMap[j, i] = GetBitIDByNoise(j, i);
            }
        }
        return returnMap;
    }

    int[,] clampLandMass(int[,] LandMap)
    {
        int[,] returnMap = new int[column,row];
        positions = new();

        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                int tempValue = Mathf.Clamp(LandMap[j, i] - 1, 0, 1);

                if (tempValue == 1)
                    positions.Add(new(j, i));

                returnMap[j,i] = tempValue;
            }
            
        }
        return returnMap;
    }

    Tile GetTileByCordinates(Vector2Int position)
    {
        return tiles[position.x, position.y];
    }

    int[,] generateMapBioms(int[,] mapClamped)
    {
        int[,] returnMap = mapClamped;

        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                if (returnMap[j, i] == 0)
                    continue;
                else
                    returnMap[j, i] = GetBiomByNoise(j, i);

            }
        }
        return returnMap;
    }

    int GetBiomByNoise(int x, int y)
    {
        float perlinNoise = Mathf.PerlinNoise((x + biomsOffset.x) / magnitudeBiom, (y + biomsOffset.y) / magnitudeBiom);
        perlinNoise -= Random.Range(0, 0.125f);
        perlinNoise = Mathf.Clamp(perlinNoise, 0, 1);

        perlinNoise *= biomsCount;

        switch(perlinNoise)
        {                       // 0 - Desert; 1 - Plains; 2 - Woods; 3 - FireWoods; 4 - Snow;
            case <= 0.75f:
                return 0;
            case <= 2f:
                return 1;
            case <= 2.75f:
                return 2;
            case <= 3f:
                return 1;
            case <= 4f:
                return 3;
            case <= 5f:
                return 4;
        }
        return 0;
    }

    int[,] addMaps(int[,] item1, int[,] item2)
    {
        int[,] returnMap = new int[column, row];
        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                returnMap[j, i] = item1[j, i] + item2[j, i];
            }
        }
        return returnMap;
    }

}