using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseUIMgr : MonoBehaviour
{
    BaseBuilding baseBuilding1 = new BaseBuilding("����ļҡ��˿� + 5",
                              "���Ľ������� * 30��ʳ�� * 10", 10, 30, 5);

    BaseBuilding baseBuilding2 = new BaseBuilding("ʳ���ռ�վ��",
                              "���Ľ������� * 50��ʳ�� * 5", 5, 50, 0);

    BaseBuilding baseBuilding3 = new BaseBuilding("�ֿ⡣",
                              "���Ľ������� * 40��ʳ�� * 10", 10, 40, 0);

    BaseBuilding baseBuilding4 = new BaseBuilding("���ݡ�",
                      "���Ľ������� * 100��ʳ�� * 10", 10, 30, 0);



    public void buttenBuild1()
    {
        baseBuilding1.buildThisBuilding();
    }

    public void buttenBuild2()
    {
        baseBuilding2.buildThisBuilding();
    }
    public void buttenBuild3()
    {
        baseBuilding3.buildThisBuilding();
    }
    public void buttenBuild4()
    {
        baseBuilding4.buildThisBuilding();
    }

}
