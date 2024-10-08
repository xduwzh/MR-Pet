using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Grid;

public class MouseManager : Singleton<MouseManager>, IDisposable
{
    // Start is called before the first frame update
    public Transform prefab;

    [HideInInspector] public List<C_Mouse> Idler_List;
    [HideInInspector] public List<C_Mouse> Logger_List;
    [HideInInspector] public List<C_Mouse> ConstructionWorker_List;
    [HideInInspector] public List<C_Mouse> IronMiner_List;
    [HideInInspector] public List<C_Mouse> StoneMiner_List;
    [HideInInspector] public List<C_Mouse> Farmer_List;

    public Action OnIdlerNumChanged;
    //资源采集时间
    [Header("资源采集时间")]
    public int CutTreeTime;
    public int MineIronTime;
    public int MineStoneTime;
    public int FarmTime;
    //资源存储时间
    [Header("资源存储时间")]
    public int TreeStoreTime;
    public int IronStoreTime;
    public int StoneStoreTime;
    public int FoodStoreTime;
    //单词资源采集数量
    [Header("单词资源采集数量")]
    public int TreeNumEachTime;
    public int IronNumEachTime;
    public int StoneNumEachTime;
    public int FoodNumEachTime;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 5; i++)
            {
                //var r = CreateNewResident();
                //Idler_List.Add(r);
                OnIdlerNumChanged.Invoke();
            }
        }
    }
    public void Dispose()
    {
    }
    //public Mouse CreateNewResident()
    //{
    //    Mouse mice = Instantiate(prefab, GridBuildingSystem.Instance.centerBuilding.transform.position , Quaternion.identity, this.transform).GetComponent<Resident>();
    //    return mice;
    //}
}

