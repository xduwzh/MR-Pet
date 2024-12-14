using Meta.XR.ImmersiveDebugger.UserInterface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class C_cat : C_pet
{
    // Start is called before the first frame update
    private float distance;
    private bool isPlaying;
    [HideInInspector]
    public bool isSleeping = false;
    private bool isStartGame;
    private bool FoodAppear;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float Timer;
    private Vector3 FoodPosition;

    public float stopDistance;
    public float startDistance;
    public float walkSpeed;
    public float runSpeed;
    public float timeToTired;
    public Transform debugPanel;

    public Transform CatToy;
    public GameObject UIManager;

    public AudioClip CatMeow;
    public AudioClip CatAttackMeow;
    public AudioClip CatSleep;
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
        states.Add(StateType.StandUp, new StandUpState(this));

        TransitionState(StateType.Idle);

        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        TransitionState(StateType.Idle);
    }

    // Update is called once per frame 
    protected override void Update()
    {
        base.Update();
        //debugPanel.GetComponent<TextMeshProUGUI>().text = "UpdateTest";
        //string text = "isSleeping: " + animator.GetBool("isSleeping") + " " +
        //    "isSitting: " + animator.GetBool("isSitting") + " " +
        //    "isIdling: " + animator.GetBool("isIdling") + " " +
        //    "isWalking: " + animator.GetBool("isWalking") + " " +
        //    "isRunning: " + animator.GetBool("isRunning") + " " +
        //    "isAttacking: " + animator.GetBool("isAttacking"); ;
        //debugPanel.GetComponent<TextMeshProUGUI>().text = text;

        if (isStartGame && !isSleeping)
        {
            targetPosition = new Vector3(CatToy.position.x, this.transform.position.y, CatToy.position.z);
            distance = Vector3.Distance(transform.position, targetPosition);
            if (distance > stopDistance)
            {
                transform.LookAt(targetPosition);
                if (currentState != states[StateType.Run])
                {
                    TransitionState(StateType.Run);
                }
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);
            }
            else
            {
                if (currentState != states[StateType.Attack])
                {
                    AudioSource.PlayClipAtPoint(CatAttackMeow, this.transform.position);
                    TransitionState(StateType.Attack);
                }
                isStartGame = false;
                isPlaying = true;
                Timer = timeToTired;
            }
        }else if (isPlaying && !isSleeping)
        {
            Timer -= Time.deltaTime;
            if(Timer <= 0)
            {
                AudioSource.PlayClipAtPoint(CatSleep, this.transform.position);
                TransitionState(StateType.Sleep);
                isSleeping = true;
                isPlaying = false;
                UIManager.GetComponent<C_UIManager>().PetPlayed();
            }
        }

        if(FoodAppear && isSleeping)
        {
            targetPosition = new Vector3(FoodPosition.x, this.transform.position.y, FoodPosition.z);
            distance = Vector3.Distance(transform.position, targetPosition);
            if (distance > stopDistance)
            {
                transform.LookAt(targetPosition);
                if (currentState == states[StateType.Sleep])
                {
                    TransitionState(StateType.Run);
                }
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if(stateInfo.IsName("Cat_Run"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);
                }
                //transform.position = Vector3.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);
            }
            else
            {
                if (currentState != states[StateType.Sit])
                {
                    AudioSource.PlayClipAtPoint(CatMeow, this.transform.position);
                    TransitionState(StateType.Sit);
                }
                FoodAppear = false;
                isSleeping = false;
            }
        }
    }
    public void AnimTestFunction()
    {
        debugPanel.GetComponent<TextMeshProUGUI>().text = "AnimTest";
        switch (animIndex)
        {
            case (1): TransitionState(StateType.Run); debugPanel.GetComponent<TextMeshProUGUI>().text = "run"; break;
            case (2): TransitionState(StateType.Attack); debugPanel.GetComponent<TextMeshProUGUI>().text = "Attack"; break;
            case (3): TransitionState(StateType.Idle); debugPanel.GetComponent<TextMeshProUGUI>().text = "Idle"; break;
            case (4): TransitionState(StateType.Walk); debugPanel.GetComponent<TextMeshProUGUI>().text = "Walk"; break;
            case (5): TransitionState(StateType.Sleep); debugPanel.GetComponent<TextMeshProUGUI>().text = "Sleep"; break;
            case (6): TransitionState(StateType.StandUp); debugPanel.GetComponent<TextMeshProUGUI>().text = "StandUp"; break;
            case (7): TransitionState(StateType.Sit); debugPanel.GetComponent<TextMeshProUGUI>().text = "Sit"; break;
            case (8): TransitionState(StateType.Idle); debugPanel.GetComponent<TextMeshProUGUI>().text = "Idle"; animIndex = 0; break;
        }
        animIndex++;
    }

    public void StartPlayGame(GameObject Toy)
    {
        if (!isSleeping)
        {
            AudioSource.PlayClipAtPoint(CatMeow, this.transform.position);
            if (currentState != states[StateType.Idle])
            {
                TransitionState(StateType.Idle);
            }
            Vector3 lookTarget = new Vector3(Toy.transform.position.x, this.transform.position.y, Toy.transform.position.z);
            transform.LookAt(lookTarget);
        }
    }
    public void startGame()
    {
        targetPosition = new Vector3(CatToy.position.x, this.transform.position.y, CatToy.position.z);
        distance = Vector3.Distance(transform.position, targetPosition);
        if (distance > startDistance)
        {
            isStartGame = true;
            startPosition = transform.position;
            //AudioSource.PlayClipAtPoint(CatMeow, this.transform.position);
        }
    }

    public void showFood(Vector3 foodPosition)
    {
        targetPosition = new Vector3(foodPosition.x, this.transform.position.y, foodPosition.z);
        distance = Vector3.Distance(transform.position, targetPosition);
        FoodAppear = true;
        startPosition = transform.position;
    }
}
