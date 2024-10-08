using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBuilding 
{
    public string name;

    public int motionCounts;

    public MotionClass [] buildingMotions;

    public TopBuilding(string buildingName,params MotionClass[] motionArr)
    {
        name = buildingName;
        buildingMotions = motionArr;
        motionCounts = motionArr.Length;
    }
}
