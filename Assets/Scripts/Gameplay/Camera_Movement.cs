using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Camera_Movement : MonoBehaviour
{
    public float movementSpeed;

    [SerializeField] Gameplay_Controler gameplay_Controler;
    GraphicRaycaster raycaster;
    List<RaycastResult> results = new List<RaycastResult>();
    PointerEventData eventData;

    Vector2 firstTouch;

    bool move = false;


    private void Start()
    {
        raycaster = GameObject.FindGameObjectWithTag("UI").GetComponent<GraphicRaycaster>();
        eventData = new PointerEventData(EventSystem.current);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            firstTouch = Input.mousePosition;
            eventData.position = firstTouch;
            firstTouch = Camera.main.ScreenToWorldPoint(firstTouch);

            results.Clear();

            raycaster.Raycast(eventData, results);

            if(results.Count != 0)
            {
                move = false;
            }
            else
            {
                move = true;
            }
            
        }
        if (Input.GetMouseButton(0) && move)
        {
            
            Vector2 deltaLocation = firstTouch - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);


            transform.position += (Vector3)deltaLocation * movementSpeed;

        }

    }
}
