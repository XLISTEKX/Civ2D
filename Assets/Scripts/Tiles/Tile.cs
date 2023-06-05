using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Unit unitOnTile;
    public ResourcesTile resources;
    public int[] maxResources = new int[4];
    public int[] minResources = new int[4];
    public Vector2Int position;
    public TileBiom biom;
    public bool block;

    [DoNotSerialize] public Player owner;

    protected bool canClick = true;

    List<GameObject> border = new List<GameObject>();


    public virtual void initTile(Vector2Int position)
    {
        resources = new ResourcesTile(Random.Range(minResources[0], maxResources[0] + 1), Random.Range(minResources[1], maxResources[1] + 1), Random.Range(minResources[2], maxResources[2] + 1), 0);
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";
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
            GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>().selectTile(this);
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

            if (grid.tiles[pos.x, pos.y].owner != owner)
            {
                GameObject temp = Instantiate(grid.borders[i], transform);

                border.Add(temp);

                temp.GetComponent<SpriteRenderer>().color = owner.color;
            }

        }

    }
}
