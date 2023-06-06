using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Gameplay_Controler : MonoBehaviour
{

    public Tile selectedTile;
    [SerializeField] List<Player> players;

    [SerializeField] Grid_Controler grid_Controler;
    [SerializeField] GameObject cheats_panel;
    [SerializeField] UI_Controler uI_Controler;
    [SerializeField] UI_Selected selectUnitUI;
    Color lastColor;

    Tile[] unitMoves;
    bool moving = false;

    public int turn = 0;

    static Vector3Int[] cubeDirections = { new Vector3Int(1, 0, -1), new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1), new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1) };

    public void startNewTurn()
    {
        turn++;
        foreach(Player player in players)
        {
            player.startNextRound();
        }
        uI_Controler.nextTurn(turn);
    }

    public void selectTile(Tile newSelected)
    {

        if (moving)
        {
            cheats_panel.SetActive(false);
            if (unitMoves.Contains(newSelected) && selectedTile != newSelected)
            {
                moveUnit(selectedTile, newSelected);
                selectedTile.GetComponent<SpriteRenderer>().color = lastColor;
            }
            else
            {
                removeInitUnitMove();
                moving = false;
                selectNewTile(newSelected);
            }
        }
        else
        {
            if(!newSelected.block && newSelected.owner == null)
                cheats_panel.SetActive(true);
            selectNewTile(newSelected);
        }
        
    }
    void selectNewTile(Tile newSelected)
    {
        if (selectedTile == newSelected)
        {
            selectedTile.GetComponent<SpriteRenderer>().color = lastColor;
            selectedTile = null;
            cheats_panel.SetActive(false);
            return;
        }
        if (selectedTile != null)
            selectedTile.GetComponent<SpriteRenderer>().color = lastColor;

        selectedTile = newSelected;
        if (newSelected.block || newSelected.owner != null)
            cheats_panel.SetActive(false);
        lastColor = selectedTile.GetComponent<SpriteRenderer>().color;

        selectedTile.GetComponent<SpriteRenderer>().color = Color.red;

        if (selectedTile.unitOnTile != null)
            initUnitMove();
    }
    void initUnitMove()
    {
        selectUnitUI.gameObject.SetActive(true);
        selectUnitUI.updateUI(selectedTile.unitOnTile);

        moving = true;
        unitMoves = findMovesInRange(selectedTile, selectedTile.unitOnTile.movementLeft);

        foreach (Tile tile in unitMoves)
        {
            if (tile.unitOnTile != null)
            {
                if (tile.unitOnTile.owner == players[0])
                {
                    tile.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.6f);
                    continue;
                }
            }

            tile.GetComponent<SpriteRenderer>().color -= new Color(0,0,0, 0.4f);
        }
    }
    void removeInitUnitMove()
    {
        selectUnitUI.gameObject.SetActive(false);

        foreach (Tile tile in unitMoves)
        {
            tile.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
        unitMoves = null;
    }
    void moveUnit(Tile start, Tile destination)
    {

        start.unitOnTile.movementLeft -= distance(axisToCube(start.position), axisToCube(destination.position));
        start.unitOnTile.moveUnit(destination);
        destination.unitOnTile = start.unitOnTile;
        start.unitOnTile = null;
        start.block = false;
        destination.block = true;
        removeInitUnitMove();
        moving = false;

    }

    public Tile[] findTilesInRange(Tile selected, int range)
    {
        List<Tile> returnValues = new List<Tile>();

        Vector3Int cubeLocation = axisToCube(selected.position);

        for (int q = -range; q <= range; q++)
        {
            int r1 = Mathf.Max(-range, -q - range);
            int r2 = Mathf.Min(range, -q + range);

            for (int r = r1; r <= r2; r++)
            {
                int s = -q - r;
                Vector3Int cubeOffset = new Vector3Int(q, r, s) + cubeLocation;
                Vector2Int location = cubeToAxis(cubeOffset);

                if(location.x < 0 || location.x > grid_Controler.column - 1)
                {
                    continue;
                    
                }
                if (location.y < 0 || location.y > grid_Controler.row - 1)
                {
                    continue;
                }

                returnValues.Add(grid_Controler.tiles[location.x, location.y]);
            }

        }
        return returnValues.ToArray();
    }

    public Tile[] findMovesInRange(Tile startTile, int range)
    {
        List<Tile> visitedTiles = new List<Tile>();
        //visitedTiles.Add(startTile);

        List<List<Tile>> fringes = new List<List<Tile>>();
        fringes.Add(new List<Tile> { startTile });

        for(int k = 1; k <= range; k++)
        {
            fringes.Add(new List<Tile>());
            foreach(Tile tile in fringes[k - 1])
            {
                foreach(Tile neighbor in cube_neighbor(tile))
                {
                    if(!visitedTiles.Contains(neighbor) && !neighbor.block)
                    {
                        visitedTiles.Add(neighbor);
                        fringes[k].Add(neighbor);
                    }
                }
            }
        }
        return visitedTiles.ToArray();
    }

    Tile[] cube_neighbor(Tile start)
    {
        List<Tile> returnValues = new List<Tile>();
        Vector3Int cubeLocation = axisToCube(start.position);
        
        foreach(Vector3Int vector in cubeDirections)
        {
            Vector3Int newLocation = cubeLocation + vector;
            Vector2Int output = cubeToAxis(newLocation);

            if (output.x < 0 || output.x > grid_Controler.column - 1)
            {
                continue;

            }
            if (output.y < 0 || output.y > grid_Controler.row - 1)
            {
                continue;
            }

            returnValues.Add(grid_Controler.tiles[output.x, output.y]);
        }
        return returnValues.ToArray();
    }
    public static Vector3Int axisToCube(Vector2Int position)
    {
        int q = position.x - (position.y + (position.y & 1)) / 2;

        return new Vector3Int(q, position.y, -q - position.y);
    }
    public static Vector2Int cubeToAxis(Vector3Int position)
    {
        int x = position.x + (position.y + (position.y & 1)) / 2;
        int y = position.y;
        return new Vector2Int(x,y);
    }

    static int distance(Vector3Int start, Vector3Int end)
    {
        Vector3Int tempVec = start - end;
        return Mathf.Max(Mathf.Abs(tempVec.x), Mathf.Abs(tempVec.y), Mathf.Abs(tempVec.z));
    }


    public Tile[] cubeRing(Tile startTile, int distance)
    {
        List<Tile> returnTiles = new List<Tile>();
        Vector3Int cubeLocation = axisToCube(startTile.position);
        if(distance <= 0)
        {
            returnTiles.Add(startTile);
            return returnTiles.ToArray();
        }
        Vector3Int hex = new Vector3Int(-1, 1, 0) * distance + cubeLocation;
        
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < distance; j++)
            {
                Vector2Int position = cubeToAxis(hex);
                hex += cubeDirections[i];
                if (position.x < 0 || position.x > grid_Controler.column - 1 || position.y < 0 || position.y > grid_Controler.row - 1)
                {
                    continue;
                }
                returnTiles.Add(grid_Controler.tiles[position.x, position.y]);
            }
        }
        return returnTiles.ToArray();

    } 


    public void spawnUnit(GameObject unit, Tile location, int playerID = 0) 
    {
        GameObject temp = Instantiate(unit, location.transform.position, unit.transform.rotation);
        temp.transform.SetParent(location.transform);
        Unit tempUnit = temp.GetComponent<Unit>();
        tempUnit.initUnit(players[playerID]);
        players[playerID].allUnits.Add(tempUnit);
        location.unitOnTile = tempUnit;
        location.block = true;
    }

    public void spawnCity(GameObject city, Tile location, int playerID = 0)
    {
        Tile_City cityTile = Instantiate(city, location.transform.position, city.transform.rotation).GetComponent<Tile_City>();
        cityTile.initCityTile(location.position, players[playerID]);
        cityTile.resources = location.resources;
        players[playerID].allCities.Add(cityTile);

        Tile[] tiles = findTilesInRange(location, 1);
        foreach (Tile tile in tiles)
        {
            cityTile.cityResouces += tile.resources;
            tile.owner = players[playerID];
        }
        tiles = findTilesInRange(location, 2);
        foreach(Tile tile in tiles)
        {
            tile.updateBorderState();
        }
        Destroy(grid_Controler.tiles[location.position.x, location.position.y].gameObject);

        grid_Controler.tiles[location.position.x, location.position.y] = cityTile;

    }

    public void openCity(Tile_City city)
    {
        uI_Controler.openCloseCity(city);
    }
}
