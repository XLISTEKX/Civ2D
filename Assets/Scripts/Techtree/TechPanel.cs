using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TechPanel : MonoBehaviour
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

        textProgressbar.text = "0/" + techNode.techCost.ToString();

        if (techNode.unlocked)
        {
            progressbar.gameObject.SetActive(false);
            textProgressbar.gameObject.SetActive(false);
        }
    }

    public void turnCover()
    {
        cover.SetActive(false);
    }

    public void clickOnPanel()
    {
        Debug.Log(tech.techName);
    }
}
