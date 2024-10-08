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


        //Debug.Log("����ִ�е��¼�������" + doingMotion.Count);

        Canvas.GetComponent<CanvasGroup>().interactable = false;
        Dice.Instance.GoRotate();

        diceSound.Play();

        //UIֹͣ����
        //diceת
        //StartCoroutine��UI���Խ�����diceͣ�����������ִ�гɹ���ʧ�ܣ�


        //todo : �����Ӿ����ɹ�ʧ��



        //��ȴ
        StartCoroutine("startCountDown",motion);
        StartCoroutine("waitDiceStop", motion);
    }




    public IEnumerator startCountDown(MotionClass motion)
    {
        motion.countTime = 20;

        while (motion.countTime >= 0)
        {
            motion.countTime--;//��ʱ�� ��λ �룬����ʱ
            motion.doingInfo = " Time:" + motion.countTime;//ʱ����ʾUI            
            if (motion.countTime == 0)
            {
                doingMotion.Dequeue();
                //Debug.Log("�������");
                motion.motionState = MotionClass.E_motionState.rest;
                yield break;//ֹͣ Э��
            }
            else if (motion.countTime > 0)
            {
                yield return new WaitForSeconds(1);// ÿ�� �Լ�1���ȴ� 1 ��
            }
        }
    }


    public IEnumerator waitDiceStop(MotionClass motion)
    {
        int rotateTime = 2;

        while (rotateTime >= 0)
        {
            rotateTime--;//��ʱ�� ��λ �룬����ʱ            
            if (rotateTime == 0)
            {
                //Debug.Log("����ֹͣ��ת");
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
                yield break;//ֹͣ Э��
            }
            else if (rotateTime > 0)
            {
                yield return new WaitForSeconds(1);// ÿ�� �Լ�1���ȴ� 1 ��
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

        doingInfoText.text = "����ִ�е��¼����� " + doingMotion.Count + "\n" + Info;


    }


}
