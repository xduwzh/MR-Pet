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
    //-------------------�����������-------------------
    //--------------------------------------------------
    //����PlaceableObjectSO��Դ �����Ĵ�С ���� eg. 2��3
    [Header("�²����罨��")]
    public PlaceableObjectSO House;
    public PlaceableObjectSO House2;
    public PlaceableObjectSO FoodFliter;
    public PlaceableObjectSO WareHouse;
    public PlaceableObjectSO StudyRoom;
    public PlaceableObjectSO Lamp;
    [Header("�ϲ�����罨��")]
    public PlaceableObjectSO ConstructionSite;
    public PlaceableObjectSO ManholeCover;
    public PlaceableObjectSO BookStore;
    public PlaceableObjectSO Restaurant;
    public PlaceableObjectSO Arboretum;
    [Header("�赲")]
    public PlaceableObjectSO Rock;
    public PlaceableObjectSO Wall;
    [Header("�²���Դ")]
    public PlaceableObjectSO FoodResource;
    public PlaceableObjectSO MineResource;
    [Header("ũ����")]
    public PlaceableObjectSO BeanSproutFarm;
    public PlaceableObjectSO FishygrassFarm;
    public PlaceableObjectSO MaizeFarm;
    public PlaceableObjectSO PeanutsFarm;
    public PlaceableObjectSO PumpkinFarm;


    public PlaceableObjectSO Trans;
    public PlaceableObjectSO Road;

    public Transform MousePrefab;

    //���������Լ��������ݴ洢
    //�²����罨��
    [HideInInspector] public List<PlaceableObject> L_House;
    [HideInInspector] public List<PlaceableObject> L_FoodFliter;
    [HideInInspector] public List<PlaceableObject> L_WareHouse;
    [HideInInspector] public List<PlaceableObject> L_StudyRoom;
    [HideInInspector] public List<PlaceableObject> L_Lamp;
    //�ϲ����罨��
    [HideInInspector] public List<PlaceableObject> L_ConstructionSite;
    [HideInInspector] public List<PlaceableObject> L_ManholeCover;
    [HideInInspector] public List<PlaceableObject> L_BookStore;
    [HideInInspector] public List<PlaceableObject> L_Restaurant;
    [HideInInspector] public List<PlaceableObject> L_Arboretum;
    //�赲
    [HideInInspector] public List<PlaceableObject> L_Rock;
    [HideInInspector] public List<PlaceableObject> L_Wall;
    //��Դ��
    [HideInInspector] public List<PlaceableObject> L_FoodResource;
    [HideInInspector] public List<PlaceableObject> L_MineResource;
    //ũ����
    [HideInInspector] public List<PlaceableObject> L_BeanSproutFarm;
    [HideInInspector] public List<PlaceableObject> L_FishygrassFarm;
    [HideInInspector] public List<PlaceableObject> L_MaizeFarm;
    [HideInInspector] public List<PlaceableObject> L_PeanutsFarm;
    [HideInInspector] public List<PlaceableObject> L_PumpkinFarm;

    [HideInInspector] public List<PlaceableObject> L_Trans;
    [HideInInspector] public List<PlaceableObject> L_Road;

    [Header("������Դ����")]
    public int FoodResourceNum;
    public int MineResourceNum;

    [Header("��Դ�ɼ��ʹ洢ʱ��")]
    public int MineCollectTime;
    public int FoodCollectTime;
    public int MineStoreTime;
    public int FoodStoreTime;
    [Header("ÿ�βɼ�����Դ����")]
    public int MineNumEachTime;
    public int FoodNumEachTime;

    [Header("UI")]
    public GameObject WinPanel;

    //������ֵ
    [Header("������ֵ")]
    public int KnowledgeLevel;
    [Header("���ֵ")]
    public int HateValue;//Խ�ʹ����Ѻ��¼� Խ�ߴ�������¼�
    [Header("��ʼʳ��洢����")]
    public int FoodStoreLimit;

    //ʳ��洢��
    public int FoodStore;
    //�������
    [Header("��������")]
    public int BuildingMaterials;
    //��������/�ж�����
    [HideInInspector] public int DiceNum;
    //�˿�����
    [HideInInspector] public int Population;
    //��������
    public int SeedStore;

    //��ǰʱ��
    [HideInInspector] public int cur_Day;
    public int EachDayTime = 60;
    private double TimerCounter = 0;

    [Header("��Ҫ��ֵ�����")]
    public int DayNumToDisaster = 100;
    [Header("��Ҫ�洢��ʳ������")]
    public int FoodStoreNeeded = 1000;

    //���������Լ��������ݴ洢
    [HideInInspector]public int MouseCount;
    [HideInInspector] public List<C_Mouse> Mouse_List;

    //�浵���б�
    [HideInInspector]public List<string> L_ArchivesName;

    //�������Ѽ��ʱ��
    private int ReminderIntervalTime = 5;
    private float IntervaltimeCounter;
    private bool isShowReminder = true;
    private bool[] showHateValueReminder = new bool[5]; 

    //�صƷ��Ӽ���
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
        //�����¼������һ�������ǳ��ֵ
        MotionClass motion1 = new MotionClass("��������", "������ԵĶ�����ô�࣬�ǵ�Ӧ�ò��ᱻ���ְɡ����������롣	",
                                  "���ӵ���>4",
                                  4, ResourceType.Food, 30, 3);

        MotionClass motion2 = new MotionClass("���ͳ�������", "ֻ�ǰ�æ���������ѣ�˳�������������õĶ���Ӧ��û�°ɣ�	",
                                             "���ӵ���>5",
                                             6, ResourceType.Food, 10, -1);


        Restaurant.buildingMotions.Add(motion1);
        Restaurant.buildingMotions.Add(motion2);

        //����¼�
        MotionClass motion3 = new MotionClass("���������", "��ͼ��ĺ���Ȥ��������ô����������Ķ���������ʲô���أ�	",
                          "���ӵ���>2",
                          2, ResourceType.Knowledge, 3, 0);

        MotionClass motion4 = new MotionClass("���ǡ��ҵ���", "�Ȿ���ͼ����������	",
                                             "���ӵ���>6",
                                             6, ResourceType.BuildingMaterials, 40, 2);

        BookStore.buildingMotions.Add(motion3);
        BookStore.buildingMotions.Add(motion4);

        //���������¼�
        MotionClass motion5 = new MotionClass("���������", "��һ��������ˮ�ͻ����̣�����",
                  "���ӵ���>4",
                  4, ResourceType.BuildingMaterials, 25, 3);

        MotionClass motion6 = new MotionClass("����������", "��Щ���ӵĶ��Ӻ�Σ�յģ���������ʰ�ˣ�",
                                             "���ӵ���>5",
                                             5, ResourceType.BuildingMaterials, 10, -1);

        ConstructionSite.buildingMotions.Add(motion5);
        ConstructionSite.buildingMotions.Add(motion6);

        //ֲ��԰
        MotionClass motion7 = new MotionClass("��ȡ����", "���иպ���ͼ��˵����Щ���Ӽ��Ժ󳤴������",
          "���ӵ���>4",
          4, ResourceType.Seed, 2, 2);

        MotionClass motion8 = new MotionClass("��ȡ����++", "���õ�ɣ��Ϳ��Զ��ֵ���",
                                             "���ӵ���>6",
                                             6, ResourceType.Seed, 5, 4);

        Arboretum.buildingMotions.Add(motion7);
        Arboretum.buildingMotions.Add(motion8);
    }

    private void Update()
    {
        TimeCount();
        //�ж���Ϸ�ɹ��Լ�ʧ��
        if(cur_Day == DayNumToDisaster)
        {
            if(FoodStore > FoodStoreNeeded || FoodStore == FoodStoreNeeded)
            {
                WinPanel.gameObject.SetActive(true);
                //Debug.Log("��Ϸ�ɹ�");
            }
            else
            {
                //Debug.Log("��Ϸʧ��");
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
                View.Instance.AddInfoPanel("����: ", "ʳ��洢���ﵽ���ޣ��������Ĳֿ������ʳ��洢����");
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
                View.Instance.AddInfoPanel("����: ", "���������ĳ��ֵ����20���ϲ��������࿪ʼ��߾��裬ִ���¼��ɹ��ĸ��ʻή��");
                showHateValueReminder[0] = false;
                decreaseProbility(1);
            }      
        }
        if (HateValue > 40 || HateValue == 40)
        {
            if (showHateValueReminder[1] == true)
            {
                View.Instance.AddInfoPanel("����: ", "���������ĳ��ֵ����40���ϲ��������ೢ�Ը�Ԥ��������Ľ��裬�������ϼ���50");
                showHateValueReminder[1] = false;
                BuildingMaterials -= 50;
            }
        }
        if (HateValue > 60 || HateValue == 60)
        {
            if (showHateValueReminder[2] == true)
            {
                View.Instance.AddInfoPanel("����: ", "���������ĳ��ֵ����60���ϲ��������࿪ʼ��ȡ������������ִ���¼��ɹ��ĸ��ʴ������");
                showHateValueReminder[2] = false;
                decreaseProbility(1);
            }
        }
        if (HateValue > 80 || HateValue == 80)
        {
            if (showHateValueReminder[3] == true)
            {
                View.Instance.AddInfoPanel("����: ", "���������ĳ��ֵ����80������ӱ���Ԥ��������Ľ��裬ʳ����ʧ200������������ʧ100");
                showHateValueReminder[3] = false;
                FoodStore -= 200;
                BuildingMaterials -= 100;
            }
        }
        if (HateValue > 100 || HateValue == 100)
        {
            if (showHateValueReminder[4] == true)
            {
                View.Instance.AddInfoPanel("����: ", "���������ĳ��ֵ�ﵽ���ޣ�ȫ��Χ�������޷���ִ���ϲ��¼�");
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
                View.Instance.AddInfoPanel("�¼�: ", "����û���㹻��ʳ���" + DecreaseMouseNum + "ֻ�������ڼ���");
                FoodStore = 0;
            }
            else
            {
                FoodStore -= MouseCount * 2;
            }
        }
    }
}  
