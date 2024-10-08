using System.Collections;
using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Grid;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using PathFinding;
public enum PlayerMode
{
    Build, Select, Delete, UIMode, UpWorldHandle, ResourceSelect
}
//����ʱ�����н����ŵ�һ��ö���£�������Ҫ�ٷֿ�
public enum PlacaebleObjectCategories
{
    House, FoodFliter, WareHouse,StudyRoom, Light,
    ConstructionSite, Arboretum, BookStore, Restaurant,
    Rock, Wall,
    FoodResource, MineResource,
    Trans,Road,
    BeanSproutFarm, FishygrassFarm, MaizeFarm, PeanutsFarm, PumpkinFarm,
    Others
}
//----------------------------------------------------
//���������½�����Ҫ�޸ĵĵط���PlacaebleObjectCategoriesö���࣬
//Model�е������б�����ֵ�ӳ�䣬������Ӻ�ɾ�������ķ���
//----------------------------------------------------------

public class Controller : Singleton<Controller>
{
    public int testCounter = 0;

    //�������ڵ�
    public Transform UnderBuildingParent;
    public Transform UpBuildingParent;
    public Transform UpWord;
    public Transform My_Camera;
    private Transform BuildingParent;
    // Start is called before the first frame update
    private PlayerInput m_PlayerInput;
    private InputAction m_InputAction;
    //----------------����ʱ����----------------
    [HideInInspector]public PlayerMode cur_PlayerMode;
    [HideInInspector]public PlaceableObjectSO cur_PlaceableObjectSO;
    private PlaceableObjectSO.Dir dir = PlaceableObjectSO.Dir.Down;
    //����ö�ٵ�����pleacableObject���ֵ�ӳ��
    private Dictionary<PlacaebleObjectCategories, PlaceableObjectSO> D_PlacaebleObject = new Dictionary<PlacaebleObjectCategories, PlaceableObjectSO>();

    //��PlacedBuildingӳ�䵽�ϲ㽨����Dictionary
    private Dictionary<PlacaebleObjectCategories, TopBuilding> D_PlaceableToEventBuilding = new Dictionary<PlacaebleObjectCategories, TopBuilding>();

    //�Ƿ���ʾ�ϲ�����
    [HideInInspector]public bool isOnUpWord = false;
    //�Ƿ��Ǳ༭ģʽ
    public bool isEditModel = true;

    //�����λ�ü�¼
    private Vector3 curWordCameraPosition;
    private Vector3 targetWordCameraPosition;
    private bool isCameraOnMove = false;
    public float moveAmount = 5;    //�ƶ��ٶ�
    
    private Vector2 wasdInputVec2;
    private Vector3 cameraPos;

    //NotePanel
    [Header("��ʾ����")]
    public Image NotePanel;


    //----------------�¼�----------------
    public Action<PlayerMode> OnPlayerModeChanged;
    public Action OnBuildingChanged;
    public Action OnBuildingNumChanged;
    public Action OnHouseBuilt;

    //Ԥɾ������
    private PlaceableObject pre_DeleteBuilding;
    private Color pre_DeleteBuildingColor;
    private Color FoodResourceColor;
    private Color MineResourceColor;

    //��ǰ�ٿص���Դ�ɼ�վ
    private PlaceableObject cur_FoodFliter;
    //Grid
    [HideInInspector]public GridXZ<GridObject> cur_grid;

    //�Ƿ��Ѿ�����
    private bool isOnLoad = true;
    //�Ƿ���Խ���
    [HideInInspector] public bool isCanBuild = true;
    [Header("������ƶ�Ч���ı�Ե���")]
    public float edgeSize = 0.1F;	//������ƶ�Ч���ı�Ե���

    public bool canDoMoition = true;

    private float halfCellseize;

