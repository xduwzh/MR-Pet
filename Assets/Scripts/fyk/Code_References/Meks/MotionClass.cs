using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MotionClass
{
    //你的点数
    public int uPoints = 0;


    //状态
    public enum E_motionState
    {
        start,
        rest,
    }

    public float countTime = 5;

    public string doingInfo;


    public E_motionState motionState = E_motionState.rest;

    //行动时间
    public float motionTime;

    //所需行动点数
    public int diceNum = 30;

    //获取的资源类型
    public ResourceType resourceType;

    //获取的资源量
    public int ResourceAmout;

    //可能降低的友好值
    public int IncreaseHateValue;

    //文字描述
    public string motionName;
    public string textIntroduce;
    public string textCost;

    


    public MotionClass(string name, string introduce, string cost, int num, ResourceType resourceType, int resourceAmout, int hateValue)
    {
        motionName = name;
        textIntroduce = introduce;
        textCost = cost;
        diceNum = num;
        this.resourceType = resourceType;
        this.ResourceAmout = resourceAmout;
        this.IncreaseHateValue = hateValue;
        refreshTExtCost();
    }

    public void refreshTExtCost()
    {
        textCost = "骰子点数>=" + diceNum + " , 成功奖励" + TopUIMgr.Instance.D_resourceTypeName[resourceType] + " * " + ResourceAmout + " , " + "失败增加" + IncreaseHateValue + "点人类对老鼠仇恨值";
    }

    //todo
    public void goMotion()
    {
        if(Model.Instance.DiceNum > 0)
        {
            Model.Instance.DiceNum -= 1;
            MotionDoing.Instance.motionSetting(this);
        }
        else
        {
            View.Instance.AddInfoPanel("提醒: ", "没有行动点数，无法执行事件，建造房子以获取行动点数");
        }     
    }

    public void successAction()
    {
        View.Instance.AddInfoPanel("提醒: ", this.motionName + "成功");
        switch (resourceType)
        {
            case (ResourceType.BuildingMaterials): Model.Instance.BuildingMaterials += ResourceAmout;break;
            case (ResourceType.Food): Model.Instance.FoodStore += ResourceAmout; break;
            case (ResourceType.Seed): Model.Instance.SeedStore += ResourceAmout; break;
            case (ResourceType.Knowledge): Model.Instance.KnowledgeLevel += ResourceAmout; break;
            default: break;
        }
    }

    public void failAction()
    {
        View.Instance.AddInfoPanel("提醒: ", this.motionName + "失败");
        Model.Instance.HateValue += IncreaseHateValue;
    }
}
