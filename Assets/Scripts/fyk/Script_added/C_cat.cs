using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_cat : C_pet
{
    // Start is called before the first frame update
    private float distance;
    protected override void Start()
    {
        base.Start();
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Run, new RunState(this));
        states.Add(StateType.Walk, new WalkState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Dead, new DeadState(this));
        states.Add(StateType.Sit, new SitState(this));
        states.Add(StateType.Sleep, new SleepState(this));

        TransitionState(StateType.Idle);
        curMode = petMode.ChaseLight;
    }

    // Update is called once per frame 
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.T))
        {
            switch (animIndex)
            {
                case (1): TransitionState(StateType.Run); break;
                case (2): TransitionState(StateType.Walk); break;
                case (3): TransitionState(StateType.Attack); break;
                case (4): TransitionState(StateType.Sleep); break;
                case (5): TransitionState(StateType.Sit); break;
                case (6): TransitionState(StateType.Dead); break;
            }
            animIndex++;
        }

        distance = Vector3.Distance(transform.position, target.position);
        if (distance > stopDistance)
        {
            transform.LookAt(target.position);
            if(currentState != states[StateType.Run])
            {
                TransitionState(StateType.Run);
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, runSpeed * Time.deltaTime);
        }
        else
        {
            if(curMode == petMode.ChaseLight)
            {
                if (currentState != states[StateType.Attack])
                {
                    TransitionState(StateType.Attack);
                }
            }
            else
            {
                if (currentState != states[StateType.Idle])
                {
                    TransitionState(StateType.Idle);
                }
            }
        }
    }

}
