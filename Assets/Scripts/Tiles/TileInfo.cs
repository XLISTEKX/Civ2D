using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    Vector2 size = new (35, 35);

    [SerializeField] GameObject ImagePrefab;
    [SerializeField] Sprite[] icons;
    Vector3 spawnOffset = new(0, 0.125f);

    public void InitInfo(ResourcesTile resources)
    {
        int[] values = { resources.food, resources.production, resources.cash, resources.science };
        int[] IDs = ResourcesTile.GetIDs(resources);
        transform.position += spawnOffset;

        switch (IDs.Length)
        {
            case 0:
                break;
            case 1:
                InitOne(values, IDs);
                break;
            case 2:
                InitTwo(values, IDs);
                break;
            case 3:
                InitThree(values, IDs);
                break;
            case 4:
                InitFour(values, IDs);
                break;
        }
    }
    void InitOne(int[] values, int[] IDs)
    {
        int tempID = IDs[0];
        TileInfoImage image = Instantiate(ImagePrefab, transform).GetComponent<TileInfoImage>();
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        image.InitImage(icons[tempID], values[tempID]);
    }
    void InitTwo(int[] values, int[] IDs)
    {

        TileInfoImage image = Instantiate(ImagePrefab, transform).GetComponent<TileInfoImage>();

        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(size.x / 2, 0);
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        image.InitImage(icons[IDs[0]], values[IDs[0]]);

        image = Instantiate(ImagePrefab, transform).GetComponent<TileInfoImage>();

        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(-size.x / 2, 0);
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        image.InitImage(icons[IDs[1]], values[IDs[1]]);
    }

    void InitThree(int[] values, int[] IDs)
    {
        TileInfoImage image = Instantiate(ImagePrefab, transform).GetComponent<TileInfoImage>();

        image.GetComponent< RectTransform >().anchoredPosition = new Vector3(size.x / 2, -size.y / 2);
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        image.InitImage(icons[IDs[0]], values[IDs[0]]);

        image = Instantiate(ImagePrefab, transform).GetComponent<TileInfoImage>();

        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(-size.x / 2, -size.y / 2);
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        image.InitImage(icons[IDs[1]], values[IDs[1]]);

        image = Instantiate(ImagePrefab, transform).GetComponent<TileInfoImage>();

        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, size.y / 2);
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        image.InitImage(icons[IDs[2]], values[IDs[2]]);
    }
    void InitFour(int[] values, int[] IDs)
    {
        TileInfoImage image = Instantiate(ImagePrefab, transform).GetComponent<TileInfoImage>();

        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(size.x / 2, -size.y / 2);
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        image.InitImage(icons[IDs[0]], values[IDs[0]]);

        image = Instantiate(ImagePrefab, transform).GetComponent<TileInfoImage>();

        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(-size.x / 2, -size.y / 2);
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        image.InitImage(icons[IDs[1]], values[IDs[1]]);

        image = Instantiate(ImagePrefab, transform).GetComponent<TileInfoImage>();

        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(-size.x / 2, size.y / 2);
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        image.InitImage(icons[IDs[2]], values[IDs[2]]);

        image = Instantiate(ImagePrefab, transform).GetComponent<TileInfoImage>();

        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(size.x / 2, size.y / 2);
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
        image.InitImage(icons[IDs[3]], values[IDs[3]]);
    }
}
