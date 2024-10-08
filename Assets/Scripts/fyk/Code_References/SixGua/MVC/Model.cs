using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ResourceType
{
    BuildingMaterials, Food, Knowledge,Seed
}
public class Model : Singleton<Model>
{
    //--------------------------------------------------
    //-------------------建筑相关数据-------------------
    //--------------------------------------------------
    //建筑PlaceableObjectSO资源 建筑的大小 长宽 eg. 2×3
    [Header("下层世界建筑")]
    public PlaceableObjectSO House;
    public PlaceableObjectSO House2;
    public PlaceableObjectSO FoodFliter;
    public PlaceableObjectSO WareHouse;
    public PlaceableObjectSO StudyRoom;
    public PlaceableObjectSO Lamp;
    [Header("上层层世界建筑")]
    public PlaceableObjectSO ConstructionSite;
    public PlaceableObjectSO ManholeCover;
    public PlaceableObjectSO BookStore;
    public PlaceableObjectSO Restaurant;
    public PlaceableObjectSO Arboretum;
    [Header("阻挡")]
    public PlaceableObjectSO Rock;
    public PlaceableObjectSO Wall;
    [Header("下层资源")]
    public PlaceableObjectSO FoodResource;
    public PlaceableObjectSO MineResource;
    [Header("农作物")]
    public PlaceableObjectSO BeanSproutFarm;
    public PlaceableObjectSO FishygrassFarm;
    public PlaceableObjectSO MaizeFarm;
    public PlaceableObjectSO PeanutsFarm;
    public PlaceableObjectSO PumpkinFarm;


    public PlaceableObjectSO Trans;
    public PlaceableObjectSO Road;

    public Transform MousePrefab;

    //建筑数量以及建筑数据存储
    //下层世界建筑
    [HideInInspector] public List<PlaceableObject> L_House;
    [HideInInspector] public List<PlaceableObject> L_FoodFliter;
    [HideInInspector] public List<PlaceableObject> L_WareHouse;
    [HideInInspector] public List<PlaceableObject> L_StudyRoom;
    [HideInInspector] public List<PlaceableObject> L_Lamp;
    //上层世界建筑
    [HideInInspector] public List<PlaceableObject> L_ConstructionSite;
    [HideInInspector] public List<PlaceableObject> L_ManholeCover;
    [HideInInspector] public List<PlaceableObject> L_BookStore;
    [HideInInspector] public List<PlaceableObject> L_Restaurant;
    [HideInInspector] public List<PlaceableObject> L_Arboretum;
    //阻挡
    [HideInInspector] public List<PlaceableObject> L_Rock;
    [HideInInspector] public List<PlaceableObject> L_Wall;
    //资源点
    [HideInInspector] public List<PlaceableObject> L_FoodResource;
    [HideInInspector] public List<PlaceableObject> L_MineResource;
    //农作物
    [HideInInspector] public List<PlaceableObject> L_BeanSproutFarm;
    [HideInInspector] public List<PlaceableObject> L_FishygrassFarm;
    [HideInInspector] public List<PlaceableObject> L_MaizeFarm;
    [HideInInspector] public List<PlaceableObject> L_PeanutsFarm;
    [HideInInspector] public List<PlaceableObject> L_PumpkinFarm;

    [HideInInspector] public List<PlaceableObject> L_Trans;
    [HideInInspector] public List<PlaceableObject> L_Road;

    [Header("生成资源数量")]
    public int FoodResourceNum;
    public int MineResourceNum;

    [Header("资源采集和存储时间")]
    public int MineCollectTime;
    public int FoodCollectTime;
    public int MineStoreTime;
    public int FoodStoreTime;
    [Header("每次采集的资源数量")]
    public int MineNumEachTime;
    public int FoodNumEachTime;

    [Header("UI")]
    public GameObject WinPanel;

    //基础数值
    [Header("基础数值")]
    public int KnowledgeLevel;
    [Header("仇恨值")]
    public int HateValue;//越低触发友好事件 越高触发仇恨事件
    [Header("初始食物存储上限")]
    public int FoodStoreLimit;

