using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//�²㽨��
public class BaseBuilding : MonoBehaviour
{


    //��������
    public string buildingInfo;
    public string buildingCost;
    
    //���Ĳ�����
    public int buildingCostFood;
    public int buildingCostMaterial;

    //������
    public int population;


    //ͼƬ
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

    //�������ĺͲ���
    public void buildThisBuilding()
    {
        //Model.Instance.FoodStore -= buildingCostFood;
        //Model.Instance.BuildingMaterials -= buildingCostMaterial;
        //Model.Instance.Population += population;

    }

}
