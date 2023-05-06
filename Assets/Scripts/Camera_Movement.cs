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

            firstTouch = Camera.main.ScreenToWorldPoint(firstTouch);

        }
        if (Input.GetMouseButton(0))
        {

            Vector2 deltaLocation = firstTouch - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);


            transform.position += (Vector3)deltaLocation * movementSpeed;

        }

    }
}