    //��Ч
    public AudioClip MouseOnUI;
    public AudioClip MouseClick;
    public AudioClip InfoPanelShow;
    public AudioClip PlaceBuildingAudio;
    [HideInInspector]public AudioSource audioS;
    void Start()
    {
        //��ʼ��
        m_PlayerInput = GetComponent<PlayerInput>();
        cur_grid = GridBuildingSystem.Instance.grid;
        cur_PlaceableObjectSO = Model.Instance.House;

        halfCellseize = GridBuildingSystem.Instance.cellSize / 2;
        audioS = GetComponent<AudioSource>();
        //������Ч
        //RenderSettings.fog = true;
        //��������
        LoadGrid();

        //���������Դ
        BuildingParent = UnderBuildingParent;
        //FoodAndMineResourceGenerate(Model.Instance.MineResourceNum, Model.Instance.MineResource);
        //FoodAndMineResourceGenerate(Model.Instance.FoodResourceNum, Model.Instance.FoodResource);

        //�����ֵ��ʼ��
        D_PlacaebleObject.Add(PlacaebleObjectCategories.House, Model.Instance.House);
        D_PlacaebleObject.Add(PlacaebleObjectCategories.WareHouse, Model.Instance.WareHouse);
        D_PlacaebleObject.Add(PlacaebleObjectCategories.FoodFliter, Model.Instance.FoodFliter);
        D_PlacaebleObject.Add(PlacaebleObjectCategories.BeanSproutFarm, Model.Instance.BeanSproutFarm);
        //�ϲ�
        D_PlacaebleObject.Add(PlacaebleObjectCategories.BookStore, Model.Instance.BookStore);
        D_PlacaebleObject.Add(PlacaebleObjectCategories.Restaurant, Model.Instance.Restaurant);
        D_PlacaebleObject.Add(PlacaebleObjectCategories.ConstructionSite, Model.Instance.ConstructionSite);
        D_PlacaebleObject.Add(PlacaebleObjectCategories.Arboretum, Model.Instance.ManholeCover);
        //�赲
        D_PlacaebleObject.Add(PlacaebleObjectCategories.Rock, Model.Instance.Rock);
        D_PlacaebleObject.Add(PlacaebleObjectCategories.Wall, Model.Instance.Wall);


        //��ʼ������ӳ���ֵ�
        D_PlaceableToEventBuilding.Add(PlacaebleObjectCategories.Restaurant,TopUIMgr.Instance.Restaurant);
        D_PlaceableToEventBuilding.Add(PlacaebleObjectCategories.BookStore, TopUIMgr.Instance.BookStore);
        D_PlaceableToEventBuilding.Add(PlacaebleObjectCategories.Arboretum, TopUIMgr.Instance.Arboretum);
        D_PlaceableToEventBuilding.Add(PlacaebleObjectCategories.ConstructionSite, TopUIMgr.Instance.ConstructionSite);

        OnPlayerModeChanged += ChangeToResourceSelectModeHander;
        //��ɫ����
        FoodResourceColor = Model.Instance.L_FoodResource[0].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color;
        MineResourceColor = Model.Instance.L_MineResource[0].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color;

        //������
        m_PlayerInput.onActionTriggered += callBack =>
        {
            if (callBack.action.name == "Click")
            {
                if (callBack.phase == InputActionPhase.Performed)
                {
                    if (!IsPointerOverGameObject(Input.mousePosition))
                    {
                        if (cur_PlayerMode == PlayerMode.Build)
                        {
                            if (isCanBuild)
                            {
                                var mousePosition = MousePositionUtils.MouseToTerrainPosition();
                                cur_grid.GetXZ(mousePosition, out int x, out int z);

                                PlaceBuilding(cur_PlaceableObjectSO, x, z, dir);
                            }
                            else
                            {
                                View.Instance.AddInfoPanel("����: ", "���������ｨ��");
                            }
                        }
                        else if (cur_PlayerMode == PlayerMode.Delete)
                        {
                            if (pre_DeleteBuilding != null)
                            {
                                //RemovePlacedObejct(pre_DeleteBuilding);
                                //pre_DeleteBuilding.DestroySelf();
                                //var gridPosList = pre_DeleteBuilding.GetGridPositionList();
                                //foreach (var gridPos in gridPosList)
                                //{
                                //    cur_grid.GetGridObject(gridPos.x, gridPos.y).PlaceableObject = pre_DeleteBuilding;
                                //}
                                //if(pre_DeleteBuilding.placeableObjectSO.category == PlacaebleObjectCategories.Light)
                                //{
                                //    SetFarmIncreaseRate(false, gridPosList[0]);
                                //}

                                //ɾ���󱣴滹�ǻ���س���
                                bool canDelete = true;

                                if (pre_DeleteBuilding.placeableObjectSO.category == PlacaebleObjectCategories.House)
                                {
                                    if(pre_DeleteBuilding.isMouseLiving == true)
                                    {
                                        View.Instance.AddInfoPanel("����: ", "�㲻��ɾ�������������Ϊ�������������ס");
                                        canDelete = false;
                                    }
                                }
                                if(pre_DeleteBuilding.placeableObjectSO.MouseNeeded != 0)
                                {
                                    for (var i = 0; i < pre_DeleteBuilding.placeableObjectSO.MouseNeeded; i++)
                                    {
                                        var tmp_Mouse = pre_DeleteBuilding.L_WorkingMouse[0];
                                        Model.Instance.Mouse_List.Add(tmp_Mouse);
                                        tmp_Mouse.cur_WorkingBuilding = null;
                                        tmp_Mouse.cur_WorkingResource = null;
                                        StopAllCoroutines();
                                        tmp_Mouse.ConvertWorkType(WorkType.Idler);
                                        pre_DeleteBuilding.L_WorkingMouse.Remove(tmp_Mouse);
                                    }
                                }

                                if (canDelete)
                                {
                                    RemovePlacedObejct(pre_DeleteBuilding);
                                    if(pre_DeleteBuilding.placeableObjectSO.buildingMaterialCost > 0)
                                    {
                                        Model.Instance.BuildingMaterials += pre_DeleteBuilding.placeableObjectSO.buildingMaterialCost / 2;
                                    }
                                    var gridPosList = pre_DeleteBuilding.GetGridPositionList();
                                    if (pre_DeleteBuilding.placeableObjectSO.category == PlacaebleObjectCategories.Light)
                                    {
                                        SetFarmIncreaseRate(false, gridPosList[0]);
                                    }
                                    foreach (var gridPos in gridPosList)
                                    {
                                        cur_grid.GetGridObject(gridPos.x, gridPos.y).ClearPlaceableObject();
                                    }
                                    pre_DeleteBuilding.DestroySelf();
                                }
                            }
                        }
                        else if (cur_PlayerMode == PlayerMode.UpWorldHandle)
                        {
                            var ray = Camera.main.ScreenPointToRay(MousePositionUtils.GetMousePos());
                            if (Physics.Raycast(ray, out RaycastHit info, 10000, LayerMask.GetMask("UpBuilding")))
                            {
                                if (info.transform.parent.parent.GetComponent<PlaceableObject>().placeableObjectSO.NameString != null)
                                {
                                    var name = info.transform.parent.parent.GetComponent<PlaceableObject>().placeableObjectSO.NameString;
                                    var type = info.transform.parent.parent.GetComponent<PlaceableObject>().placeableObjectSO.category;
                                    if(type == PlacaebleObjectCategories.Restaurant || type == PlacaebleObjectCategories.Arboretum || type == PlacaebleObjectCategories.BookStore || type == PlacaebleObjectCategories.ConstructionSite)
                                    {
                                        if (canDoMoition)
                                        {
                                            switch (type)
                                            {
                                                case PlacaebleObjectCategories.Restaurant:
                                                    TopUIMgr.Instance.showPanel(TopUIMgr.Instance.topPanel);
                                                    TopUIMgr.Instance.setToggle(TopUIMgr.Instance.Restaurant);
                                                    TopUIMgr.Instance.motionShow(); break;
                                                case PlacaebleObjectCategories.Arboretum:
                                                    TopUIMgr.Instance.showPanel(TopUIMgr.Instance.topPanel);
                                                    TopUIMgr.Instance.setToggle(TopUIMgr.Instance.Arboretum);
                                                    TopUIMgr.Instance.motionShow(); break;
                                                case PlacaebleObjectCategories.BookStore:
                                                    TopUIMgr.Instance.showPanel(TopUIMgr.Instance.topPanel);
                                                    TopUIMgr.Instance.setToggle(TopUIMgr.Instance.BookStore);
                                                    TopUIMgr.Instance.motionShow(); break;
                                                case PlacaebleObjectCategories.ConstructionSite:
                                                    TopUIMgr.Instance.showPanel(TopUIMgr.Instance.topPanel);
                                                    TopUIMgr.Instance.setToggle(TopUIMgr.Instance.ConstructionSite);
                                                    TopUIMgr.Instance.motionShow(); break;
                                            }
                                        }
                                        else
                                        {
                                            View.Instance.AddInfoPanel("����: ", "���������ĳ��ֵ�ﵽ���ޣ�ȫ��Χ���������޷���ִ���ϲ��¼�");
                                        }
                                    }
                                    
                                }
                            }
                        }
                        else if (cur_PlayerMode == PlayerMode.Select)
                        {
                            var ray = Camera.main.ScreenPointToRay(MousePositionUtils.GetMousePos());
                            if (Physics.Raycast(ray, out RaycastHit info, 20000, LayerMask.GetMask("UnderBuiding")))
                            {
                                if (info.transform.parent.parent.GetComponent<PlaceableObject>().placeableObjectSO.NameString == "FoodFliter")
                                {
                                    cur_FoodFliter = info.transform.parent.parent.GetComponent<PlaceableObject>();
                                    ConvertPlayerMode(PlayerMode.ResourceSelect);
                                }
                            }
                            //var mousePosition = MousePositionUtils.MouseToTerrainPosition();
                            //GridBuildingSystem.Instance.pf_grid.GetXZ(mousePosition, out int x, out int z);
                            //Debug.Log("(" + x + "," + z + "): " + GridBuildingSystem.Instance.pf_grid.GetGridObject(x, z).IsWalkable);

                        }
                        else if (cur_PlayerMode == PlayerMode.ResourceSelect)
                        {
                            var ray = Camera.main.ScreenPointToRay(MousePositionUtils.GetMousePos());
                            if (Physics.Raycast(ray, out RaycastHit info, 20000, LayerMask.GetMask("UnderBuiding")))
                            {
                                if (info.transform.parent.parent.GetComponent<PlaceableObject>().placeableObjectSO.attribute == Attribute.Resources)
                                {
                                    cur_FoodFliter.isStartCollectResource = true;

                                    pre_DeleteBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = pre_DeleteBuildingColor;
                                    pre_DeleteBuilding = null;
                                    pre_DeleteBuildingColor = new Color(0, 0, 0);

                                    cur_FoodFliter.curBindingResource = info.transform.parent.parent.GetComponent<PlaceableObject>();
                                    ConvertPlayerMode(PlayerMode.Select);
                                    StartCoroutine(WaitToExcute(0.5f, cur_FoodFliter.L_WorkingMouse.Count, info, cur_FoodFliter));
                                }
                                else if(info.transform.parent.parent.GetComponent<PlaceableObject>().placeableObjectSO.NameString == "FoodFliter")
                                {
                                    cur_FoodFliter = info.transform.parent.parent.GetComponent<PlaceableObject>();
                                }
                            }
                        }
                    }
                }
            }else if(callBack.action.name == "MousePosition"){

            }
            else if (callBack.action.name == "Build")
            {
                if (callBack.phase == InputActionPhase.Performed)
                {
                    if ((cur_PlayerMode != PlayerMode.Build && cur_PlayerMode != PlayerMode.UpWorldHandle) || isEditModel == true)
                    {
                        ConvertPlayerMode(PlayerMode.Build);
                    }
                    else if (cur_PlayerMode == PlayerMode.Build)
                    {
                        if (isOnUpWord == false)
                        {
                            ConvertPlayerMode(PlayerMode.Select);
                        }
                        else
                        {
                            ConvertPlayerMode(PlayerMode.UpWorldHandle);
                        }
                    }
                }
            }
            else if (callBack.action.name == "Delete")
            {
                if (callBack.phase == InputActionPhase.Performed)
                {
                    if ((cur_PlayerMode != PlayerMode.Delete && cur_PlayerMode != PlayerMode.UpWorldHandle) || isEditModel == true)
                    {
                        ConvertPlayerMode(PlayerMode.Delete);
                    }
                    else if (cur_PlayerMode == PlayerMode.Delete)
                    {
                        ConvertPlayerMode(PlayerMode.Select);
                    }
                }
            }
            else if (callBack.action.name == "RightClick")
            {
                if (callBack.phase == InputActionPhase.Performed)
                {
                    if (cur_PlayerMode == PlayerMode.Build)
                    {
                        if (isOnUpWord == false)
                        {
                            ConvertPlayerMode(PlayerMode.Select);
                        }
                        else
                        {
                            ConvertPlayerMode(PlayerMode.UpWorldHandle);
                        }
                    }
                    else if (cur_PlayerMode == PlayerMode.Delete)
                    {
                        ConvertPlayerMode(PlayerMode.Select);
                        if (pre_DeleteBuilding != null)
                        {
                            pre_DeleteBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = pre_DeleteBuildingColor;
                            pre_DeleteBuilding = null;
                            pre_DeleteBuildingColor = new Color(0, 0, 0);
                        }
                    }
                    else if (cur_PlayerMode == PlayerMode.ResourceSelect)
                    {
                        ConvertPlayerMode(PlayerMode.Select);
                    }
                }
            }
            else if (callBack.action.name == "ChangeWord")
            {
                if(callBack.phase == InputActionPhase.Performed)
                {
                    if (isCameraOnMove == false)
                    {
                        if (isOnUpWord == false)
                        {
                            ConvertPlayerMode(PlayerMode.UpWorldHandle);
                            UpWord.gameObject.SetActive(true);
                            UpBuildingParent.gameObject.SetActive(true);
                            //RenderSettings.fog = false;
                            if (isEditModel == true)
                            {
                                cur_grid = GridBuildingSystem.Instance.Up_grid;
                            }
                            curWordCameraPosition = My_Camera.transform.position;
                            targetWordCameraPosition = new Vector3(My_Camera.position.x, My_Camera.position.y + GridBuildingSystem.Instance.UpWorldHeigh, My_Camera.position.z);
                            isOnUpWord = true;
                            isCameraOnMove = true;
                        }
                        else
                        {
                            ConvertPlayerMode(PlayerMode.Select);
                            UpWord.gameObject.SetActive(false);
                            UpBuildingParent.gameObject.SetActive(false);
                            //RenderSettings.fog = true;
                            if (isEditModel == true)
                            {
                                cur_grid = GridBuildingSystem.Instance.grid;
                            }
                            curWordCameraPosition = My_Camera.transform.position;
                            targetWordCameraPosition = new Vector3(My_Camera.position.x, My_Camera.position.y - GridBuildingSystem.Instance.UpWorldHeigh, My_Camera.position.z);
                            isOnUpWord = false;
                            isCameraOnMove = true;
                        }
                    }
                }
            }
            else if (callBack.action.name == "Rotate")
            {
                if (callBack.phase == InputActionPhase.Performed)
                {
                    dir = PlaceableObjectSO.GetNextDir(dir);
                }
            }
            else if (callBack.action.name == "CameraMovement")
            {
                wasdInputVec2 = callBack.ReadValue<Vector2>();
                cameraPos = My_Camera.transform.position;
            }
        };
    }
    void Update()
    {
        if (cur_PlayerMode == PlayerMode.Delete)
        {
            if (isEditModel)
            {
                if (isOnUpWord)
                {
                    MouseOnColorChange(Attribute.UpBuilding, Color.red);
                }
                else
                {
                    MouseOnColorChange(Attribute.Resources, Color.red);
                }
            }
            else
            {
                //�����ָ��Ľ�����Ϊ��ɫ
                MouseOnColorChange(Attribute.UnderBuilding, Color.red);
            }
        }
        else if (cur_PlayerMode == PlayerMode.ResourceSelect)
        {
            MouseOnColorChange(Attribute.Resources, Color.blue);
        }
        else if (cur_PlayerMode == PlayerMode.UpWorldHandle)
        {
            MouseOnColorChange(Attribute.UpBuilding, Color.grey);
        }
        else if (cur_PlayerMode == PlayerMode.Select)
        {
            MouseOnColorChange(PlacaebleObjectCategories.FoodFliter, Color.grey);
        }
    }
    private void LateUpdate()
    {
        if ((Input.mousePosition.x > Screen.width - edgeSize || Input.mousePosition.x < edgeSize || Input.mousePosition.y > Screen.height - edgeSize || Input.mousePosition.y < edgeSize) && !isCameraOnMove)
        {
            cameraPos = My_Camera.transform.position;
            if (Input.mousePosition.x > Screen.width - edgeSize && cameraPos.x < 135)//������λ�����Ҳ�
            {
                cameraPos.x += moveAmount * Time.deltaTime;//�������ƶ�
            }
            if (Input.mousePosition.x < edgeSize && cameraPos.x > 55)
            {
                cameraPos.x -= moveAmount * Time.deltaTime;
            }
            if (Input.mousePosition.y > Screen.height - edgeSize && cameraPos.z < 140)
            {
                cameraPos.z += moveAmount * Time.deltaTime;
            }
            if (Input.mousePosition.y < edgeSize && cameraPos.z > 10)
            {
                cameraPos.z -= moveAmount * Time.deltaTime;
            }
            My_Camera.transform.position = cameraPos;
        }
        //if (wasdInputVec2 != Vector2.zero && !isCameraOnMove)// && cameraPos.x < 135 && cameraPos.x > 55 && cameraPos.z < 140 && cameraPos.z > 10)
        //{
        //    Vector3 moveDirection = new Vector3(wasdInputVec2.x, 0, wasdInputVec2.y);
        //    cameraPos += moveDirection * Time.deltaTime * moveAmount;
        //    My_Camera.transform.position = cameraPos;
        //}
        if (Keyboard.current != null && !isCameraOnMove && (Keyboard.current.wKey.isPressed || Keyboard.current.aKey.isPressed || Keyboard.current.sKey.isPressed || Keyboard.current.dKey.isPressed))
        {
            cameraPos = My_Camera.transform.position;
            if (Keyboard.current.wKey.isPressed && cameraPos.z < 140)
            {
                cameraPos.z += moveAmount * Time.deltaTime;
            }
            if (Keyboard.current.aKey.isPressed && cameraPos.x > 55)
            {
                cameraPos.x -= moveAmount * Time.deltaTime;
            }
            if (Keyboard.current.sKey.isPressed && cameraPos.z > 10)
            {
                cameraPos.z -= moveAmount * Time.deltaTime;
            }
            if (Keyboard.current.dKey.isPressed && cameraPos.x < 135)
            {
                cameraPos.x += moveAmount * Time.deltaTime;//�������ƶ�
            }
            My_Camera.transform.position = cameraPos;
        }
        //Camera�ƶ�
        if (isCameraOnMove)
        {
            if (Mathf.Abs(My_Camera.position.y - targetWordCameraPosition.y) > 0.5f)
            {
                My_Camera.position = Vector3.Lerp(My_Camera.position, targetWordCameraPosition, Time.deltaTime * 5f);
            }
            else
            {
                My_Camera.position = targetWordCameraPosition;
                isCameraOnMove = false;
            }
        }
    }
    IEnumerator WaitToExcute(float time, int excuteTime, RaycastHit info, PlaceableObject foodFilter)
    {
        if(excuteTime > 0)
        {
            var tmpMouse = foodFilter.L_WorkingMouse[excuteTime - 1];
            tmpMouse.cur_WorkingBuilding = foodFilter;
            tmpMouse.cur_WorkingResource = info.transform.parent.parent.GetComponent<PlaceableObject>();
            tmpMouse.ConvertWorkType(WorkType.ResourceCollecter);
        }
        yield return new WaitForSeconds(time);
        StartCoroutine(WaitToExcute(time, excuteTime - 1, info, foodFilter));
    }
    private void MouseOnColorChange(Attribute attribute, Color color)
    {
        //var gridObject = grid.GetGridObject(MousePositionUtils.MouseToTerrainPosition()); ���λ��ӳ�䵽����
        PlaceableObject placedObject;
        var ray = Camera.main.ScreenPointToRay(MousePositionUtils.GetMousePos());
        if (Physics.Raycast(ray, out RaycastHit info, 10000, LayerMask.GetMask("UpBuilding", "UnderBuiding")))
        {
            placedObject = info.transform.parent.parent.GetComponent<PlaceableObject>();
        }
        else
        {
            placedObject = null; 
        }
        bool changeColor = false;
        if (placedObject != null)
        {
            if (placedObject.placeableObjectSO.attribute == Attribute.Farm)
            {
                changeColor = true;
            }
        }
        //var placedObject = gridObject.PlaceableObject;
        if (placedObject != null && (placedObject.placeableObjectSO.attribute == attribute || changeColor == true))// && typeof(PlaceableObject).IsAssignableFrom(placedObject.GetType()))
        {
            if (pre_DeleteBuilding == null)
            {
                pre_DeleteBuildingColor = placedObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color;
                pre_DeleteBuilding = placedObject;
                pre_DeleteBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = color;
            }
            else if (placedObject != pre_DeleteBuilding)
            {
                pre_DeleteBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = pre_DeleteBuildingColor;
                pre_DeleteBuildingColor = placedObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color;
                pre_DeleteBuilding = placedObject;
                pre_DeleteBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = color;
            }
        }
        else if (placedObject == null && pre_DeleteBuilding != null)
        {
            pre_DeleteBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = pre_DeleteBuildingColor;
            pre_DeleteBuilding = null;
            pre_DeleteBuildingColor = new Color(0, 0, 0);
        }
    }
    private void MouseOnColorChange(PlacaebleObjectCategories category, Color color)
    {
        //var gridObject = grid.GetGridObject(MousePositionUtils.MouseToTerrainPosition()); ���λ��ӳ�䵽����
        PlaceableObject placedObject;
        var ray = Camera.main.ScreenPointToRay(MousePositionUtils.GetMousePos());
        if (Physics.Raycast(ray, out RaycastHit info, 10000, LayerMask.GetMask("UpBuilding", "UnderBuiding")))
        {
            placedObject = info.transform.parent.parent.GetComponent<PlaceableObject>();
        }
        else
        {
            placedObject = null;
        }
        //var placedObject = gridObject.PlaceableObject;
        if (placedObject != null && placedObject.placeableObjectSO.category == category)// && typeof(PlaceableObject).IsAssignableFrom(placedObject.GetType()))
        {
            if (pre_DeleteBuilding == null)
            {
                pre_DeleteBuildingColor = placedObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color;
                pre_DeleteBuilding = placedObject;
                pre_DeleteBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = color;
            }
            else if (placedObject != pre_DeleteBuilding)
            {
                pre_DeleteBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = pre_DeleteBuildingColor;
                pre_DeleteBuildingColor = placedObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color;
                pre_DeleteBuilding = placedObject;
                pre_DeleteBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = color;
            }
        }
        else if (placedObject == null && pre_DeleteBuilding != null)
        {
            pre_DeleteBuilding.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = pre_DeleteBuildingColor;
            pre_DeleteBuilding = null;
            pre_DeleteBuildingColor = new Color(0, 0, 0);
        }
    }

