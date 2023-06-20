using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Camera_Movement : MonoBehaviour
{
    public float movementSpeed;
    public float zoomMagnitude;

    public Vector3 boxOffset;

    [SerializeField] Gameplay_Controler gameplay_Controler;
    GraphicRaycaster raycaster;
    List<RaycastResult> results = new List<RaycastResult>();
    PointerEventData eventData;

    Vector2 firstTouch;
    BoxCollider2D boxCollider;

    bool move = false;


    private void Start()
    {
        raycaster = GameObject.FindGameObjectWithTag("UI").GetComponent<GraphicRaycaster>();
        eventData = new PointerEventData(EventSystem.current);
        boxCollider = GetComponent<BoxCollider2D>();

        UpdateColider();
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
        if(Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0Prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1Prev = touch1.position - touch1.deltaPosition;

            float magnitude0 = (touch0Prev - touch1Prev).magnitude;
            float magnitude1 = (touch0.position - touch1.position).magnitude;

            float diffrence = magnitude1 - magnitude0;

            zoom(diffrence * zoomMagnitude);
            move = false;
        }
        else if (Input.GetMouseButton(0) && move)
        {
            
            Vector2 deltaLocation = firstTouch - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);


            transform.position += (Vector3)deltaLocation * movementSpeed;

        }
        if(Input.GetAxis("Mouse ScrollWheel") != 0)
            zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float zoomRate)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - zoomRate, 1.5f, 10);
        UpdateColider();
    }

    void UpdateColider()
    {
        Vector3 size = -(Camera.main.ScreenToWorldPoint(new Vector3(1, 1)) - transform.position)* 2;


        boxCollider.size = size + boxOffset;
    }
}