    //食物存储量
    public int FoodStore;
    //建造材料
    [Header("建筑材料")]
    public int BuildingMaterials;
    //骰子数量/行动点数
    [HideInInspector] public int DiceNum;
    //人口数量
    [HideInInspector] public int Population;
    //种子数量
    public int SeedStore;

    //当前时间
    [HideInInspector] public int cur_Day;
    public int EachDayTime = 60;
    private double TimerCounter = 0;

    [Header("需要坚持的天数")]
    public int DayNumToDisaster = 100;
    [Header("需要存储的食物总量")]
    public int FoodStoreNeeded = 1000;

    //老鼠数量以及各种数据存储
    [HideInInspector]public int MouseCount;
    [HideInInspector] public List<C_Mouse> Mouse_List;

    //存档名列表
    [HideInInspector]public List<string> L_ArchivesName;

    //两次提醒间隔时间
    private int ReminderIntervalTime = 5;
    private float IntervaltimeCounter;
    private bool isShowReminder = true;
    private bool[] showHateValueReminder = new bool[5]; 

    //关灯房子计数
    private int downLightHouseCounter = 0;
    private void Start()
    {
        for (int i = 0; i < showHateValueReminder.Length; i++)
        {
            showHateValueReminder[i] = true;
        }
    }
    public void InitUpBuidingMotions()
    {
        //餐厅事件，最后一个参数是仇恨值
        MotionClass motion1 = new MotionClass("垂涎三尺", "“这里吃的东西这么多，那点应该不会被发现吧”鼠鼠心里想。	",
                                  "骰子点数>4",
                                  4, ResourceType.Food, 30, 3);

        MotionClass motion2 = new MotionClass("运送厨余垃圾", "只是帮忙倒垃圾而已，顺便拿走里面有用的东西应该没事吧？	",
                                             "骰子点数>5",
                                             6, ResourceType.Food, 10, -1);


        Restaurant.buildingMotions.Add(motion1);
        Restaurant.buildingMotions.Add(motion2);

        //书店事件
        MotionClass motion3 = new MotionClass("读书破万卷", "插图真的很有趣，但是那么多密密麻麻的东西究竟有什么用呢？	",
                          "骰子点数>2",
                          2, ResourceType.Knowledge, 3, 0);

        MotionClass motion4 = new MotionClass("书是·我的了", "这本书插图不错，收下了	",
                                             "骰子点数>6",
                                             6, ResourceType.BuildingMaterials, 40, 2);

        BookStore.buildingMotions.Add(motion3);
        BookStore.buildingMotions.Add(motion4);

        //建筑工地事件
        MotionClass motion5 = new MotionClass("来点混凝土", "灰一样但是遇水就会凝固，神奇",
                  "骰子点数>4",
                  4, ResourceType.BuildingMaterials, 25, 3);

        MotionClass motion6 = new MotionClass("清理建筑碎渣", "这些乱扔的钉子很危险的，鼠鼠来收拾了！",
                                             "骰子点数>5",
                                             5, ResourceType.BuildingMaterials, 10, -1);

        ConstructionSite.buildingMotions.Add(motion5);
        ConstructionSite.buildingMotions.Add(motion6);

        //植物园
        MotionClass motion7 = new MotionClass("获取种子", "书中刚好有图画说明这些种子及以后长大的样子",
          "骰子点数>4",
          4, ResourceType.Seed, 2, 2);

        MotionClass motion8 = new MotionClass("获取种子++", "多拿点吧，就可以多种点了",
                                             "骰子点数>6",
                                             6, ResourceType.Seed, 5, 4);

        Arboretum.buildingMotions.Add(motion7);
        Arboretum.buildingMotions.Add(motion8);
    }

