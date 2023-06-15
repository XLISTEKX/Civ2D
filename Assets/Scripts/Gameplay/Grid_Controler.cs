using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Controler : MonoBehaviour
{
    public Tile[,] tiles;

    [Header("Tiles: ")]
    [SerializeField] GameObject tileOcean;
    [SerializeField] GameObject tileWater;
    [SerializeField] List<GameObject> tileDesert;
    [SerializeField] List<GameObject> tilePlains;
    [SerializeField] List<GameObject> tileWoods;
    [SerializeField] List<GameObject> tileFireWoods;
    [SerializeField] List<GameObject> tileMountain;
    [SerializeField] List<GameObject> tileSpecialDesert;
    [SerializeField] List<GameObject> tileSpecialPlains;

    public GameObject[] borders;
    [Header("Settings: ")]
    [SerializeField] Transform grid;
    public int column =10;
    public int row = 5;


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
        magnitudeOffset = Random.Range(5f, 7f);
        biomsOffset = new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
        magnitudeBiom = Random.Range(20f, 22f);


        tiles = new Tile[column, row];
        map = generateLandMap(column, row);
        mapBioms = clampLandMass(map);
        mapBioms = generateMapBioms(mapBioms);
        map = addMaps(map, mapBioms);
        createGrid();
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
                temp.transform.SetParent(grid);
                temp.initTile(new Vector2Int(j, i));
                

                tiles[j, i] = temp;
            }

        }
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
                    temp = Random.Range(0, tileSpecialDesert.Count);
                    return tileSpecialDesert[temp];
                }
                else
                {
                    temp = Random.Range(0, tileDesert.Count);
                    return tileDesert[temp];
                }
            case 3:
                if(Random.value > 0.995f)
                {
                    temp = Random.Range(0, tileSpecialPlains.Count);
                    return tileSpecialPlains[temp];
                }
                temp = Random.Range(0, tilePlains.Count);
                return tilePlains[temp];

            case 4:
                temp = Random.Range(0, tileWoods.Count);
                return tileWoods[temp];
            case 5:
                temp = Random.Range(0, tileFireWoods.Count);
                return tileFireWoods[temp];

            case 6:
                temp = Random.Range(0, tileMountain.Count);
                return tileMountain[temp];
        }
        return null;
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

        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                returnMap[j,i] = Mathf.Clamp((LandMap[j,i] - 1), 0, 1);
            }
            
        }
        return returnMap;
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
        {                       // 0 - Desert; 1 - Plains; 2 - Woods; 3 - FireWoods; 4 - Mountains;
            case <= 0.75f:
                return 0;
            case <= 2f:
                return 1;
            case <= 2.75f:
                return 2;
            case <= 3f:
                return 1;
            case <= 4.25f:
                return 3;
            case <= 5:
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