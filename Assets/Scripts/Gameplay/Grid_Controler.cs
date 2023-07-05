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
    [SerializeField] GameObject resourcesWater;
    [SerializeField] GameObject resourcesPlains;
    [SerializeField] GameObject resourcesDesert;
    [SerializeField] GameObject resourcesWoods;
    [SerializeField] GameObject resourcesFireWood;
    [SerializeField] GameObject resourcesSnow;
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
    [SerializeField] [Range(0, 0.2f)] float resourcesRate;
    [SerializeField] [Range(0, 0.2f)] float mountainsRate;
    [SerializeField] [Range(0, 0.2f)] float campsRate;
    [SerializeField] bool discoverTiles;
    [SerializeField] Vector2 magnitudeBioms;
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
        magnitudeBiom = Random.Range(magnitudeBioms.x, magnitudeBioms.y);


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
        SpawnMountains();
        SpawnCamps();
        SpawnResources();

        GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>().InitPlayer(0);
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

                InitTile(temp, new Vector2Int(j, i));
            }

        }
    }

    void InitTile(Tile tile, Vector2Int position, bool init = true)
    {
        if (init)
        {
            tile.transform.SetParent(grid);
            tile.InitTile(position);

            tiles[position.x, position.y] = tile;
        }
        if (discoverTiles)
        {
            tile.discovered = true;
        }
        else
        {
            tile.GetComponent<TileFog>().SpawnFog();
        }
        tile.TurnRender(false);
        tile.TurnVisibility(false);
    }


    void SpawnCamps()
    {
        int allTiles = column * row;
        int tilesCount = Mathf.FloorToInt(campsRate * allTiles);

        Gameplay_Controler gameplay = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();


        for(int i = 0; i < tilesCount; i++)
        {
            int id = Random.Range(0, landTiles.Length);
            Tile location = landTiles[id];

            TileCamp tile = gameplay.SpawnCamp(camp, location);
            InitTile(tile, Vector2Int.zero, false);
            landTiles[id] = tile;
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

                    if(destination.x < 0 || destination.x > column - 1 || destination.y < 0 || destination.y > row - 1)
                    {
                        directions.Remove(direction);
                        continue;
                    }

                    Tile returnTile = tiles[destination.x, destination.y];
                    if (returnTile.biom != TileBiom.Water && !fringle.Contains(returnTile.position))
                    {
                        fringle.Add(returnTile.position);

                        Tile tile = gameplay.SpawnTile(tileMountain[Random.Range(0, tileMountain.Length)], returnTile);
                        InitTile(tile, Vector2Int.zero, false);
                        break;
                    }
                    directions.Remove(direction);
                }
            }
        }

    }

    void SpawnResources()
    {
        int tilesCount = Mathf.FloorToInt(column * row * resourcesRate);

        Gameplay_Controler gameplay = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();

        for (int i = 0; i < tilesCount; i++)
        {
            Vector2Int pos = new(Random.Range(0, column), Random.Range(0, row));
            Tile location = tiles[pos.x, pos.y];

            if(location.biom != TileBiom.Mountain && location.biom != TileBiom.None)
            {
                Tile tile = gameplay.SpawnTileResource(GetResourcePrefabByBiom(location.biom), location);

                InitTile(tile, Vector2Int.zero, false);
                continue;
            }
            i--;
        }
    }


    GameObject GetResourcePrefabByBiom(TileBiom biom)
    {
        return biom switch
        {
            TileBiom.Water => resourcesWater,
            TileBiom.Desert => resourcesDesert,
            TileBiom.Woods => resourcesWoods,
            TileBiom.Plains => resourcesPlains,
            TileBiom.Snow => resourcesSnow,
            TileBiom.FireWood => resourcesFireWood,
            _ => null,
        };
    }

    GameObject getBiomVariant(int Biom)
    {
        int temp;
        switch(Biom)
        {
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

        return perlin_clamped switch
        {
            >= 0.45f => 2,
            >= 0.40f => 1,
            < 0.4f => 0,
            _ => 0,
        };
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

        return perlinNoise switch
        {                       // 0 - Desert; 1 - Plains; 2 - Woods; 3 - FireWoods; 4 - Snow;
            <= 0.85f => 0,
            <= 1.5f => 1,
            <= 2.25f => 2,
            <= 2.50f => 1,
            <= 3.25f => 3,
            <= 5f => 4,
            _ => 0,
        };
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