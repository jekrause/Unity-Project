using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] public GameObject InteractivePanel;
    [SerializeField] public GameObject ItemTypeText;
    [SerializeField] public Slider HoldButtonDownBar;
    private Image FillColor;
    private Color DefaultColor;

    // Use this for initialization
    void Start()
    {
        HoldButtonDownBar.value = 0;
        FillColor = HoldButtonDownBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        DefaultColor = FillColor.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowInteractionPanel(string item, string action)
    {
        InteractivePanel.transform.GetComponentInChildren<Text>().text = action;
        ItemTypeText.GetComponent<Text>().text = item;
        InteractivePanel.gameObject.SetActive(true);
    }

    public void RemoveInteractivePanel()
    {
        InteractivePanel.gameObject.SetActive(false);
    }

    public void ShowLoadBar(float time, float maxTime)
    {
        HoldButtonDownBar.value = time / maxTime;
        if (HoldButtonDownBar.value > .20f) HoldButtonDownBar.gameObject.SetActive(true);
        if (HoldButtonDownBar.value >= 1)
        {
            HoldButtonDownBar.value = 0;
            HoldButtonDownBar.gameObject.SetActive(false);
        }
    }

    public void RemoveLoadBar()
    {
        if (FillColor == null) return;
        HoldButtonDownBar.value = 0;
        FillColor.color = DefaultColor;
        HoldButtonDownBar.gameObject.SetActive(false);
    }

    public void ShowRejectedLoadBar()
    {
        HoldButtonDownBar.value = 1;
        FillColor.color = Color.red;
    }
}
