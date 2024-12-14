using Meta.XR.ImmersiveDebugger.UserInterface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class C_Dog : C_pet
{
    public float stopDistance;
    public float walkSpeed;
    public float runSpeed;
    public float startDistance;

    public Transform DogToy;
    public Transform MousePosition;
    public GameObject UIManager;

    private float distance;

    private bool isGet = true;
    private bool isStartGame = false;

    private Vector3 startPosition;
    private Vector3 targetPosition;


    public Transform debugPanel;

    public AudioClip DogBark;
    // Start is called before the first frame update

    private void Start()
    {
        base.Start();
        states.Add(StateType.Sit, new SitState(this));
        states.Add(StateType.Run, new RunState(this));
        states.Add(StateType.Walk, new WalkState(this));
        states.Add(StateType.Bark, new BarkState(this));
        states.Add(StateType.Idle, new IdleState(this));

        TransitionState(StateType.Idle);

        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        TransitionState(StateType.Idle);
    }

    private void Update()
    {
        base.Update();
        //debugPanel.GetComponent<TextMeshProUGUI>().text = "UpdateTest";
        //string text = "isWalking: " + animator.GetBool("isWalking") + " " +
        //    "isRunning: " + animator.GetBool("isRunning") + " " +
        //    "isBarking: " + animator.GetBool("isBarking") + " " +
        //    "isSitting: " + animator.GetBool("isSitting") + " " +
        //    "isIdling: " + animator.GetBool("isIdling");
        //debugPanel.GetComponent<TextMeshProUGUI>().text = text;

        if (!isGet && isStartGame && currentState != states[StateType.Bark])
        {
            targetPosition = new Vector3(DogToy.position.x, this.transform.position.y, DogToy.position.z);
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
                isGet = true;
                //DogToy.position = MousePosition.position;
                debugPanel.GetComponent<TextMeshProUGUI>().text = "SomethingWrong1";
                DogToy.position = MousePosition.position;
                DogToy.SetParent(MousePosition);
                DogToy.GetComponent<Rigidbody>().useGravity = false;
                //DogToy.position = new Vector3(0, 0, 0);
                if (currentState != states[StateType.Bark])
                {
                    AudioSource.PlayClipAtPoint(DogBark, this.transform.position);
                    TransitionState(StateType.Bark);
                }
                debugPanel.GetComponent<TextMeshProUGUI>().text = "SomethingWrong2";
            }
        }
        else if (isGet && isStartGame && currentState != states[StateType.Bark])
        {
            distance = Vector3.Distance(transform.position, startPosition);
            if (distance > stopDistance)
            {
                transform.LookAt(startPosition);
                if (currentState != states[StateType.Walk])
                {
                    TransitionState(StateType.Walk);
                }
                transform.position = Vector3.MoveTowards(transform.position, startPosition, walkSpeed * Time.deltaTime);
            }
            else
            {
                DogToy.parent = null;
                DogToy.GetComponent<Rigidbody>().useGravity = true;
                if (currentState != states[StateType.Bark] && currentState != states[StateType.Idle])
                {
                    AudioSource.PlayClipAtPoint(DogBark, this.transform.position);
                    TransitionState(StateType.Bark);
                }else if(currentState == states[StateType.Idle])
                {
                    TransitionState(StateType.Sit);
                    isStartGame = false;
                }
                UIManager.GetComponent<C_UIManager>().PetPlayed();
            }
        }
    }

    public void AnimTestFunction()
    {
        debugPanel.GetComponent<TextMeshProUGUI>().text = "AnimTest";
        switch (animIndex)
        {
            case (1): TransitionState(StateType.Run); debugPanel.GetComponent<TextMeshProUGUI>().text = "run"; break;
            case (2): TransitionState(StateType.Walk); debugPanel.GetComponent<TextMeshProUGUI>().text = "walk"; break;
            case (3): TransitionState(StateType.Bark); debugPanel.GetComponent<TextMeshProUGUI>().text = "bark"; break;
            case (4): TransitionState(StateType.Idle); debugPanel.GetComponent<TextMeshProUGUI>().text = "idle"; break;
            case (5): TransitionState(StateType.Sit); debugPanel.GetComponent<TextMeshProUGUI>().text = "sit"; animIndex = 0; break;
        }
        animIndex++;
    }

    public void StartPlayGame(GameObject Toy)
    {
        TransitionState(StateType.Bark);
        AudioSource.PlayClipAtPoint(DogBark, this.transform.position);
        Vector3 lookTarget = new Vector3(Toy.transform.position.x, this.transform.position.y, Toy.transform.position.z);
        transform.LookAt(lookTarget);
    }
    public void startGame()
    {
        targetPosition = new Vector3(DogToy.position.x, this.transform.position.y, DogToy.position.z);
        distance = Vector3.Distance(transform.position, targetPosition);
        if(distance > startDistance)
        {
            isGet = false;
            isStartGame = true;
            startPosition = transform.position;
            TransitionState(StateType.Bark);
            AudioSource.PlayClipAtPoint(DogBark, this.transform.position);
        }
    }
}