    private void PlaceBuilding(PlaceableObjectSO placeableObjectSO, int x, int z, PlaceableObjectSO.Dir direction)
    {
        var gridPosList = placeableObjectSO.GetGridPositionList(new Vector2Int(x, z), direction);

        var canBuild = true;
        var MouseCountcanBuild = true;
        var MaterialcanBuild = true;
        var SeedcanBuild = true;
        var KnowledgeCanBuild = true;
        foreach (var gridPos in gridPosList)
        {
            cur_grid.GetGridObject(gridPos.x, gridPos.y).Direction = direction;
            if (!cur_grid.GetGridObject(gridPos.x, gridPos.y).CanBuild)
            {
                canBuild = false;
                break;
            }
        }
        if (placeableObjectSO.buildingMaterialCost > Model.Instance.BuildingMaterials)
        {
            MaterialcanBuild = false;
        }
        if (placeableObjectSO.MouseNeeded != 0 && Model.Instance.Mouse_List.Count < placeableObjectSO.MouseNeeded)
        {
            MouseCountcanBuild = false;
        }
        if (placeableObjectSO.seedsCost != 0 && Model.Instance.SeedStore < placeableObjectSO.seedsCost)
        {
            SeedcanBuild = false;
        }
        if(placeableObjectSO.KnowledgeNeeded != 0 && Model.Instance.KnowledgeLevel < placeableObjectSO.KnowledgeNeeded)
        {
            KnowledgeCanBuild = false;
        }

        if (canBuild && MouseCountcanBuild && MaterialcanBuild && SeedcanBuild && KnowledgeCanBuild)
        {
            if (!isOnLoad)
            {
                audioS.PlayOneShot(PlaceBuildingAudio);
            }
            var rotOffset = placeableObjectSO.GetRotationOffset(direction);
            var placedWorldPos = cur_grid.GetWorldPosition(x, z) + new Vector3(rotOffset.x, 0, rotOffset.y) * cur_grid.CellSize;
            var pf_gridPosList = placeableObjectSO.pf_GetGridPositionList(new Vector2Int(x, z), direction);

            var placedObject = PlaceableObject.Create(
                placedWorldPos,
                new Vector2Int(x, z), direction,
                placeableObjectSO, BuildingParent);
            //�洢����Ķ���
            StorePlacedObject(placedObject);
            foreach (var gridPos in gridPosList)
            {
                var gridTobuild = cur_grid.GetGridObject(gridPos.x, gridPos.y);
                gridTobuild.PlaceableObject = placedObject;
                gridTobuild.PlaceableObjectName = placedObject.placeableObjectSO.NameString;
                gridTobuild.Direction = placedObject.dir;
            }
            //pathfinding�������
            foreach (var gridPos in pf_gridPosList)
            {
                var pf_gridTobuild = GridBuildingSystem.Instance.pf_grid.GetGridObject(gridPos.x, gridPos.y);
                pf_gridTobuild.PlaceableObject = placedObject;
                pf_gridTobuild.PlaceableObjectName = placedObject.placeableObjectSO.NameString;
                pf_gridTobuild.Direction = placedObject.dir;
            }
            //���Ľ������� ����
            Model.Instance.BuildingMaterials -= placedObject.placeableObjectSO.buildingMaterialCost;
            if(placeableObjectSO.seedsCost != 0)
            {
                Model.Instance.SeedStore -= placedObject.placeableObjectSO.seedsCost;
            }

            if (placedObject.placeableObjectSO.category == PlacaebleObjectCategories.House)
            {
                for(int i = 0; i < 5; i++)
                {
                    C_Mouse mouse = Instantiate(Model.Instance.MousePrefab, new Vector3(placedObject.transform.position.x + halfCellseize, placedObject.transform.position.y, placedObject.transform.position.z + halfCellseize)
                        , Quaternion.identity, Model.Instance.transform).GetComponent<C_Mouse>();
                    Model.Instance.Mouse_List.Add(mouse);
                    Model.Instance.MouseCount++;
                }
                OnHouseBuilt.Invoke();
                //StartCoroutine(WaitToCalculateIdlePath(0.5f, 5, placedObject));
            }else if (placedObject.placeableObjectSO.category == PlacaebleObjectCategories.WareHouse)
            {
                Model.Instance.FoodStoreLimit += 100;

            }
            else if (placedObject.placeableObjectSO.category == PlacaebleObjectCategories.Light)
            {
                SetFarmIncreaseRate(true, gridPosList[0]);
            }
            if(placedObject.placeableObjectSO.MouseNeeded != 0)
            {
                if (Model.Instance.Mouse_List.Count > placedObject.placeableObjectSO.MouseNeeded - 1)
                {
                    for (var i = 0; i < placedObject.placeableObjectSO.MouseNeeded; i++)
                    {
                        var tmp_Mouse = Model.Instance.Mouse_List[0];
                        placedObject.L_WorkingMouse.Add(tmp_Mouse);
                        tmp_Mouse.cur_WorkingBuilding = placedObject;
                        tmp_Mouse.ConvertWorkType(WorkType.StableBuildingWorker);
                        Model.Instance.Mouse_List.Remove(tmp_Mouse);
                    }
                    placedObject.isStartWorking = true;
                }
                else
                {
                    //Debug.Log("��ǰ������������");
                }
            }
            //���¼���·��
            //ReCalculatePath();
        }
        else if(!canBuild && !isOnLoad)
        {
            UtilsClass.CreateWorldTextPopup("���������ｨ��", cur_grid.GetWorldPosition(x, z));
            View.Instance.AddInfoPanel("����: ", "���������ｨ��");
        }
        else if (!MouseCountcanBuild)
        {
            UtilsClass.CreateWorldTextPopup("������������", cur_grid.GetWorldPosition(x, z));
            View.Instance.AddInfoPanel("����: ", "������������������,�������ס���������˿�����");
        }
        else if (!MaterialcanBuild)
        {
            UtilsClass.CreateWorldTextPopup("�������ϲ���", cur_grid.GetWorldPosition(x, z));
            View.Instance.AddInfoPanel("����: ", "������ϲ���");
        }
        else if (!SeedcanBuild)
        {
            UtilsClass.CreateWorldTextPopup("���Ӵ�������", cur_grid.GetWorldPosition(x, z));
            View.Instance.AddInfoPanel("����: ", "���Ӵ������㣬����Ҫ��ø��������");
        }
        else if (!KnowledgeCanBuild)
        {
            UtilsClass.CreateWorldTextPopup("�����ȼ�����", cur_grid.GetWorldPosition(x, z));
            View.Instance.AddInfoPanel("����: ", "�����ȼ����㣬����Ҫ�����鷿����������ȼ�");
        }
    }
    IEnumerator WaitToCalculateIdlePath(float time, int excuteTime, PlaceableObject placedObject)
    {
        if (excuteTime > 0)
        {
            C_Mouse mouse = Instantiate(Model.Instance.MousePrefab, placedObject.transform.position, Quaternion.identity, Model.Instance.transform).GetComponent<C_Mouse>();
            Model.Instance.Mouse_List.Add(mouse);
            Model.Instance.MouseCount++;
        }
        yield return new WaitForSeconds(time);
        StartCoroutine(WaitToCalculateIdlePath(time, excuteTime - 1, placedObject));
    }
    //ʹ����¼�������UI
    public bool IsPointerOverGameObject(Vector2 screenPosition)
    {
        //ʵ��������¼�
        PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        //�����λ�õ���Ļ���긳ֵ������¼�
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        //��������������
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
    public void ChangeToResourceSelectModeHander(PlayerMode playMode)
    {
        if(playMode == PlayerMode.ResourceSelect)
        {
            NotePanel.gameObject.SetActive(true);
            NotePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "��ѡ����Ҫ����������вɼ�����Դ��";
            foreach(var reource in Model.Instance.L_FoodResource)
            {
                reource.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = Color.green;
            }
            foreach (var reource in Model.Instance.L_MineResource)
            {
                reource.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = Color.green;
            }
        }
        else
        {
            NotePanel.gameObject.SetActive(false);
            foreach (var reource in Model.Instance.L_FoodResource)
            {
                reource.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = FoodResourceColor;
            }
            foreach (var reource in Model.Instance.L_MineResource)
            {
                reource.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.color = MineResourceColor;
            }
        }
    }
    public void StorePlacedObject(PlaceableObject placedObject)
    {
        switch (placedObject.placeableObjectSO.category)
        {
            case PlacaebleObjectCategories.House: Model.Instance.L_House.Add(placedObject); break;
            case PlacaebleObjectCategories.StudyRoom: Model.Instance.L_StudyRoom.Add(placedObject); break;
            case PlacaebleObjectCategories.FoodFliter: Model.Instance.L_FoodFliter.Add(placedObject); break;
            case PlacaebleObjectCategories.WareHouse: Model.Instance.L_WareHouse.Add(placedObject); break;
            case PlacaebleObjectCategories.Restaurant: Model.Instance.L_Restaurant.Add(placedObject); break;
            case PlacaebleObjectCategories.ConstructionSite: Model.Instance.L_ConstructionSite.Add(placedObject); break;
            case PlacaebleObjectCategories.BookStore: Model.Instance.L_BookStore.Add(placedObject); break;
            case PlacaebleObjectCategories.Arboretum: Model.Instance.L_ManholeCover.Add(placedObject); break;
            case PlacaebleObjectCategories.Rock: Model.Instance.L_Rock.Add(placedObject); break;
            case PlacaebleObjectCategories.Wall: Model.Instance.L_Wall.Add(placedObject); break;
            case PlacaebleObjectCategories.FoodResource: Model.Instance.L_FoodResource.Add(placedObject); break;
            case PlacaebleObjectCategories.MineResource: Model.Instance.L_MineResource.Add(placedObject); break;
            case PlacaebleObjectCategories.Light: Model.Instance.L_Lamp.Add(placedObject); break;

            case PlacaebleObjectCategories.BeanSproutFarm: Model.Instance.L_BeanSproutFarm.Add(placedObject); break;
            case PlacaebleObjectCategories.FishygrassFarm: Model.Instance.L_FishygrassFarm.Add(placedObject); break;
            case PlacaebleObjectCategories.MaizeFarm: Model.Instance.L_MaizeFarm.Add(placedObject); break;
            case PlacaebleObjectCategories.PeanutsFarm: Model.Instance.L_PeanutsFarm.Add(placedObject); break;
            case PlacaebleObjectCategories.PumpkinFarm: Model.Instance.L_PumpkinFarm.Add(placedObject); break;

            case PlacaebleObjectCategories.Trans: Model.Instance.L_Trans.Add(placedObject); break;
            case PlacaebleObjectCategories.Road: Model.Instance.L_Road.Add(placedObject); break;
        }
        //OnBuildingNumChanged.Invoke();
    }
    public void RemovePlacedObejct(PlaceableObject placedObject)
    {
        switch (placedObject.placeableObjectSO.category)
        {
            case PlacaebleObjectCategories.House: Model.Instance.L_House.Remove(placedObject); break;
            case PlacaebleObjectCategories.StudyRoom: Model.Instance.L_StudyRoom.Remove(placedObject); break;
            case PlacaebleObjectCategories.FoodFliter: Model.Instance.L_FoodFliter.Remove(placedObject); break;
            case PlacaebleObjectCategories.WareHouse: Model.Instance.L_WareHouse.Remove(placedObject); break;
            case PlacaebleObjectCategories.Restaurant: Model.Instance.L_Restaurant.Remove(placedObject); break;
            case PlacaebleObjectCategories.ConstructionSite: Model.Instance.L_ConstructionSite.Remove(placedObject); break;
            case PlacaebleObjectCategories.BookStore: Model.Instance.L_BookStore.Remove(placedObject); break;
            case PlacaebleObjectCategories.Arboretum: Model.Instance.L_ManholeCover.Remove(placedObject); break;
            case PlacaebleObjectCategories.Rock: Model.Instance.L_Rock.Remove(placedObject); break;
            case PlacaebleObjectCategories.Wall: Model.Instance.L_Wall.Remove(placedObject); break;
            case PlacaebleObjectCategories.FoodResource: Model.Instance.L_FoodResource.Remove(placedObject); break;
            case PlacaebleObjectCategories.MineResource: Model.Instance.L_MineResource.Remove(placedObject); break;
            case PlacaebleObjectCategories.Light: Model.Instance.L_Lamp.Remove(placedObject); break;

            case PlacaebleObjectCategories.BeanSproutFarm: Model.Instance.L_BeanSproutFarm.Remove(placedObject); break;
            case PlacaebleObjectCategories.FishygrassFarm: Model.Instance.L_FishygrassFarm.Remove(placedObject); break;
            case PlacaebleObjectCategories.MaizeFarm: Model.Instance.L_MaizeFarm.Remove(placedObject); break;
            case PlacaebleObjectCategories.PeanutsFarm: Model.Instance.L_PeanutsFarm.Remove(placedObject); break;
            case PlacaebleObjectCategories.PumpkinFarm: Model.Instance.L_PumpkinFarm.Remove(placedObject); break;

            case PlacaebleObjectCategories.Trans: Model.Instance.L_Trans.Remove(placedObject); break;
            case PlacaebleObjectCategories.Road: Model.Instance.L_Road.Remove(placedObject); break;
        }
        //OnBuildingNumChanged.Invoke();
    }
    private void SetFarmIncreaseRate(bool isIncrease, Vector2Int pos)
    {
        tmpFunction(pos.x - 1, pos.y, isIncrease);
        tmpFunction(pos.x + 1, pos.y, isIncrease);
        tmpFunction(pos.x - 1, pos.y + 1, isIncrease);
        tmpFunction(pos.x + 1, pos.y + 1, isIncrease);
        tmpFunction(pos.x - 1, pos.y - 1, isIncrease);
        tmpFunction(pos.x + 1, pos.y - 1, isIncrease);
        tmpFunction(pos.x, pos.y + 1, isIncrease);
        tmpFunction(pos.x, pos.y - 1, isIncrease);

    }
    private void tmpFunction(int x, int y, bool isIncrease)
    {
        if (!cur_grid.GetGridObject(x, y).CanBuild)
        {
            if (cur_grid.GetGridObject(x, y).PlaceableObject.placeableObjectSO.attribute == Attribute.Farm)
            {
                cur_grid.GetGridObject(x, y).PlaceableObject.FarmIncreaseRate = isIncrease ? 2 : 1;
            }
        }
    }

    
    public void ChangePreBuilding(PlacaebleObjectCategories placaebleObjectCategories)
    {
        if(cur_PlaceableObjectSO != D_PlacaebleObject[placaebleObjectCategories])
        {
            ChangeBuilding(D_PlacaebleObject[placaebleObjectCategories]);
        }
    }

    public void ChangeToUpWord()
    {
        if(isOnUpWord == false)
        {
            UpWord.gameObject.SetActive(true);
        }
    }
    public void ChangeToUnderWord()
    {
        if(isOnUpWord == true)
        {
            UpWord.gameObject.SetActive(false);
        }
    }
    public void ConvertPlayerMode(PlayerMode playerMode)
    {
        
        if(playerMode == PlayerMode.Build)
        {
            if (isOnUpWord)
            {
                BuildingParent = UpBuildingParent;
            }
            else
            {
                BuildingParent = UnderBuildingParent;
            }
        }
        if(cur_PlayerMode != playerMode)
        {
            cur_PlayerMode = playerMode;
        }
        OnPlayerModeChanged.Invoke(cur_PlayerMode);
    }
    public void ChangeBuilding(PlaceableObjectSO placeableObjectSO)
    {
        if (cur_PlaceableObjectSO != placeableObjectSO)
        {
            cur_PlaceableObjectSO = placeableObjectSO;
            OnBuildingChanged.Invoke();
        }
    }
    public void ChangeBuildingInEditMode(PlaceableObjectSO placeableObjectSO)
    {
        audioS.PlayOneShot(MouseClick);
        if (cur_PlayerMode != PlayerMode.Build)
        {
            ConvertPlayerMode(PlayerMode.Build);
        }
        if (cur_PlaceableObjectSO != placeableObjectSO)
        {
            if(placeableObjectSO.category == PlacaebleObjectCategories.House)
            {
                placeableObjectSO = (UnityEngine.Random.Range(0, 2) == 0) ? placeableObjectSO : Model.Instance.House2;
            }
            cur_PlaceableObjectSO = placeableObjectSO;
            OnBuildingChanged.Invoke();
        }
    }
    public void ChangeToBuildMode()
    {
        if (cur_PlayerMode != PlayerMode.Build && cur_PlayerMode != PlayerMode.UpWorldHandle)
        {
            ConvertPlayerMode(PlayerMode.Build);
        }
    }
    private void FoodAndMineResourceGenerate(int resourceNumber, PlaceableObjectSO resourceSO)
    {
        while (resourceNumber > 0)
        {
            int x = UnityEngine.Random.Range(0, GridBuildingSystem.Instance.rowCount - 1);
            int y = UnityEngine.Random.Range(0, GridBuildingSystem.Instance.columnCount - 1);
            if (cur_grid.GetGridObject(x, y).CanBuild)
            {
                PlaceBuilding(resourceSO, x, y, dir);
                resourceNumber--;
            }
        }
    }
    //Grid����
    public Quaternion GetCurrentBuildingRotation()
    {
        if (cur_PlaceableObjectSO != null)
        {
            return Quaternion.Euler(0, cur_PlaceableObjectSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        var mousePosition = MousePositionUtils.MouseToTerrainPosition();
        cur_grid.GetXZ(mousePosition, out int x, out int z);

        if (cur_PlaceableObjectSO != null)
        {
            var rotationOffset = cur_PlaceableObjectSO.GetRotationOffset(dir);
            return cur_grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * cur_grid.CellSize;
        }
        else
        {
            return mousePosition;
        }
    }
    public void SaveGrid()
    {
        SaveSystem.SaveObject("grid", GridBuildingSystem.Instance.grid);
        SaveSystem.SaveObject("Up_grid", GridBuildingSystem.Instance.Up_grid);
        //SaveSystem.SaveObject("Model", Model.Instance);
    }
    

    public void LoadGrid()
    {
        foreach (var gridObject in GridBuildingSystem.Instance.grid.GridArray)
        {
            if (gridObject.PlaceableObject != null)
            {
                RemovePlacedObejct(gridObject.PlaceableObject);
                gridObject.PlaceableObject.DestroySelf();
                gridObject.ClearPlaceableObject();
            }
        }
        foreach (var gridObject in GridBuildingSystem.Instance.Up_grid.GridArray)
        {
            if (gridObject.PlaceableObject != null)
            {
                RemovePlacedObejct(gridObject.PlaceableObject);
                gridObject.PlaceableObject.DestroySelf();
                gridObject.ClearPlaceableObject();
            }
        }
        BuildingParent = UpBuildingParent;
        RebuildGrid(GridBuildingSystem.Instance.Up_grid, "Up_grid");
        BuildingParent = UnderBuildingParent;
        RebuildGrid(GridBuildingSystem.Instance.grid, "grid");
        //RebuildGrid(GridBuildingSystem.Instance.pf_grid, "pf_grid");
        cur_grid = GridBuildingSystem.Instance.grid;
        GridBuildingSystem.Instance.pathFinding = new GridPathFinding(GridBuildingSystem.Instance.pf_grid);
        isOnLoad = false;
    }

    public void RebuildGrid(GridXZ<GridObject> grid, string loadedGrid)
    {
        var grid_tmp = SaveSystem.LoadSavedObject<GridXZ<GridObject>>(loadedGrid);
        //��ʱ����
        Vector3 tmpStartOrigin = Vector3.zero;
        string resourcePath = "";
        if (loadedGrid == "Up_grid")
        {
            tmpStartOrigin = GridBuildingSystem.Instance.UpStartOrigin;
            resourcePath = "Prefab/UpBuilding";           
        }
        else if (loadedGrid == "grid")
        {
            tmpStartOrigin = GridBuildingSystem.Instance.StartOrigin;
            resourcePath = "Prefab/Resource";
        }
        cur_grid = grid;
        grid = new GridXZ<GridObject>(grid_tmp.Width,
            grid_tmp.Height,
            grid_tmp.CellSize,
            tmpStartOrigin,         
            (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z),
            false);

        foreach (var gridObject in grid_tmp.GridArray)
        {
            var name = gridObject.PlaceableObjectName;
            if (!String.IsNullOrEmpty(name))
            {
                var placeableObjectSO = Resources.Load<PlaceableObjectSO>(resourcePath + $"/{name}_SO");
                if(placeableObjectSO == null)
                {
                    Debug.Log(name);
                    break;
                }
                PlaceBuilding(placeableObjectSO, gridObject.x, gridObject.z, grid_tmp.GetGridObject(gridObject.x, gridObject.z).Direction);
            }
        }
        
    }

    public void ReCalculatePath()
    {
        if (Model.Instance.L_FoodFliter.Count > 1)
        {
            foreach (var foodFilter in Model.Instance.L_FoodFliter)
            {
                if (foodFilter.isStartCollectResource == true)
                {
                    if (foodFilter.L_WorkingMouse.Count > 0)
                    {
                        var tmpMouse = foodFilter.L_WorkingMouse[0];
                        tmpMouse.CalculateWorkPath();
                        for (int i = 1; i < foodFilter.L_WorkingMouse.Count; i++)
                        {
                            foodFilter.L_WorkingMouse[i].BuildingToResourcePath = tmpMouse.BuildingToResourcePath;
                            foodFilter.L_WorkingMouse[i].ResourceToBuildingPath = tmpMouse.ResourceToBuildingPath;
                        }
                    }
                }
            }
        }
    }
}
