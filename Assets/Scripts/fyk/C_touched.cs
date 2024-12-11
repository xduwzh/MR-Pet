using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_touched : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform UImanager;

    private int cleanStep = 0;
    private bool isbrushIn = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("brush"))
        {
            isbrushIn = true;
        }else if (other.CompareTag("hand"))
        {
            UImanager.GetComponent<C_UIManager>().CubeDebugger();
            UImanager.GetComponent<C_UIManager>().PetGetTouched();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 检查是否与指定目标物体碰撞
        if (other.CompareTag("brush"))
        {
            if (isbrushIn == true)
            {
                if(cleanStep == 1 || cleanStep == 2)
                {
                    UImanager.GetComponent<C_UIManager>().GenerateOneBubble();
                    isbrushIn = false;
                    cleanStep = 2;
                }
            }
        }
    }

    public void cleanStep1()
    {
        if(cleanStep == 0)
        {
            cleanStep = 1;
        }
    }
    public void cleanStep3()
    {
        if(cleanStep == 2)
        {
            UImanager.GetComponent<C_UIManager>().DestroyBubbles();
            UImanager.GetComponent<C_UIManager>().petCleaned();
            //UImanager.GetComponent<C_UIManager>().cleaningEnd();
            cleanStep = 0;
        }
    }
}
