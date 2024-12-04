using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Rabit : C_pet
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        states.Add(StateType.LookingOut, new LookOutState(this));
        states.Add(StateType.Run, new RunState(this));
        states.Add(StateType.Jump, new JumpState(this));
        states.Add(StateType.JumpUp, new JumpUpState(this));
        states.Add(StateType.Dead, new DeadState(this));

        TransitionState(StateType.LookingOut);
        curMode = petMode.Idle;
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
                case (2): TransitionState(StateType.Jump); break;
                case (3): TransitionState(StateType.JumpUp); break;
                case (4): TransitionState(StateType.Dead); break;
            }
            animIndex++;
        }
    }
}
