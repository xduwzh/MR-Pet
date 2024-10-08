using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MotionDoing : Singleton<MotionDoing>
{
    Queue<MotionClass> doingMotion = new Queue<MotionClass>();
    
    public TextMeshProUGUI doingInfoText;

    public GameObject dicePanel;
    public GameObject Canvas;
    public AudioSource diceSound;


    public void motionSetting(MotionClass motion)
    {
        motion.motionState = MotionClass.E_motionState.start;


        doingMotion.Enqueue(motion);


        //Debug.Log("正在执行的事件数量：" + doingMotion.Count);

        Canvas.GetComponent<CanvasGroup>().interactable = false;
        Dice.Instance.GoRotate();

        diceSound.Play();

        //UI停止交互
        //dice转
        //StartCoroutine（UI可以交互，dice停，传出结果，执行成功和失败）


        //todo : 丢骰子决定成功失败



        //冷却
        StartCoroutine("startCountDown",motion);
        StartCoroutine("waitDiceStop", motion);
    }




    public IEnumerator startCountDown(MotionClass motion)
    {
        motion.countTime = 20;

        while (motion.countTime >= 0)
        {
            motion.countTime--;//总时间 单位 秒，倒计时
            motion.doingInfo = " Time:" + motion.countTime;//时间显示UI            
            if (motion.countTime == 0)
            {
                doingMotion.Dequeue();
                //Debug.Log("任务结束");
                motion.motionState = MotionClass.E_motionState.rest;
                yield break;//停止 协程
            }
            else if (motion.countTime > 0)
            {
                yield return new WaitForSeconds(1);// 每次 自减1，等待 1 秒
            }
        }
    }


    public IEnumerator waitDiceStop(MotionClass motion)
    {
        int rotateTime = 2;

        while (rotateTime >= 0)
        {
            rotateTime--;//总时间 单位 秒，倒计时            
            if (rotateTime == 0)
            {
                //Debug.Log("骰子停止旋转");
                int diceNum = Dice.Instance.GetResult();
                
                Canvas.GetComponent<CanvasGroup>().interactable = true;
                if (diceNum >= motion.diceNum)
                {
                    motion.successAction();
                }
                else
                {
                    motion.failAction();
                }
                yield break;//停止 协程
            }
            else if (rotateTime > 0)
            {
                yield return new WaitForSeconds(1);// 每次 自减1，等待 1 秒
            }
        }
    }

    private void Update()
    {
        MotionClass[] doingMotionArr = doingMotion.ToArray();

        string Info = "";

        for(int i = 0;i < doingMotionArr.Length;i++)
        {
            Info += doingMotionArr[i].textIntroduce + doingMotionArr[i].doingInfo + "\n";
            
        }

        doingInfoText.text = "正在执行的事件数量 " + doingMotion.Count + "\n" + Info;


    }


}