    private void Update()
    {
        TimeCount();
        //判断游戏成功以及失败
        if(cur_Day == DayNumToDisaster)
        {
            if(FoodStore > FoodStoreNeeded || FoodStore == FoodStoreNeeded)
            {
                WinPanel.gameObject.SetActive(true);
                //Debug.Log("游戏成功");
            }
            else
            {
                //Debug.Log("游戏失败");
                SceneManager.LoadScene(2);

            }
        }
        if (FoodStore < 0)
        {
            FoodStore = 0;
        }
        if(BuildingMaterials < 0)
        {
            BuildingMaterials = 0;
        }
        if(FoodStore > FoodStoreLimit)
        {
            FoodStore = FoodStoreLimit;
            if (isShowReminder)
            {
                View.Instance.AddInfoPanel("提醒: ", "食物存储量达到上限，建造更多的仓库以提高食物存储上限");
                isShowReminder = false;
            }
            IntervaltimeCounter += Time.deltaTime;
            if(IntervaltimeCounter > ReminderIntervalTime)
            {
                isShowReminder = true; ;
                IntervaltimeCounter = 0;
            }
        }
        if(HateValue > 20 || HateValue == 20)
        {
            Debug.Log(showHateValueReminder[0]);
            if (showHateValueReminder[0] == true)
            {
                View.Instance.AddInfoPanel("提醒: ", "人类对老鼠的仇恨值大于20，上层世界人类开始提高警惕，执行事件成功的概率会降低");
                showHateValueReminder[0] = false;
                decreaseProbility(1);
            }      
        }
        if (HateValue > 40 || HateValue == 40)
        {
            if (showHateValueReminder[1] == true)
            {
                View.Instance.AddInfoPanel("提醒: ", "人类对老鼠的仇恨值大于40，上层世界人类尝试干预老鼠世界的建设，建筑材料减少50");
                showHateValueReminder[1] = false;
                BuildingMaterials -= 50;
            }
        }
        if (HateValue > 60 || HateValue == 60)
        {
            if (showHateValueReminder[2] == true)
            {
                View.Instance.AddInfoPanel("提醒: ", "人类对老鼠的仇恨值大于60，上层世界人类开始采取方案防备老鼠，执行事件成功的概率大幅降低");
                showHateValueReminder[2] = false;
                decreaseProbility(1);
            }
        }
        if (HateValue > 80 || HateValue == 80)
        {
            if (showHateValueReminder[3] == true)
            {
                View.Instance.AddInfoPanel("提醒: ", "人类对老鼠的仇恨值大于80，人类加倍干预老鼠世界的建设，食物损失200，建筑材料损失100");
                showHateValueReminder[3] = false;
                FoodStore -= 200;
                BuildingMaterials -= 100;
            }
        }
        if (HateValue > 100 || HateValue == 100)
        {
            if (showHateValueReminder[4] == true)
            {
                View.Instance.AddInfoPanel("提醒: ", "人类对老鼠的仇恨值达到极限，全面围剿老鼠，无法再执行上层事件");
                showHateValueReminder[4] = false;
                Controller.Instance.canDoMoition = false;
            }
        }
    }
    private void decreaseProbility(int num)
    {
        foreach (var motionClass in TopUIMgr.Instance.BookStore.buildingMotions)
        {
            motionClass.diceNum += num;
            motionClass.refreshTExtCost();
        }
        foreach (var motionClass in TopUIMgr.Instance.Restaurant.buildingMotions)
        {
            motionClass.diceNum += num;
            motionClass.refreshTExtCost();
        }
        foreach (var motionClass in TopUIMgr.Instance.Arboretum.buildingMotions)
        {
            motionClass.diceNum += num;
            motionClass.refreshTExtCost();
        }
        foreach (var motionClass in TopUIMgr.Instance.ConstructionSite.buildingMotions)
        {
            motionClass.diceNum += num;
            motionClass.refreshTExtCost();
        }
    }

