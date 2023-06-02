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
    Color lastColor;

    Tile[] unitMoves;
    bool moving = false;

    public int turn = 0;

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
            if(!newSelected.block)
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
        if (newSelected.block)
            cheats_panel.SetActive(false);
        lastColor = selectedTile.GetComponent<SpriteRenderer>().color;

        selectedTile.GetComponent<SpriteRenderer>().color = Color.red;

        if (selectedTile.unitOnTile != null)
            initUnitMove();
    }
    void initUnitMove()
    {
        moving = true;
        unitMoves = findMovesInRange(selectedTile, selectedTile.unitOnTile.movementLeft);

        foreach (Tile tile in unitMoves)
        {
            tile.GetComponent<SpriteRenderer>().color -= new Color(0,0,0, 0.4f);
        }
    }
    void removeInitUnitMove()
    {
        foreach(Tile tile in unitMoves)
        {
            tile.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.4f);
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

                returnValues.Add(grid_Controler.tiles[location.x, location.y]);
            }

        }
        return returnValues.ToArray();
    }

    public Tile[] findMovesInRange(Tile startTile, int range)
    {
        List<Tile> visitedTiles = new List<Tile>();
        visitedTiles.Add(startTile);

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
        Vector3Int[] directions = { new Vector3Int(1,0,-1), new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1), new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1) };
        
        foreach(Vector3Int vector in directions)
        {
            Vector3Int newLocation = cubeLocation + vector;
            Vector2Int output = cubeToAxis(newLocation);

            returnValues.Add(grid_Controler.tiles[output.x, output.y]);
        }
        return returnValues.ToArray();
    }
    Vector3Int axisToCube(Vector2Int position)
    {
        int q = position.x - (position.y + (position.y & 1)) / 2;

        return new Vector3Int(q, position.y, -q - position.y);
    }
    Vector2Int cubeToAxis(Vector3Int position)
    {
        int x = position.x + (position.y + (position.y & 1)) / 2;
        int y = position.y;
        return new Vector2Int(x,y);
    }

    int distance(Vector3Int start, Vector3Int end)
    {
        Vector3Int tempVec = start - end;
        return Mathf.Max(Mathf.Abs(tempVec.x), Mathf.Abs(tempVec.y), Mathf.Abs(tempVec.z));
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

       
        foreach (Tile tile in findTilesInRange(location, 1))
        {
            cityTile.cityResouces += tile.resources;
        }
        Destroy(grid_Controler.tiles[location.position.x, location.position.y].gameObject);

        grid_Controler.tiles[location.position.x, location.position.y] = cityTile;

    }

    public void openCity(Tile_City city)
    {
        uI_Controler.openCloseCity(city);
    }
}
