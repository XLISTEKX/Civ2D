using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Controler : MonoBehaviour
{
    public Tile[,] tiles;

    [SerializeField] List<GameObject> tilePrefabs;
    [SerializeField] Transform grid;
    [SerializeField] int column =10;
    [SerializeField] int row = 5;
    float tileSize = 0.5f;

    public Vector2Int noiseOffset;
    float magnitudeOffset = 10f;
    int biomsCount = 4;

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
        magnitudeBiom = Random.Range(8f, 12f);


        tiles = new Tile[column, row];
        map = generateLandMap(column, row);
        mapBioms = clampLandMass(map);
        mapBioms = generateMapBioms(mapBioms);
        map = addMaps(map, mapBioms);
        createGrid();
    }

    void createGrid()
    {
        Vector2 bottomOffset = new Vector2(tileSize * Mathf.Sqrt(3) * 0.5f, -3 * tileSize * 0.25f);
        Vector2 rightOffset = new Vector2(Mathf.Sqrt(3) * tileSize, 0);

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
                Tile temp = Instantiate(tilePrefabs[map[j,i]], offset + rightOffset * j, tilePrefabs[map[j, i]].transform.rotation).GetComponent<Tile>();
                temp.transform.SetParent(grid);
                temp.initTile(new Vector2Int(j, i));
                //temp.GetComponent<SpriteRenderer>().color = Random.ColorHSV();

                tiles[j, i] = temp;
            }

        }
    }

    int GetBitIDByNoise(int x, int y)
    {
        float perlin = Mathf.PerlinNoise((x + noiseOffset.x) / magnitudeOffset, (y + noiseOffset.y) / magnitudeOffset);
        float perlin_clamped = Mathf.Clamp(perlin, 0f, 1f);

        switch (perlin_clamped)
        {
            case >= 0.5f:
                return 2;
                break;
            case >= 0.4f:
                return 1;
                break;
            case < 0.4f:
                return 0;
                break;
            default:
                return 0;
                break;
        }
        return 0;
         
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
        perlinNoise = Mathf.Clamp(perlinNoise, 0, 1);

        perlinNoise *= biomsCount;

        if(perlinNoise == biomsCount)
        {
            return biomsCount - 1;
        }
        return Mathf.FloorToInt(perlinNoise);
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