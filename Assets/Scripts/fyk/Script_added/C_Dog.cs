using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Dog : C_pet
{
    public Transform bone;
    public Transform romateBone;
    private float distance;
    private float distace2;
    private bool isGet = false;
    private float timer = 1;
    public Transform origalPosition;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        states.Add(StateType.Sit, new SitState(this));
        states.Add(StateType.Run, new RunState(this));
        states.Add(StateType.Walk, new WalkState(this));
        states.Add(StateType.Bark, new BarkState(this));

        TransitionState(StateType.Sit);
        curMode = petMode.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.T))
        {
            switch (animIndex)
            {
                case (1): TransitionState(StateType.Run); break;
                case (2): TransitionState(StateType.Walk); break;
                case (3): TransitionState(StateType.Bark); break;
            }
            animIndex++;
        }

        if (!isGet)
        {
            distance = Vector3.Distance(transform.position, target.position);
            if (distance > stopDistance)
            {
                transform.LookAt(target.position);
                if (currentState != states[StateType.Run])
                {
                    TransitionState(StateType.Run);
                }
                transform.position = Vector3.MoveTowards(transform.position, target.position, runSpeed * Time.deltaTime);
            }
            else
            {
                isGet = true;
                bone.gameObject.SetActive(true);
                romateBone.gameObject.SetActive(false);
                if (currentState != states[StateType.Bark])
                {
                    TransitionState(StateType.Bark);
                }
            }
        }
        else if(timer <= 0)
        {
            distance = Vector3.Distance(transform.position, origalPosition.position);
            if (distance > stopDistance)
            {
                transform.LookAt(origalPosition.position);
                if (currentState != states[StateType.Walk])
                {
                    TransitionState(StateType.Walk);
                }
                transform.position = Vector3.MoveTowards(transform.position, origalPosition.position, walkSpeed * Time.deltaTime);
            }
            else
            {
                if (currentState != states[StateType.Sit])
                {
                    TransitionState(StateType.Sit);
                }
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
        
    }
}
