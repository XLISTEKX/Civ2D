using UnityEngine;

public class Tile : MonoBehaviour
{
    public Unit unitOnTile;

    public Vector2Int position;

    public TileBiom biom;
    public bool block; 

    bool clicked = false;

    public void initTile(Vector2Int position)
    {
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";
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
    void changeClicked()
    {
        clicked = false;
    }
}
