using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DontDestroyGO<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static GameObject dontDestroyGO;

    static T ins;
    public static T Ins
    {
        get
        {
            if (ins == null) CreateDontDestroyGO();
            return ins;
        }
    }
    //创建挂载常存的游戏对象
    internal static void CreateDontDestroyGO()
    {
        if (dontDestroyGO == null) dontDestroyGO = GameObject.Find("DontDestroyGO");
        if (dontDestroyGO == null)
        {
            dontDestroyGO = new GameObject("DontDestroyGO");
            DontDestroyOnLoad(dontDestroyGO);
        }
        if (ins) return;
        ins = dontDestroyGO.GetComponent<T>();
        if (ins == null) ins = dontDestroyGO.AddComponent<T>();
    }
}
public class TimerTask : MonoBehaviour
{
    //延时时间
    public float delayed_time;
    //计时器，计时达到延时时间就执行事件
    public float timer;
    //是否重复执行
    public bool isRepeat;
    //事件当前状态：是暂停，还是继续
    public bool isPause;
    //事件
    public Action func;

    public TimerTask(float delayed_time, bool isRepeat, Action func)
    {
        this.delayed_time = delayed_time;
        timer = 0;
        this.isRepeat = isRepeat;
        this.isPause = false;
        this.func = func;
    }
}
//定时任务控制
public class TimerManager : DontDestroyGO<TimerManager>
{
    static List<TimerTask> timerTasks = new List<TimerTask>();

    void OnEnable()
    {
        Debug.Log("提醒：开启定时器...");
    }

    TimerTask taskTemp;
    void FixedUpdate()
    {
        //if (pause) return;
        if (timerTasks.Count < 1) return;

        for (int i = 0; i < timerTasks.Count; i++)
        {
            taskTemp = timerTasks[i];
            if (!taskTemp.isPause) taskTemp.timer += Time.fixedDeltaTime;
            if (taskTemp.timer >= taskTemp.delayed_time)
            {
                taskTemp.func.Invoke();
                if (!taskTemp.isRepeat)
                {
                    timerTasks.RemoveAt(i);
                    --i;
                    if (timerTasks.Count == 0) break;
                }
                else
                {
                    taskTemp.timer = 0;
                }
            }
        }

    }

    //添加固定更新定时事件
    /// <param name="time">定时时长</param>
    /// <param name="callback">回调事件</param>
    /// <param name="isrepeat">是否重复(不传入，则默认为不重复执行)</param>
    public static void Add(float delayedTime, Action func, bool isrepeat = false)
    {
        CreateDontDestroyGO();
        if (func != null)
        {
            bool isContain = false;
            for (int i = 0; i < timerTasks.Count; i++)
            {
                if (timerTasks[i].func.Equals(func))
                {
                    isContain = true;
                    break;
                }
            }
            if (!isContain) timerTasks.Add(new TimerTask(delayedTime, isrepeat, func));
        }
    }

    /// <summary>
    /// 暂停延时事件的计时
    /// </summary>
    /// <param name="callback"></param>
    public static void Pause(Action func)
    {
        if (func != null)
        {
            for (int i = 0; i < timerTasks.Count; i++)
            {
                var taskTemp = timerTasks[i];
                if (taskTemp.func.Equals(func))
                {
                    taskTemp.isPause = true;
                }
            }
        }

    }

    /// <summary>
    /// 结束事件的计时暂停状态
    /// </summary>
    /// <param name="callback"></param>
    public static void UnPause(Action func)
    {
        if (func != null)
        {
            for (int i = 0; i < timerTasks.Count; i++)
            {
                var taskTemp = timerTasks[i];
                if (taskTemp.func.Equals(func))
                {
                    taskTemp.isPause = false;
                }
            }
        }
    }

    /// <summary>
    /// 移除指定事件
    /// </summary>
    /// <param name="callback"></param>
    public static void Remove(Action func)
    {
        if (func != null)
        {
            for (int i = 0; i < timerTasks.Count; i++)
            {
                var taskTemp = timerTasks[i];
                if (taskTemp.func.Equals(func))
                {
                    timerTasks.Remove(taskTemp);
                }
            }
        }
    }
    /// <summary>
    /// 清空定时任务
    /// </summary>
    public static void Clear()
    {
        timerTasks.Clear();
    }
}
