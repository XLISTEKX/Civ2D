using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TechPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text textName, textProgressbar;
    [SerializeField] Image icon;
    [SerializeField] Slider progressbar;
    [SerializeField] GameObject cover;

    TechNode tech;

    public void updateNode(TechNode techNode)
    {
        tech = techNode;
        textName.text = techNode.techName;
        icon.sprite = techNode.icon;

        textProgressbar.text = tech.techProgress + "/" + techNode.techCost;
        progressbar.value = (float)tech.techProgress / techNode.techCost;

        if (techNode.unlocked)
        {
            progressbar.gameObject.SetActive(false);
            textProgressbar.gameObject.SetActive(false);
        }
    }

    public void changeColor(Color color)
    {
        textName.color = color;
    }

    public void turnCover()
    {
        cover.SetActive(false);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (tech.research && !tech.unlocked)
        {
            GetComponentInParent<Techtree>().changeResearch(tech);
        }
    }
}
