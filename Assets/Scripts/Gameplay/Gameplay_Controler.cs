using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using XGeneric;
using XGeneric.Collections;

public class Gameplay_Controler : MonoBehaviour
{

    public Tile selectedTile;
    [SerializeField] List<Player> players;

    [SerializeField] Grid_Controler grid_Controler;
    [SerializeField] GameObject cheats_panel;
    [SerializeField] UI_Controler uI_Controler;
    [SerializeField] UI_Selected selectUnitUI;
    [SerializeField] GameObject settler;
    Tile[] unitMoves;

    public bool canClick = true;
    public int turn = 0;

    public static Vector3Int[] cubeDirections = { new Vector3Int(1, 0, -1), new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1), new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1) };


    public static Gameplay_Controler GetControler()
    {
        return GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
    }

    public void InitPlayer(int playerID)
    {
        int randX = grid_Controler.column;
        int randY = grid_Controler.row;

        Tile tile = null;
        while(tile == null)
        {
            int x = Random.Range(0, randX);
            int y = Random.Range(0, randY);

            tile = grid_Controler.tiles[x, y];

            if (tile.biom == TileBiom.Water)
                tile = null;
        }

        SpawnUnit(settler, tile, playerID);
        Camera.main.transform.position += new Vector3(tile.transform.position.x, tile.transform.position.y);
    }

    public void startNewTurn()
    {
        Tile lastSelected = selectedTile;

        turn++;
        SelectTile(null);
        foreach (Player player in players)
        {
            player.StartNextRound();
        }
        uI_Controler.nextTurn(turn);
        SelectTile(lastSelected);
    }

    public void SelectTile(Tile newSelected)
    {
        if (!canClick)
            return;
        cheats_panel.SetActive(false);

        if(selectedTile != null)
        {
            if (selectedTile.getType() == 1)
            {
                if (selectedTile.unitOnTile != null && selectedTile.unitOnTile.owner == players[0] && newSelected == selectedTile)
                {
                    if (unitMoves != null)
                    {
                        RemoveInitUnitMove();
                    }
                    else
                    {
                        InitUnitMove();
                    }
                    uI_Controler.openCloseCity(selectedTile.GetComponent<Tile_City>());
                    
                }
                else
                {
                    if(selectedTile.unitOnTile != null && selectedTile.unitOnTile.owner == players[0])
                    {
                        if (unitMoves == null)
                        {
                            uI_Controler.openCloseCity(selectedTile.GetComponent<Tile_City>());
                            selectedTile = null;
                            return;
                        }
                        if (unitMoves.Contains(newSelected))
                        {
                            if(MoveUnit(selectedTile, newSelected))
                            {
                                RemoveInitUnitMove();
                                selectedTile = null;
                                SelectNewTile(newSelected);
                            }
                            else
                            {
                                RemoveInitUnitMove();
                                InitUnitMove();
                            }
                            
                        }
                        else
                        {
                            RemoveInitUnitMove();
                            SelectNewTile(newSelected);
                        }
                        
                        return;
                    }
                    if (unitMoves != null)
                        RemoveInitUnitMove();

                    uI_Controler.openCloseCity(selectedTile.GetComponent<Tile_City>());
                    selectedTile = null;
                }
                
                return;
            }

            if (selectedTile == newSelected)
            {
                selectedTile.UpdateSelectColor(false);

                if (unitMoves != null)
                    RemoveInitUnitMove();

                selectedTile = null;
                return;
            }

            if (selectedTile.unitOnTile != null && selectedTile.unitOnTile.owner == players[0])
            {
                if (unitMoves.Contains(newSelected))
                {
                    if(MoveUnit(selectedTile, newSelected))
                    {
                        RemoveInitUnitMove();
                        selectedTile.UpdateSelectColor(false); ;
                        SelectNewTile(newSelected);
                    }
                    else
                    {
                        RemoveInitUnitMove();
                        InitUnitMove();
                    }
                    return;
                }
                RemoveInitUnitMove();
                SelectNewTile(newSelected);
                return;
            }
        }
        SelectNewTile(newSelected);
        
    }
    void SelectNewTile(Tile newSelected)
    {
        if (selectedTile != null)
        {
            selectedTile.UpdateSelectColor(false);
        }
            

        selectedTile = newSelected;

        if (newSelected == null)
            return;
        
        if (selectedTile.getType() == 1)
        {
            uI_Controler.openCloseCity(selectedTile.GetComponent<Tile_City>());
            return;
        }
        if (!newSelected.block && newSelected.owner == null && newSelected.unitOnTile == null)
            cheats_panel.SetActive(true);

        selectedTile.UpdateSelectColor(true);

        if (selectedTile.unitOnTile != null && selectedTile.unitOnTile.owner == players[0])
            InitUnitMove();
        
    }
    void InitUnitMove()
    {
        selectUnitUI.gameObject.SetActive(true);
        selectUnitUI.updateUI(selectedTile.unitOnTile);

        unitMoves = findMovesInRange(selectedTile, selectedTile.unitOnTile.movementLeft);
        
        if(selectedTile.unitOnTile.canAttack)
        {
            Tile[] temp = GetUnitsInRange(selectedTile.unitOnTile);

            if (temp != null)
                unitMoves = unitMoves.Concat(temp).ToArray();
        }

        foreach (Tile tile in unitMoves)
        {
            SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();

            if (tile.unitOnTile != null)
            {
                if (tile.unitOnTile.owner.ID != 0)
                {
                    if(tile.visiblity)
                        renderer.color = new Color(1, 0, 0, 0.9f);
                    else
                        tile.TurnMoveTile(true);
                }
                continue;
            }

            tile.TurnMoveTile(true);
        }
    }
    void RemoveInitUnitMove()
    {
        selectUnitUI.gameObject.SetActive(false);

        foreach (Tile tile in unitMoves)
        {
            tile.TurnMoveTile(false);
        }
        unitMoves = null;
    }

    public static bool MoveUnit(Tile start, Tile destination) 
    {
        if (start == destination)
            return false;
        Gameplay_Controler gameplay = GetControler();
        Tile[] path = gameplay.GetPathTiles(start, destination);
        Unit firstUnit = start.unitOnTile;
        int distanceU = Distance(axisToCube(start.position), axisToCube(destination.position));

        if(destination.unitOnTile != null && distanceU <= firstUnit.attackRange && destination.visiblity)
        {
            destination.unitOnTile.TakeDamage(firstUnit.damage);
            firstUnit.movementLeft = 0;
            firstUnit.canAttack = false;
            return false;
        }

        Tile last = start;

        foreach(Tile des in path)
        {
            if (des.unitOnTile != null)
            {
                if (des.visiblity)
                {
                    des.unitOnTile.TakeDamage(firstUnit.damage);
                    last.unitOnTile.movementLeft = 0;
                    last.unitOnTile.canAttack = false;
                }
                return false;
            }

            last.unitOnTile.movementLeft -= 1;
            last.unitOnTile.MoveUnit(des);
            des.unitOnTile = firstUnit;
            last.unitOnTile = null;
            last = des;
            if (des.unitOnTile.owner.ID == 0)
                gameplay.DiscoverTiles(des, des.unitOnTile.viewRange);
        }

        return true;
    }

    Tile[] GetUnitsInRange(Unit unit)
    {

        if(unit.movementLeft >= unit.attackRange)
            return null;

        Tile startTile = unit.GetComponentInParent<Tile>();
        List<Tile> returns = new();

        for (int i = unit.movementLeft; i <= unit.attackRange; i++)
        {
            foreach(Tile tile in CubeRing(startTile, i))
            {
                if(tile.unitOnTile != null)
                {
                    if (tile.unitOnTile.owner != unit.owner)
                        returns.Add(tile);
                }
            }
        }

        return returns.ToArray();
    }

    public bool isNeighborOwnerTile(Tile start)
    {
        foreach(Tile neighbor in cube_neighbor(start))
        {
            if (neighbor.owner != null)
            {
                return true;
            }
        }
        return false;
    }

    public Tile[] FindTilesInRange(Tile selected, int range)
    {
        List<Tile> returnValues = new ();

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
        List<Tile> visitedTiles = new ();
        //visitedTiles.Add(startTile);

        List<List<Tile>> fringes = new();
        fringes.Add(new List<Tile> { startTile });

        for(int k = 1; k <= range; k++)
        {
            fringes.Add(new List<Tile>());
            foreach(Tile tile in fringes[k - 1])
            {
                foreach(Tile neighbor in cube_neighbor(tile))
                {
                    if(!visitedTiles.Contains(neighbor) && !neighbor.block && neighbor.biom != TileBiom.Water)
                    {
                        if(neighbor.unitOnTile != null)
                        {
                            if(neighbor.unitOnTile.owner.ID != 0)
                            {
                                visitedTiles.Add(neighbor);
                                fringes[k].Add(neighbor);
                            }
                            continue;
                        }
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

    static int Distance(Vector3Int start, Vector3Int end)
    {
        Vector3Int tempVec = start - end;
        return Mathf.Max(Mathf.Abs(tempVec.x), Mathf.Abs(tempVec.y), Mathf.Abs(tempVec.z));
    }


    public Tile[] CubeRing(Tile startTile, int distance)
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


    public GameObject SpawnUnit(GameObject unit, Tile location, int playerID = 0) 
    {
        GameObject temp = Instantiate(unit, location.transform.position, unit.transform.rotation);
        temp.transform.SetParent(location.transform);
        Unit tempUnit = temp.GetComponent<Unit>();
        tempUnit.InitUnit(players[playerID]);
        players[playerID].allUnits.Add(tempUnit);
        location.unitOnTile = tempUnit;

        if(playerID == 0)
            DiscoverTiles(location, 2);

        return temp;
    }
    public Tile SpawnTile(GameObject tile, Tile location)
    {
        Tile tiles = Instantiate(tile, location.transform.position, tile.transform.rotation).GetComponent<Tile>();
        grid_Controler.tiles[location.position.x, location.position.y] = tiles;
        tiles.transform.SetParent(GameObject.Find("Grid").transform);
        tiles.InitTile(location.position);
        Destroy(location.gameObject);

        return tiles;
    }

    public TileResource SpawnTileResource(GameObject tile, Tile location)
    {
        TileResource tiles = Instantiate(tile, location.transform.position, tile.transform.rotation).GetComponent<TileResource>();
        grid_Controler.tiles[location.position.x, location.position.y] = tiles;
        tiles.transform.SetParent(GameObject.Find("Grid").transform);
        tiles.InitTile(location.position);
        Destroy(location.gameObject);

        return tiles;
    }

    public void SpawnCity(GameObject city, Tile location, int playerID = 0)
    {
        Tile_City cityTile = Instantiate(city, location.transform.position, city.transform.rotation).GetComponent<Tile_City>();
        cityTile.InitCityTile(location.position, players[playerID]);
        cityTile.transform.SetParent(GameObject.Find("Grid").transform);

        cityTile.resources = location.resources;
        players[playerID].allCities.Add(cityTile);

        Tile[] tiles = FindTilesInRange(location, 1);
        foreach (Tile tile in tiles)
        {
            cityTile.cityResouces += tile.resources;
            tile.owner = players[playerID];
            cityTile.cityTiles.Add(tile);
        }
        cityTile.cityTiles.Remove(location);

        tiles = FindTilesInRange(location, 2);
        foreach(Tile tile in tiles)
        {
            tile.updateBorderState();
        }
       
        Destroy(location.gameObject);
        
        grid_Controler.tiles[location.position.x, location.position.y] = cityTile;
        DiscoverTiles(cityTile, 2);
        uI_Controler.updateUI();
        
    }

    public TileCamp SpawnCamp(GameObject camp, Tile location, int playerID = 1)
    {
        TileCamp campTile = Instantiate(camp, location.transform.position, camp.transform.rotation).GetComponent<TileCamp>();
        campTile.InitTile(location.position);
        campTile.transform.SetParent(GameObject.Find("Grid").transform);
        campTile.owner = players[playerID];
        campTile.resources = location.resources;
        players[playerID].gameObject.GetComponent<PlayerNeutral>().camps.Add(campTile);

        Destroy(grid_Controler.tiles[location.position.x, location.position.y].gameObject);

        grid_Controler.tiles[location.position.x, location.position.y] = campTile;
        uI_Controler.updateUI();

        return campTile;
    }

    public void OpenCity(Tile_City city)
    {
        uI_Controler.openCloseCity(city);
    }

    public Tile SpawnCityTile(GameObject tile, Tile location, Tile_City city, bool destroy = true)
    {
        Building_Tile spawn = Instantiate(tile, location.transform.position, tile.transform.rotation).GetComponent<Building_Tile>();
        spawn.transform.SetParent(GameObject.Find("Grid").transform);
        spawn.owner = city.owner;
        spawn.InitTile(location.position);
        

        grid_Controler.tiles[location.position.x, location.position.y] = spawn;

        city.cityResouces += spawn.resources;
        spawn.SetTile(location.resources);

        city.cityTiles.Remove(location);
        city.cityTiles.Add(spawn);
        
        if(destroy)
            Destroy(location.gameObject);

        return spawn;
    }
    public Tile SpawnCityConstructionTile(GameObject tile, Tile location, Tile_City city, bool destroy = true)
    {
        Tile spawn = Instantiate(tile, location.transform.position, tile.transform.rotation).GetComponent<Tile>();
        spawn.transform.SetParent(GameObject.Find("Grid").transform);
        spawn.owner = city.owner;
        spawn.InitTile(location.position);

        grid_Controler.tiles[location.position.x, location.position.y] = spawn;

        city.cityTiles.Remove(location);
        city.cityTiles.Add(spawn);

        if (destroy)
            Destroy(location.gameObject);

        return spawn;
    }

    public Tile[] GetTilesBuyExpanse(Tile tile, int distance)
    {
        List<Tile> returns = new();

        for(int i = 2; i <= distance + 1; i++)
        {
            foreach (Tile exp in CubeRing(tile, i))
            {
                if (exp.owner == null && isNeighborOwnerTile(exp))
                {
                    returns.Add(exp);
                }
                    
            }

        }

        
        return returns.ToArray();
    }
    #region Visibility
    public void DiscoverTiles(Tile start, int range)
    {
        ISeeable see;
        if (start.gameObject.TryGetComponent(out ISeeable seen))
        {
            if(start.unitOnTile == null)
                see = seen;
            else
                see = start.unitOnTile.GetComponent<ISeeable>();
        }
        else
        {
            see = start.unitOnTile.GetComponent<ISeeable>();
        }

        foreach(Tile tile in see.GetTilesInRange())
        {
            tile.seeUnits.Remove(see);
            if(tile.seeUnits.Count > 0)
                continue;
            
            tile.TurnVisibility(false);
        }


        start.TurnVisibility(true);
        Tile[] tilesInRange = FindTilesInRange(start, range);

        foreach (Tile tile in tilesInRange)
        {
            tile.TurnVisibility(true);
            tile.seeUnits.Add(see);
        }
        see.SetTilesInRange(tilesInRange);
    }

    public void DiscoverCityTiles(Tile start, Tile_City city)
    {
        ISeeable seeObject = city.GetComponent<ISeeable>();
        List<Tile> newTiles = new();

        foreach(Tile tile in CubeRing(start, 1))
        {
            if (tile.visiblity)
                continue;
            tile.TurnVisibility(true);
            tile.seeUnits.Add(seeObject);
            newTiles.Add(tile);
        }

        if (newTiles.Count == 0)
            return;
        Tile[] tempTiles = seeObject.GetTilesInRange().Concat(newTiles).ToArray();

        seeObject.SetTilesInRange(tempTiles);
    }

    #endregion
    public bool IsNeighborCityTile(Tile tile)
    {
        foreach(Tile tile1 in cube_neighbor(tile))
        {
            int type = tile1.getType();
            if (type == 1 || type == 2)
                return true;
        }
        return false;
    }
    #region PathFinding 
    public Tile[] GetPathTiles(Tile start, Tile destination)
    {
        PriorityQueue<Tile> front = new();
        front.Enqueue(start, 0);
        Dictionary<Tile, Tile> came_from = new();
        Dictionary<Tile, int> cost_so_far = new();
        Vector3Int destinationPosition = axisToCube(destination.position);
        came_from[start] = null;
        cost_so_far[start] = 0;

        while(front.Count > 0)
        {
            Tile current = front.Dequeue();

            if (current == destination)
                return XGeneric<Tile>.GetTilesByDictionary(came_from, current);

            foreach(Tile next in cube_neighbor(current))
            {
                int new_cost = cost_so_far[current] + 1; // koszt poruszania sie tu
                if(!cost_so_far.ContainsKey(next) || new_cost < cost_so_far[next])
                {
                    cost_so_far[next] = new_cost;
                    int priority = new_cost + Distance(destinationPosition, axisToCube(next.position));
                    front.Enqueue(next, priority);
                    came_from[next] = current;
                }
            }
        }
        return null;
    }
    #endregion
}
