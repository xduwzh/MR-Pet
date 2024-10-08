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



    //点击建筑的时候调用这个函数，用来调整UI
    //传参就传建筑实例
    public void setToggle(TopBuilding building)
    {

        currentBuilding = building;
        currentMotion = currentBuilding.buildingMotions[0];
        //Debug.Log("设定当前的建筑和事件");
        

        //Debug.Log("获取开关集");
        //Debug.Log(toggles.Length);

        for (int i = 0;i< 4; i++)
        {
            if(i < currentBuilding.motionCounts)
            {

                toggles[i].gameObject.SetActive(true);
                //Debug.Log("现在是第" + i + "个按钮，激活");

                int K = i;
                toggles[K].onValueChanged.AddListener((bool value) => SetEveryToggle(value, K));

            }
            else
            {
                //Debug.Log("现在是第" + i + "个按钮，失活");
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
            //Debug.Log("第一个toggle");
        }
        if (j == 1 && value)
        {
            currentMotion = currentBuilding.buildingMotions[j];
            motionShow();
            //Debug.Log("第二个toggle");
        }
        if (j == 2 && value)
        {
            currentMotion = currentBuilding.buildingMotions[j];
            motionShow();
            //Debug.Log("第三个toggle");
        }

        if (j == 3 && value)
        {
            currentMotion = currentBuilding.buildingMotions[j];
            motionShow();
            //Debug.Log("第四个toggle");
        }
    }



    public void buttenMotionGo()
    {
        currentMotion.goMotion();
    }



    //例子：
    private void Start()
    {
        D_resourceTypeName = new Dictionary<ResourceType, string>();
        D_resourceTypeName.Add(ResourceType.BuildingMaterials, "建筑材料");
        D_resourceTypeName.Add(ResourceType.Food, "食物");
        D_resourceTypeName.Add(ResourceType.Knowledge, "智力");
        D_resourceTypeName.Add(ResourceType.Seed, "种子");
        //test
        //餐厅事件，最后一个参数是仇恨值
        MotionClass motion1 = new MotionClass("获取食物", "“这里吃的东西这么多，那点应该不会被发现吧”鼠鼠心里想。	",
                                  "骰子点数>3,成功奖励食物*30，失败增加仇恨值3点",
                                  3, ResourceType.Food, 15, 2);

        MotionClass motion2 = new MotionClass("运送厨余垃圾", "只是帮忙倒垃圾而已，顺便拿走里面有用的东西应该没事吧。	",
                                             "骰子点数>4,成功奖励食物*10，失败减少仇恨值1点",
                                             4, ResourceType.Food, 25, 3);

        Restaurant = new TopBuilding("Restaurant", motion1, motion2);


        //书店事件
        MotionClass motion3 = new MotionClass("读书破万卷", "插图真的很有趣，但是那么多密密麻麻的东西究竟有什么用呢。	",
                          "骰子点数>2,成功奖励智商*3，失败增加仇恨值1点",
                          3, ResourceType.Knowledge, 1, 1);

        MotionClass motion4 = new MotionClass("书,是我的了", "这本书插图不错，收下了	",
                                             "骰子点数>6,成功奖励建筑材料*40，失败增加仇恨值2点",
                                             5, ResourceType.Knowledge, 2, 2);

        BookStore = new TopBuilding("BookStore", motion3, motion4);

        //建筑工地事件
        MotionClass motion5 = new MotionClass("来点混凝土", "灰一样但是遇水就会凝固，神奇",
                  "骰子点数>3,成功奖励建筑材料*25，失败增加仇恨值3点",
                  3, ResourceType.BuildingMaterials, 25, 2);

        MotionClass motion6 = new MotionClass("清理建筑碎渣", "这些乱扔的钉子很危险的，鼠鼠来收拾了！",
                                             "骰子点数>4,成功奖励建筑材料*10，失败减少仇恨值1点",
                                             4, ResourceType.BuildingMaterials, 35, 3);

        ConstructionSite = new TopBuilding("ConstructionSite", motion5, motion6);

        //植物园
        MotionClass motion7 = new MotionClass("获取种子", "书中刚好有图画说明这些种子及以后长大的样子",
          "骰子点数>4,成功奖励种子*2，失败增加仇恨值2点",
          2, ResourceType.Seed, 2, 2);

        MotionClass motion8 = new MotionClass("获取种子++", "多拿点吧，就可以多种点了",
                                             "骰子点数>6,成功奖励种子*5，失败增加仇恨值4点",
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
