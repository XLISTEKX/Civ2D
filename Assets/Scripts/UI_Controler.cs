using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controler : MonoBehaviour
{
    [SerializeField] List<GameObject> UIPanels;
    int ID = 0;
    
    public void openPanel(int id)
    {
        UIPanels[id].SetActive(true);
        UIPanels[ID].SetActive(false);
        ID = id;
    }
}
