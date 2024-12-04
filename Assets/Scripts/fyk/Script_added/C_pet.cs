using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public enum StateType
{
    Idle,Run,Walk,Attack,Dead,Sit,Sleep,Bark,Shake,Jump,LookingOut,JumpUp
}
public enum petMode
{
    Idle,ChaseLight,focus
}
public class C_pet : MonoBehaviour
{
    public Animator animator;
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float stopDistance = 3.0f;
    
    public Transform target;
    [HideInInspector]
    public int animIndex = 1;
    [HideInInspector]
    public IState currentState;
    [HideInInspector]
    public petMode curMode;
    [HideInInspector]
    public Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TransitionState(StateType type)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    void comeHere(Transform target)
    {

    }
}

public interface IState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}

public class SitState : IState {
    private C_pet pet;
    public SitState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isSitting", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isSitting", false);
    }
}

public class WalkState : IState
{
    private C_pet pet;
    public WalkState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isWalking", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isWalking", false);
    }
}
public class IdleState : IState
{
    private C_pet pet;
    public IdleState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isIdling", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isIdling", false);
    }
}
public class RunState : IState
{
    private C_pet pet;
    public RunState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isRunning", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isRunning", false);
    }
}
public class DeadState : IState
{
    private C_pet pet;
    public DeadState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isDead", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isDead", false);
    }
}
public class SleepState : IState
{
    private C_pet pet;
    public SleepState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isSleeping", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isSleeping", false);
    }
}
public class AttackState : IState
{
    private C_pet pet;
    public AttackState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isAttacking", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isAttacking", false);
    }
}
public class BarkState : IState
{
    private C_pet pet;
    public BarkState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isBarking", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isBarking", false);
    }
}

public class ShakeState : IState
{
    private C_pet pet;
    public ShakeState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isShaking", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isShaking", false);
    }
}
public class JumpState : IState
{
    private C_pet pet;
    public JumpState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isJumping", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isJumping", false);
    }
}
public class LookOutState : IState
{
    private C_pet pet;
    public LookOutState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isLookingOut", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isLookingOut", false);
    }
}
public class JumpUpState : IState
{
    private C_pet pet;
    public JumpUpState(C_pet animationController)
    {
        this.pet = animationController;
    }
    public void OnEnter()
    {
        pet.animator.SetBool("isJumping_Up", true);
    }
    public void OnUpdate()
    {

    }
    public void OnExit()
    {
        pet.animator.SetBool("isJumping_Up", false);
    }
}
