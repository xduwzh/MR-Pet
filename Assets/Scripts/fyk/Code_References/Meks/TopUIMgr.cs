using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopUIMgr : Singleton<TopUIMgr>
{

    public TopBuilding ConstructionSite;
    public TopBuilding Restaurant;
    public TopBuilding BookStore;
    public TopBuilding Arboretum;


    public GameObject topPanel = null;

    public TextMeshProUGUI[] motionCollection;



    public TextMeshProUGUI building; 
    public TextMeshProUGUI introduce;
    public TextMeshProUGUI cost;

    //public Image Img;


    public Toggle[] toggles = new Toggle[4];

    public Button goButton;


    MotionClass currentMotion = null;

    public TopBuilding currentBuilding = null;

    public Dictionary<ResourceType, string> D_resourceTypeName;
    public void motionShow()
    {
        introduce.text = currentMotion.textIntroduce;
        cost.text = currentMotion.textCost;
        //Img = currentMotion.motionImg;

        //Debug.Log(currentBuilding.name);
        building.text = currentBuilding.name;

        for(int i = 0;i < currentBuilding.motionCounts;i++)
        {
            motionCollection[i].text = currentBuilding.buildingMotions[i].motionName;
        }


    }



    //���������ʱ����������������������UI
    //���ξʹ�����ʵ��
    public void setToggle(TopBuilding building)
    {

        currentBuilding = building;
        currentMotion = currentBuilding.buildingMotions[0];
        //Debug.Log("�趨��ǰ�Ľ������¼�");
        

        //Debug.Log("��ȡ���ؼ�");
        //Debug.Log(toggles.Length);

        for (int i = 0;i< 4; i++)
        {
            if(i < currentBuilding.motionCounts)
            {

                toggles[i].gameObject.SetActive(true);
                //Debug.Log("�����ǵ�" + i + "����ť������");

                int K = i;
                toggles[K].onValueChanged.AddListener((bool value) => SetEveryToggle(value, K));

            }
            else
            {
                //Debug.Log("�����ǵ�" + i + "����ť��ʧ��");
                toggles[i].gameObject.SetActive(false);
            }

        }
    }


    void SetEveryToggle(bool value, int j)
    {
        if (j == 0 && value)
        {
            currentMotion = currentBuilding.buildingMotions[j];
            motionShow();
            //Debug.Log("��һ��toggle");
        }
        if (j == 1 && value)
        {
            currentMotion = currentBuilding.buildingMotions[j];
            motionShow();
            //Debug.Log("�ڶ���toggle");
        }
        if (j == 2 && value)
        {
            currentMotion = currentBuilding.buildingMotions[j];
            motionShow();
            //Debug.Log("������toggle");
        }

        if (j == 3 && value)
        {
            currentMotion = currentBuilding.buildingMotions[j];
            motionShow();
            //Debug.Log("���ĸ�toggle");
        }
    }



    public void buttenMotionGo()
    {
        currentMotion.goMotion();
    }



    //���ӣ�
    private void Start()
    {
        D_resourceTypeName = new Dictionary<ResourceType, string>();
        D_resourceTypeName.Add(ResourceType.BuildingMaterials, "��������");
        D_resourceTypeName.Add(ResourceType.Food, "ʳ��");
        D_resourceTypeName.Add(ResourceType.Knowledge, "����");
        D_resourceTypeName.Add(ResourceType.Seed, "����");
        //test
        //�����¼������һ�������ǳ��ֵ
        MotionClass motion1 = new MotionClass("��ȡʳ��", "������ԵĶ�����ô�࣬�ǵ�Ӧ�ò��ᱻ���ְɡ����������롣	",
                                  "���ӵ���>3,�ɹ�����ʳ��*30��ʧ�����ӳ��ֵ3��",
                                  3, ResourceType.Food, 15, 2);

        MotionClass motion2 = new MotionClass("���ͳ�������", "ֻ�ǰ�æ���������ѣ�˳�������������õĶ���Ӧ��û�°ɡ�	",
                                             "���ӵ���>4,�ɹ�����ʳ��*10��ʧ�ܼ��ٳ��ֵ1��",
                                             4, ResourceType.Food, 25, 3);

        Restaurant = new TopBuilding("Restaurant", motion1, motion2);


        //����¼�
        MotionClass motion3 = new MotionClass("���������", "��ͼ��ĺ���Ȥ��������ô����������Ķ���������ʲô���ء�	",
                          "���ӵ���>2,�ɹ���������*3��ʧ�����ӳ��ֵ1��",
                          3, ResourceType.Knowledge, 1, 1);

        MotionClass motion4 = new MotionClass("��,���ҵ���", "�Ȿ���ͼ����������	",
                                             "���ӵ���>6,�ɹ�������������*40��ʧ�����ӳ��ֵ2��",
                                             5, ResourceType.Knowledge, 2, 2);

        BookStore = new TopBuilding("BookStore", motion3, motion4);

        //���������¼�
        MotionClass motion5 = new MotionClass("���������", "��һ��������ˮ�ͻ����̣�����",
                  "���ӵ���>3,�ɹ�������������*25��ʧ�����ӳ��ֵ3��",
                  3, ResourceType.BuildingMaterials, 25, 2);

        MotionClass motion6 = new MotionClass("����������", "��Щ���ӵĶ��Ӻ�Σ�յģ���������ʰ�ˣ�",
                                             "���ӵ���>4,�ɹ�������������*10��ʧ�ܼ��ٳ��ֵ1��",
                                             4, ResourceType.BuildingMaterials, 35, 3);

        ConstructionSite = new TopBuilding("ConstructionSite", motion5, motion6);

        //ֲ��԰
        MotionClass motion7 = new MotionClass("��ȡ����", "���иպ���ͼ��˵����Щ���Ӽ��Ժ󳤴������",
          "���ӵ���>4,�ɹ���������*2��ʧ�����ӳ��ֵ2��",
          2, ResourceType.Seed, 2, 2);

        MotionClass motion8 = new MotionClass("��ȡ����++", "���õ�ɣ��Ϳ��Զ��ֵ���",
                                             "���ӵ���>6,�ɹ���������*5��ʧ�����ӳ��ֵ4��",
                                             4, ResourceType.Seed, 5, 3);



        Arboretum = new TopBuilding("Arboretum", motion7, motion8);


        currentBuilding = Restaurant;
        currentMotion = Restaurant.buildingMotions[0];
    }


    public void showPanel(GameObject panel)
    {
        panel.gameObject.SetActive(true);
    }




    private void Update()
    {

        if(currentMotion.motionState == MotionClass.E_motionState.rest)
        {
            goButton.interactable = true;
        }
        if (currentMotion.motionState == MotionClass.E_motionState.start)
        {
            goButton.interactable = false;
        }

    }




}
