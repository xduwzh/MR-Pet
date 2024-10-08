using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroducePanel : MonoBehaviour
{
    public GameObject lastBtn;
    public GameObject nextBtn;
    public GameObject panel0;
    public GameObject panel1;
    public GameObject introducePanel;
    public GameObject backgroundPanel;

    // Start is called before the first frame update
    public void nextPanel()
    {
        lastBtn.SetActive(true);
        nextBtn.SetActive(false);
        panel0.SetActive(false);
        panel1.SetActive(true);
    }

    public void lastPanel()
    {
        lastBtn.SetActive(false);
        nextBtn.SetActive(true);
        panel0.SetActive(true);
        panel1.SetActive(false);
    }

    public void closePanel()
    {
        introducePanel.SetActive(false);
    }

    public void closeBackgroundPanel()
    {
        backgroundPanel.SetActive(false);
    }

}
