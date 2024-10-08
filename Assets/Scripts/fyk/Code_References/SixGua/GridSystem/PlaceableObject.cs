using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;
using Grid;
using CodeMonkey.Utils;
public class PlaceableObject : MonoBehaviour
{
    public PlaceableObjectSO placeableObjectSO;
    public PlaceableObjectSO.Dir dir;
    public Vector2Int origin;

    [HideInInspector]public bool isStartWorking = false;
    [HideInInspector] public bool isStartCollectResource = false;
    [HideInInspector] public bool isMouseLiving = true;
    private double timer;
    private int matureTime = 5;
    private double matureTimer;

    [HideInInspector] public int FoodAmount;
    [HideInInspector] public int ConstructionMaterialAmount;
    //当前建筑工作列表
    [HideInInspector] public List<C_Mouse> L_WorkingMouse;
    [HideInInspector] public PlaceableObject curBindingResource;
    [HideInInspector] public int FarmIncreaseRate;
    public static PlaceableObject Create(Vector3 worldPosition, Vector2Int origin, PlaceableObjectSO.Dir dir, PlaceableObjectSO placeableObjectSO, Transform parent)
    {
        var placedObjectTransform = Instantiate(
            placeableObjectSO.Prefab,
            worldPosition,
            Quaternion.Euler(0, placeableObjectSO.GetRotationAngle(dir), 0), parent);
        var placeableObject = placedObjectTransform.GetComponent<PlaceableObject>();
        placedObjectTransform.GetChild(0).Find("Anchor").gameObject.SetActive(false);
        placeableObject.placeableObjectSO = placeableObjectSO;
        placeableObject.origin = origin;
        placeableObject.dir = dir;

        // Build effects
        //placedObjectTransform.DOShakeScale(.5f, .2f, 10, 90, true);

        return placeableObject;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return placeableObjectSO.GetGridPositionList(origin, dir);
    }
    private void Start()
    {
        Controller.Instance.OnHouseBuilt += SearchWorkMouse;
        FoodAmount = placeableObjectSO.FoodAmount;
        ConstructionMaterialAmount = placeableObjectSO.ConstructionMaterialAmount;
        if (placeableObjectSO.attribute == Attribute.Farm)
        {
            gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            var gridPosList = this.GetGridPositionList();
            if (GetSuroundLight(gridPosList[0])){
                FarmIncreaseRate = 2;
            }
        }
        if(placeableObjectSO.category == PlacaebleObjectCategories.House)
        {
            isStartWorking = true;
        }
    }
    private bool GetSuroundLight(Vector2Int pos)
    {
        if(tmpFunction(pos.x - 1, pos.y) || 
        tmpFunction(pos.x + 1, pos.y) || 
        tmpFunction(pos.x - 1, pos.y + 1)||
        tmpFunction(pos.x + 1, pos.y + 1)||
        tmpFunction(pos.x - 1, pos.y - 1)||
        tmpFunction(pos.x + 1, pos.y - 1)||
        tmpFunction(pos.x, pos.y + 1)||
        tmpFunction(pos.x, pos.y - 1)){
            return true;
        }else
        {
            return false;
        }
    }
    private bool tmpFunction(int x, int y)
    {
        if (!GridBuildingSystem.Instance.grid.GetGridObject(x, y).CanBuild)
        {
            if (GridBuildingSystem.Instance.grid.GetGridObject(x, y).PlaceableObject.placeableObjectSO.category== PlacaebleObjectCategories.Light)
            {
                return true;
            }
        }
        return false;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    private void SearchWorkMouse()
    {
        if (placeableObjectSO.MouseNeeded != 0 && L_WorkingMouse.Count < placeableObjectSO.MouseNeeded)
        {
            int leakNum = placeableObjectSO.MouseNeeded - L_WorkingMouse.Count;
            for (int i = 0; i < leakNum; i++)
            {
                if (Model.Instance.Mouse_List.Count > 0) {
                    var tmp_Mouse = Model.Instance.Mouse_List[0];
                    L_WorkingMouse.Add(tmp_Mouse);
                    tmp_Mouse.cur_WorkingBuilding = this;
                    if (isStartCollectResource)
                    {
                        tmp_Mouse.cur_WorkingResource = curBindingResource;
                        tmp_Mouse.ConvertWorkType(WorkType.ResourceCollecter);
                    }
                    else
                    {
                        tmp_Mouse.ConvertWorkType(WorkType.StableBuildingWorker);
                    }                   
                    Model.Instance.Mouse_List.Remove(tmp_Mouse);
                    isStartWorking = true;
                }
            }
        }
    }
    private void Update()
    {
        //if (placeableObjectSO.category == PlacaebleObjectCategories.FoodResource)
        //{
        //    if (FoodAmount < 0 || FoodAmount == 0)
        //    {
        //        DestroySelf();
        //    }
        //}
        //if (placeableObjectSO.category == PlacaebleObjectCategories.MineResource)
        //{
        //    if (ConstructionMaterialAmount < 0 || ConstructionMaterialAmount == 0)
        //    {
        //        DestroySelf();
        //    }
        //}
        if (isStartWorking == true)
        {
            timer += Time.deltaTime;
            if(timer > Model.Instance.EachDayTime)
            {
                if(placeableObjectSO.attribute == Attribute.Farm)
                {
                    matureTimer += Time.deltaTime;
                    gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                    if (placeableObjectSO.attribute != Attribute.Farm) { matureTimer = matureTime + 1; }
                    if (matureTimer > matureTime)
                    {
                        matureTimer = 0;
                        timer = 0;
                        gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

                        switch (placeableObjectSO.attribute)
                        {
                            case Attribute.Farm:
                                Model.Instance.FoodStore += placeableObjectSO.foodProduceSpeed * FarmIncreaseRate;
                                UtilsClass.CreateWorldTextPopup("食物+" + placeableObjectSO.foodProduceSpeed * FarmIncreaseRate, gameObject.transform.position);
                                break;
                            case Attribute.UnderBuilding:
                                if (placeableObjectSO.category == PlacaebleObjectCategories.StudyRoom)
                                {
                                    Model.Instance.KnowledgeLevel += placeableObjectSO.KnowledgeIncrease;
                                }
                                else if (placeableObjectSO.category == PlacaebleObjectCategories.House)
                                {
                                    Model.Instance.DiceNum += 1;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    timer = 0;
                    if(placeableObjectSO.attribute == Attribute.UnderBuilding)
                    {
                        if (placeableObjectSO.category == PlacaebleObjectCategories.StudyRoom)
                        {
                            Model.Instance.KnowledgeLevel += placeableObjectSO.KnowledgeIncrease;
                        }
                        else if (placeableObjectSO.category == PlacaebleObjectCategories.House)
                        {
                            Model.Instance.DiceNum += 1;
                        }
                    }                   
                }
            }
        }
    }
}


