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

    bool canClick = true;

    public virtual void initTile(Vector2Int position)
    {
        resources = new ResourcesTile(Random.Range(minResources[0], maxResources[0] + 1), Random.Range(minResources[1], maxResources[1] + 1), Random.Range(minResources[2], maxResources[2] + 1));
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(canClick) 
            GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>().selectTile(this);
    }


    /*void OnMouseDown()
    {
        clicked = true;

        Invoke("changeClicked", 0.15f);

    }

    private void OnMouseUp()
    {
        if(clicked)
            GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>().selectTile(this);
    }
*/
}
