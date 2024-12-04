using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_AnimatiorTest : MonoBehaviour
{
    private Animator animator;
    private int animIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            switch (animIndex) {
                case (1): animator.SetBool("isWalking", true);break;
                case (2): animator.SetBool("isWalking", false); animator.SetBool("isSitting", true); break;
                case (3): animator.SetBool("isRunning", true); break;
                case (4): animator.SetBool("isIdling", true); break;
                case (5): animator.SetBool("isDead", true); break;
                case (6): animator.SetBool("isSleeping", true); break;
            }
            animIndex++;
        }
    }
}
