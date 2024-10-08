using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class View : Singleton<View>
{
    // Start is called before the first frame update
    [Header("编辑面板")]
    public Transform EditPanel;
    [Header("工作面板")]
    public Transform WorkPanelPhototype;
    public Transform WorkPanelParent;
    [Header("信息面板")]
    public Transform InfoPanelPhototype;
    public Transform InfoPanelParent;
    public Transform InfoSlider;

    public Transform DayPanel;

    private List<Transform> L_WorkPanel = new List<Transform>();
    private List<Transform> L_infoPanel = new List<Transform>();
    private List<InfoItem> L_infoItemPanel = new List<InfoItem>();
    //消息面板信息


    [HideInInspector]public int curWorkPanelNum;
    [HideInInspector]public int curInfoPanelNum;

    void Start()
    {
        curWorkPanelNum = 0;
        if (Controller.Instance.isEditModel == true)
        {
            EditPanel.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        DayPanel.GetComponent<TextMeshProUGUI>().text = "当前天数: " + Model.Instance.cur_Day;
    }
    public void AddWorkPanel(string workCategory)
    {
        curWorkPanelNum++;
        Transform tmpPanel = null;
        foreach (var tmp in L_WorkPanel)
        {
            if(tmp.GetChild(0).GetComponent<TextMeshProUGUI>().text == workCategory)
            {
                tmpPanel = tmp;
            }
        }
        if(tmpPanel == null)
        {
            tmpPanel = Instantiate(WorkPanelPhototype, WorkPanelParent, false);
        }
        tmpPanel.gameObject.SetActive(true);
        tmpPanel.localPosition = new Vector3(WorkPanelPhototype.localPosition.x, WorkPanelPhototype.localPosition.y * (curWorkPanelNum * 2 - 1), 0);
        tmpPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = workCategory;
        L_WorkPanel.Add(tmpPanel);
    }
    public void DecreaseWorkPanel(string workCategory)
    {
        for(int count = 0; count < curWorkPanelNum; count++)
        {
            if (L_WorkPanel[count].GetChild(0).GetComponent<TextMeshProUGUI>().text == workCategory)
            {                
                L_WorkPanel[count].gameObject.SetActive(false);
                L_WorkPanel.Remove(L_WorkPanel[count]);
                curWorkPanelNum--;
                for (int i = count; i < curWorkPanelNum; i++)
                {
                    L_WorkPanel[i].localPosition = new Vector3(WorkPanelPhototype.localPosition.x, WorkPanelPhototype.localPosition.y * (i * 2 + 1), 0);
                }
                break;
            }           
        }
    }

    public void AddInfoPanel(string infoType, string infoContent)
    {
        Controller.Instance.audioS.PlayOneShot(Controller.Instance.InfoPanelShow);
        var tmpPanel = Instantiate(InfoPanelPhototype, InfoPanelParent, false);
        int tmpHeigh = 32;
        int tmpYPosition = 16;
        if (infoContent.Length > 12)
        {
            tmpHeigh = ((infoContent.Length / 12) + 1) * 32 ;
        }
        if(L_infoItemPanel.Count == 0 && curInfoPanelNum == 0)
        {
            tmpYPosition = - tmpHeigh / 2;
        }
        else
        {
            tmpYPosition = L_infoItemPanel[curInfoPanelNum - 1].localYPos - L_infoItemPanel[curInfoPanelNum - 1].Heigh / 2 - tmpHeigh / 2;
        }
        //如果超过content内容就扩充content
        if (Mathf.Abs(tmpYPosition) + tmpHeigh / 2 > InfoPanelParent.GetComponent<RectTransform>().rect.height)
        {
            InfoPanelParent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 
                InfoPanelParent.GetComponent<RectTransform>().rect.height * 2);
        }
        InfoItem tmpInfo = new InfoItem(tmpPanel, infoType, infoContent, tmpHeigh, tmpYPosition);
        tmpPanel.gameObject.SetActive(true);
        tmpPanel.localPosition = new Vector3(InfoPanelPhototype.localPosition.x, tmpYPosition, 0);
        tmpPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(345, tmpHeigh);
        tmpPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = infoType;
        tmpPanel.GetChild(1).GetComponent<TextMeshProUGUI>().text = infoContent;

        float tmp = (Mathf.Abs(tmpYPosition) + (tmpHeigh / 2)) / 625;
        float tmpValue = (Mathf.Abs(tmpYPosition) + (tmpHeigh / 2)) / InfoPanelParent.GetComponent<RectTransform>().rect.height;
        if(tmp > 0.5)
        {
            InfoSlider.GetComponent<Scrollbar>().value = 1 - tmpValue;
        }

        L_infoItemPanel.Add(tmpInfo);
        curInfoPanelNum++;
    }
    public void DecreaseInfoPanel(string infoType, string infoContent)
    {

    }
}
public class InfoItem
{
    public Transform transform;
    public string infoType;
    public string infoContent;
    public int Heigh;
    public int localYPos;
    public InfoItem(Transform transform, string infoType, string infoContent, int Heigh, int localYPos)
    {
        this.transform = transform;
        this.infoType = infoType;
        this.infoContent = infoContent;
        this.Heigh = Heigh;
        this.localYPos = localYPos; 
    }
}
