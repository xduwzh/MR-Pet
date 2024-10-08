using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//下层建筑
public class BaseBuilding : MonoBehaviour
{


    //文字描述
    public string buildingInfo;
    public string buildingCost;
    
    //消耗材料量
    public int buildingCostFood;
    public int buildingCostMaterial;

    //产出量
    public int population;


    //图片
    public Image buildingImg;



    public BaseBuilding(string info,string cost,int costFood,int costMaterial,int peoNum)
    {
        buildingInfo = info;
        buildingCost = cost;
        buildingCostFood = costFood;
        buildingCostMaterial = costMaterial;
        population = peoNum;
    }

    public BaseBuilding(string info, string cost, int costFood, int costMaterial, int peoNum, Image img)
    {
        buildingInfo = info;
        buildingCost = cost;
        buildingCostFood = costFood;
        buildingCostMaterial = costMaterial;
        population = peoNum;

        buildingImg = img;
    }

    //建造消耗和产出
    public void buildThisBuilding()
    {
        //Model.Instance.FoodStore -= buildingCostFood;
        //Model.Instance.BuildingMaterials -= buildingCostMaterial;
        //Model.Instance.Population += population;

    }

}
