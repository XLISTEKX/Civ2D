using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IRenderable
{
    [Header("Tile Properties")]
    public Vector2Int position;
    public TileBiom biom;
    public Unit unitOnTile;

    [HideInInspector] public ResourcesTile resources;
    [HideInInspector] public int tileCost;

    [Header("Tile Settings")]
    public int[] maxResources = new int[4];
    public int[] minResources = new int[4];
    
    public bool block;

    public bool discovered;
    public bool visiblity;

    [DoNotSerialize] public Player owner;

    protected bool canClick = true;
    GameObject[] border;
    public List<ISeeable> seeUnits = new();
    public virtual int getType()
    {
        return 0;
    }

    public virtual void InitTile(Vector2Int position)
    {
        resources = new ResourcesTile(Random.Range(minResources[0], maxResources[0] + 1), Random.Range(minResources[1], maxResources[1] + 1), Random.Range(minResources[2], maxResources[2] + 1), Random.Range(minResources[3], maxResources[3] + 1));
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";

        tileCost = ResourcesTile.CalcTileValue(resources);

        maxResources = null;
        minResources = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canClick = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canClick = true;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if(canClick) 
            GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>().SelectTile(this);
    }


    public void updateBorderState()
    {
        if (owner == null)
            return;

        if(border != null)
        {
            foreach(GameObject bord in border)
            {
                Destroy(bord);
            }
            border = null;
        }
        
        Grid_Controler grid = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Grid_Controler>();

        Vector3Int[] directions = Gameplay_Controler.cubeDirections;
        Vector3Int cubeLocation = Gameplay_Controler.axisToCube(position);
        Vector3Int tempLocation;
        List<GameObject> tempList = new();
        for(int i = 0; i < 6; i++)
        {
            tempLocation = cubeLocation + directions[i];
            Vector2Int pos = Gameplay_Controler.cubeToAxis(tempLocation);

            if(pos.x < 0 || pos.x > grid.column - 1 || pos.y < 0 || pos.y > grid.row - 1)
            {
                GameObject temp = Instantiate(grid.borders[i], transform);

                tempList.Add(temp);

                temp.GetComponent<SpriteRenderer>().color = owner.color;
                continue;
            }



            if (grid.tiles[pos.x, pos.y].owner != owner)
            {
                GameObject temp = Instantiate(grid.borders[i], transform);

                tempList.Add(temp);

                temp.GetComponent<SpriteRenderer>().color = owner.color;
            }

        }
        border = tempList.ToArray();
    }

    public virtual void TurnRender(bool turn)
    {
        if (discovered)
        {
            GetComponent<SpriteRenderer>().enabled = turn;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<TileFog>().TurnFog(turn);
        }
        
    }

    public void DiscoverTile()
    {
        if (discovered)
            return;

        discovered = true;
        GetComponent<TileFog>().UnCoverFog();
        TurnRender(true);
    }

    public virtual void TurnVisibility(bool turn)
    {

        if (turn)
        {
            if (!discovered)
                DiscoverTile();
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f);
        }
        visiblity = turn;

        if (unitOnTile != null)
            unitOnTile.TurnVisibility(turn);
    }

    public virtual void TurnMoveTile(bool turn)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        if (visiblity)
        {
            if (turn)
            {
                renderer.color -= new Color(0, 0, 0, 0.4f);
            }
            else
            {
                renderer.color = Color.white;
            }
        }
        else
        {
            if (turn)
            {
                renderer.color -= new Color(0, 0, 0, 0.4f);
            }
            else
            {
                renderer.color = new Color(0.6f, 0.6f, 0.6f);
            }
        }
        
    }

    public void UpdateSelectColor(bool select)
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (select)
        {
            renderer.color = new Color(1, 0, 0, 0.7f);
        }
        else
        {
            if (visiblity)
            {
                renderer.color = Color.white;
            }
            else
            {
                renderer.color = new Color(0.6f, 0.6f, 0.6f);
            }
        }
    }
    /*public void changeVisibility(bool hide)
    {
        if (!hide && hidden)
        {

            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            hidden = false;

        }
        else if(hide && !hidden)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.825f, 0.825f, 0.825f);
            hidden = true;
        }
    }*/
}
