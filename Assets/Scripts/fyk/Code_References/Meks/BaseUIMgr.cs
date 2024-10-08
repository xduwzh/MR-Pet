using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseUIMgr : MonoBehaviour
{
    BaseBuilding baseBuilding1 = new BaseBuilding("鼠鼠的家。人口 + 5",
                              "消耗建筑材料 * 30，食物 * 10", 10, 30, 5);

    BaseBuilding baseBuilding2 = new BaseBuilding("食物收集站。",
                              "消耗建筑材料 * 50，食物 * 5", 5, 50, 0);

    BaseBuilding baseBuilding3 = new BaseBuilding("仓库。",
                              "消耗建筑材料 * 40，食物 * 10", 10, 40, 0);

    BaseBuilding baseBuilding4 = new BaseBuilding("书屋。",
                      "消耗建筑材料 * 100，食物 * 10", 10, 30, 0);



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
