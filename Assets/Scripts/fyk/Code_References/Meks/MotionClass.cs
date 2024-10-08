using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MotionClass
{
    //��ĵ���
    public int uPoints = 0;


    //״̬
    public enum E_motionState
    {
        start,
        rest,
    }

    public float countTime = 5;

    public string doingInfo;


    public E_motionState motionState = E_motionState.rest;

    //�ж�ʱ��
    public float motionTime;

    //�����ж�����
    public int diceNum = 30;

    //��ȡ����Դ����
    public ResourceType resourceType;

    //��ȡ����Դ��
    public int ResourceAmout;

    //���ܽ��͵��Ѻ�ֵ
    public int IncreaseHateValue;

    //��������
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
        textCost = "���ӵ���>=" + diceNum + " , �ɹ�����" + TopUIMgr.Instance.D_resourceTypeName[resourceType] + " * " + ResourceAmout + " , " + "ʧ������" + IncreaseHateValue + "�������������ֵ";
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
            View.Instance.AddInfoPanel("����: ", "û���ж��������޷�ִ���¼������췿���Ի�ȡ�ж�����");
        }     
    }

    public void successAction()
    {
        View.Instance.AddInfoPanel("����: ", this.motionName + "�ɹ�");
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
        View.Instance.AddInfoPanel("����: ", this.motionName + "ʧ��");
        Model.Instance.HateValue += IncreaseHateValue;
    }
}
