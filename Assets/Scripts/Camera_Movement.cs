using UnityEngine;
using UnityEngine.EventSystems;

public class Camera_Movement : MonoBehaviour
{
    public float movementSpeed;

    [SerializeField] Gameplay_Controler gameplay_Controler;

    Vector2 firstTouch;

    RaycastHit2D _hit;
    int _rayLayer = 1 << 6;


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            firstTouch = Input.mousePosition;
            _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,100, _rayLayer);

            firstTouch = Camera.main.ScreenToWorldPoint(firstTouch);
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (_hit.collider != null)
                {
                    gameplay_Controler.selectTile(_hit.collider.GetComponent<Tile>());
                }
            }
               
        }
        if (Input.GetMouseButton(0))
        {

            Vector2 deltaLocation =  firstTouch - (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            

            transform.position += (Vector3) deltaLocation * movementSpeed;

        }
        
    }
}
