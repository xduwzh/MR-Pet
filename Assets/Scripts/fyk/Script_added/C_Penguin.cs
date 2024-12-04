using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Penguin : C_pet
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        states.Add(StateType.Dead, new DeadState(this));
        states.Add(StateType.Run, new RunState(this));
        states.Add(StateType.Walk, new WalkState(this));
        states.Add(StateType.Shake, new ShakeState(this));

        TransitionState(StateType.Shake);
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
                case (3): TransitionState(StateType.Run); break;
            }
            animIndex++;
        }
    }
}
