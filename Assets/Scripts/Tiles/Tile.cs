using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IRenderable
{
    public Unit unitOnTile;
    public ResourcesTile resources;
    public int tileCost;
    public int[] maxResources = new int[4];
    public int[] minResources = new int[4];
    public Vector2Int position;
    public TileBiom biom;
    public bool block;

    [DoNotSerialize] public Player owner;

    protected bool canClick = true;

    List<GameObject> border = new List<GameObject>();

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
            border.Clear();
        }
        
        Grid_Controler grid = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Grid_Controler>();

        Vector3Int[] directions = { new Vector3Int(1, 0, -1), new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1), new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1) };
        Vector3Int cubeLocation = Gameplay_Controler.axisToCube(position);
        Vector3Int tempLocation;

        for(int i = 0; i < 6; i++)
        {
            tempLocation = cubeLocation + directions[i];
            Vector2Int pos = Gameplay_Controler.cubeToAxis(tempLocation);

            if(pos.x < 0 || pos.x > grid.column - 1 || pos.y < 0 || pos.y > grid.row - 1)
            {
                GameObject temp = Instantiate(grid.borders[i], transform);

                border.Add(temp);

                temp.GetComponent<SpriteRenderer>().color = owner.color;
                continue;
            }



            if (grid.tiles[pos.x, pos.y].owner != owner)
            {
                GameObject temp = Instantiate(grid.borders[i], transform);

                border.Add(temp);

                temp.GetComponent<SpriteRenderer>().color = owner.color;
            }

        }

    }

    public virtual void TurnRender(bool turn)
    {
        GetComponent<SpriteRenderer>().enabled = turn;
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
