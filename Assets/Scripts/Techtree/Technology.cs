using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Technology : MonoBehaviour
{
    public string techName;
    public int cost;

    bool unlocked;

    [SerializeField] Image icon;
    [SerializeField] TMP_Text text_techName;
    public int[] conectedTechIDs;


    public void initTech()
    {
        Button button = GetComponent<Button>();
    }

}