    public void ChangeBasicValue(string valueName, bool isIncrease, int changeValue)
    {
        if(valueName == "Food")
        {
            FoodStore += isIncrease ? changeValue : (-changeValue);
        }
        else if (valueName == "ConstructionMaterial")
        {
            BuildingMaterials += isIncrease ? changeValue : (-changeValue);
        }
        else if (valueName == "Seed")
        {
            SeedStore += isIncrease ? changeValue : (-changeValue);
        }
    }
    private bool isListStillHaveMouse(List<PlaceableObject> list)
    {
        foreach(PlaceableObject item in list)
        {
            if(item.L_WorkingMouse.Count > 0)
            {
                return true;
            }
        }
        return false;
    }
    private void deleteMouseFromBuilding(List<PlaceableObject> list)
    {
        foreach (var foodCollecter in list)
        {
            if (foodCollecter.L_WorkingMouse.Count > 0)
            {
                var tmpMouse = foodCollecter.L_WorkingMouse[0];
                foodCollecter.L_WorkingMouse.Remove(tmpMouse);
                Destroy(tmpMouse.gameObject);
                if (foodCollecter.L_WorkingMouse.Count == 0)
                {
                    foodCollecter.isStartWorking = false;
                    foodCollecter.isStartCollectResource = false;
                }
                return;
            }
        }
    }

    public void TimeCount()
    {
        TimerCounter += Time.deltaTime;
        if(TimerCounter > EachDayTime)
        {
            cur_Day++;
            TimerCounter = 0;
            if(FoodStore < MouseCount * 2)
            {
                int DecreaseMouseNum = ((MouseCount * 2 - FoodStore) / 2) == 0 ? 1 : (MouseCount * 2 - FoodStore) / 2;
                if (MouseCount > DecreaseMouseNum)
                {
                    MouseCount -= DecreaseMouseNum;
                }
                else
                {
                    SceneManager.LoadScene(2);
                }

                if (DecreaseMouseNum > 5 || DecreaseMouseNum == 5)
                {
                    int downLightHouseNum = DecreaseMouseNum / 5;
                    for(int i = 0; i < downLightHouseNum; i++)
                    {
                        if(downLightHouseCounter < L_House.Count)
                        {
                            L_House[downLightHouseCounter].transform.GetChild(0).Find("Light").gameObject.SetActive(false);
                            L_House[downLightHouseCounter].isMouseLiving = false;
                            downLightHouseCounter++;
                        }
                    }
                }
                
                for(int i = 0; i < DecreaseMouseNum; i++)
                {
                    if (Mouse_List.Count > 0)
                    {
                        var tmpMouse = Mouse_List[0];
                        Mouse_List.Remove(tmpMouse);
                        Destroy(tmpMouse.gameObject);
                    }
                    else
                    {
                        if (isListStillHaveMouse(L_FoodFliter))
                        {
                            deleteMouseFromBuilding(L_FoodFliter);
                        }else if (isListStillHaveMouse(L_BeanSproutFarm))
                        {
                            deleteMouseFromBuilding(L_BeanSproutFarm);
                        }
                        else if (isListStillHaveMouse(L_FishygrassFarm))
                        {
                            deleteMouseFromBuilding(L_FishygrassFarm);
                        }
                        else if (isListStillHaveMouse(L_MaizeFarm))
                        {
                            deleteMouseFromBuilding(L_MaizeFarm);
                        }
                        else if (isListStillHaveMouse(L_PeanutsFarm))
                        {
                            deleteMouseFromBuilding(L_PeanutsFarm);
                        }
                        else if (isListStillHaveMouse(L_PumpkinFarm))
                        {
                            deleteMouseFromBuilding(L_PumpkinFarm);
                        }
                        else if (isListStillHaveMouse(L_BookStore))
                        {
                            deleteMouseFromBuilding(L_BookStore);
                        }
                        else if (isListStillHaveMouse(L_WareHouse))
                        {
                            deleteMouseFromBuilding(L_WareHouse);
                        }
                    }
                }
                View.Instance.AddInfoPanel("事件: ", "由于没有足够的食物，有" + DecreaseMouseNum + "只老鼠死于饥饿");
                FoodStore = 0;
            }
            else
            {
                FoodStore -= MouseCount * 2;
            }
        }
    }
}  
